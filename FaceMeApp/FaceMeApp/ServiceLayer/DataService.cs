using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FaceMeApp.Helper;
using FaceMeApp.Model;
using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;

namespace FaceMeApp.ServiceLayer
{
    public class DataService:IDataService
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient(Contants.Key1, Contants.BASE_URL);
        public DataService()
        {

        }
        async public Task<EmployeeDetail> GetEmployeeDetails(string employeeId, string macAddress)
        {
            CommonHelper.ShowLoader();
            var url = String.Format("http://14.140.104.62/faceAuth/GetEmployeeInfo?EmployeeId={0}&MACAddress={1}", employeeId, "D0:22:BE:40:6A:70");
            var uri = new Uri(url);

            EmployeeDetail employeeDetails = null;

            //using (var client = HttpHelper.GetHttpClient())
            //{
                try
                {
                 var client = HttpHelper.GetHttpClient();
                    await client.GetAsync(uri).ContinueWith( (arg) => {
                        var responseMessage = arg.Result;
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var content = responseMessage.Content.ReadAsStringAsync().Result;
                            using (StreamReader reader = new StreamReader(responseMessage.Content.ReadAsStreamAsync().Result))
                            {
                                var contents = reader.ReadToEnd();
                                JObject jObj = JObject.Parse(contents);

                            employeeDetails = jObj["EmployeeDetail"].ToObject<EmployeeDetail>();
                                //employeeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                            }
                        }
                    });

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    CommonHelper.DismissLoader();
                }
            //}
            return employeeDetails;
        }


        async public Task<bool> UpdateEmployeeDetails(EmployeeDetail request)
        {
            CommonHelper.ShowLoader();
            var uri = new Uri("http://14.140.104.62/faceAuth/UploadImage");

            bool updateFlag = false;

            using (var client = HttpHelper.GetHttpClient())
            {
                try
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StringContent(request.Id.ToString()), "Id");
                        content.Add(new StringContent("D0:22:BE:40:6A:70"), "MACAddress");
                        content.Add(new StringContent(request.DeviceId), "DeviceId");
                        content.Add(new StringContent(request.EmployeeId.ToString()), "EmployeeId");

                        content.Add(new StreamContent(new MemoryStream(request.ImageContent)), "img1");

                        var requestJson = JsonConvert.SerializeObject(request);
                        var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpHelper.RequestFormat);

                        await client.PostAsync(uri, content).ContinueWith((arg) => 
                        {
                            var responseMessage = arg.Result;
                            updateFlag = responseMessage.IsSuccessStatusCode;
                           
                            var returnContent = responseMessage.Content.ReadAsStringAsync().Result;
                            using (StreamReader reader = new StreamReader(responseMessage.Content.ReadAsStreamAsync().Result))
                            {
                                var contents = reader.ReadToEnd();
                                JObject jObj = JObject.Parse(contents);

                               var employeeDetails = jObj["EmployeeDetail"].ToObject<EmployeeDetail>();
                                //employeeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                            }
                        });

                    }
                   
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    CommonHelper.DismissLoader();
                }
            }
            return updateFlag;
        }

        async public Task<EmployeeDetail> UpdateEmployeeDetail(EmployeeDetail request)
        {
            CommonHelper.ShowLoader();

            var uri = new Uri("http://14.140.104.62/faceAuth/VerifyFace");

            EmployeeDetail employeeDetails = null;
            JObject j = new JObject();
            if (request.InTime != null)
                j.Add("InTime", request.InTime.ToString());
            if (request.OutTime != null)
                 j.Add("OutTime", request.OutTime.ToString());
            j.Add("MACAddress", "D0:22:BE:40:6A:70");
            j.Add("EmployeeId", request.EmployeeId);
            j.Add("CheckInType", request.CheckInType);

            var json = JsonConvert.SerializeObject(j);
            var contentRequest = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = HttpHelper.GetHttpClient())
            {
                try
                {
                    //var requestJson = JsonConvert.SerializeObject(request);
                    //var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpHelper.RequestFormat);

                    var responseMessage = await client.PostAsync(uri, contentRequest);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var content = responseMessage.Content.ReadAsStringAsync().Result;

                        using (StreamReader reader = new StreamReader(responseMessage.Content.ReadAsStreamAsync().Result))
                        {
                            var contents = reader.ReadToEnd();
                            JObject jObj = JObject.Parse(contents);

                             employeeDetails = jObj["EmployeeDetail"].ToObject<EmployeeDetail>();
                            //employeeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    CommonHelper.DismissLoader();
                }
            }
            return employeeDetails;

            //using (var client = HttpHelper.GetHttpClient())
            //{
                //try
                //{
                //    using (var content = new MultipartFormDataContent())
                //    {
                //        content.Add(new StringContent(request.Id.ToString()), "Id");
                //        content.Add(new StringContent(request.MACAddress), "MACAddress");
                //        content.Add(new StringContent(request.DeviceId), "DeviceId");
                //        content.Add(new StringContent(request.EmployeeId.ToString()), "EmployeeId");

                //        var requestJson = JsonConvert.SerializeObject(request);
                //        var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpHelper.RequestFormat);

                //        await client.PostAsync(uri, content).ContinueWith((arg) =>
                //        {
                //            var responseMessage = arg.Result;
                //            updateFlag = responseMessage.IsSuccessStatusCode;

                //            var returnContent = responseMessage.Content.ReadAsStringAsync().Result;
                //            using (StreamReader reader = new StreamReader(responseMessage.Content.ReadAsStreamAsync().Result))
                //            {
                //                var contents = reader.ReadToEnd();
                //                JObject jObj = JObject.Parse(contents);

                //                employeeDetails = jObj["EmployeeDetail"].ToObject<EmployeeDetail>();
                //                //employeeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                //            }
                //        });

                //    }

                //}
            //    catch (Exception ex)
            //    {
            //    }
            //    finally
            //    {
            //        CommonHelper.DismissLoader();
            //    }
            //}
        }

        async public Task<EmployeeDetail> VerifyEmployeeDetails(EmployeeDetail request)
        {
            var uri = new Uri("");

            EmployeeDetail employeDetails = null;

            using (var client = HttpHelper.GetHttpClient())
            {
                try
                {
                    var requestJson = JsonConvert.SerializeObject(request);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpHelper.RequestFormat);

                    var responseMessage = await client.PostAsync(uri, requestContent);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var content = responseMessage.Content.ReadAsStringAsync().Result;

                        employeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return employeDetails;

        }

        /// <summary>
        /// Faces the recognition demo. services. identifier ata service. upload and detect faces.
        /// </summary>
        /// <returns>The recognition demo. services. identifier ata service. upload and detect faces.</returns>
        /// <param name="imageFilePath">Image file path.</param>
        public async Task<bool> UploadAndDetectFaces(Stream imageStream)
        {
            CommonHelper.ShowLoader();
            // The list of Face attributes to return.
            IEnumerable<FaceAttributeType> faceAttributes =
                new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

            // Call the Face API.
            try
            {
                //using (Stream imageFileStream = imageFile.GetStream())
                //{
                var face = await faceServiceClient.DetectAsync(imageStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
                    if (face != null && face.Length > 0)
                        return true;
                    else
                        return false;
                //}
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Debug.WriteLine(f.ErrorMessage, f.ErrorCode);
                return false;
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, "Error");
                return false;
            }
            finally
            {
                CommonHelper.DismissLoader();
            }
        }

        async Task<bool> IDataService.VerifyFace(Stream registeredFace, Stream faceToVerify)
        {
            bool ret = false;
            CommonHelper.ShowLoader();
            try
            {
                Guid faceid1;
                Guid faceid2;

                using (Stream faceimagestream = faceToVerify)
                {
                    var faces = await faceServiceClient.DetectAsync(faceimagestream, returnFaceId: true);
                    if (faces.Length > 0)
                        faceid2 = faces[0].FaceId;
                    else
                        throw new Exception("No face found in image 2.");
                }

                // Detect the face in each image - need the FaceId for each
                using (Stream faceimagestream = registeredFace)
                {
                    var faces = await faceServiceClient.DetectAsync(faceimagestream, returnFaceId: true);
                    if (faces.Length > 0)
                        faceid1 = faces[0].FaceId;
                    else
                        throw new Exception("No face found in image 1.");
                }


                // Verify the faces
                var result = await faceServiceClient.VerifyAsync(faceid1, faceid2);
                ret = result.IsIdentical;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                CommonHelper.DismissLoader();
            }
            return ret;
        }
    }
}
