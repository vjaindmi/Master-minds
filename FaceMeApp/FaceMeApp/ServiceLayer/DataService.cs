using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FaceMeApp.Model;
using Newtonsoft.Json;

namespace FaceMeApp.ServiceLayer
{
    public class DataService
    {
        async public Task<EmployeeDetail> GetEmployeeDetails()
        {
            var uri = new Uri("");

            EmployeeDetail employeeDetails = null;

            using (var client = HttpHelper.GetHttpClient())
            {
                try
                {
                    var responseMessage = await client.GetAsync(uri);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var content = responseMessage.Content.ReadAsStringAsync().Result;

                        employeeDetails = JsonConvert.DeserializeObject<EmployeeDetail>(content);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return employeeDetails;
        }


        async public Task<bool> UpdateEmployeeDetails(EmployeeDetail request)
        {
            var uri = new Uri("");

            bool updateFlag = false;

            using (var client = HttpHelper.GetHttpClient())
            {
                try
                {
                    var requestJson = JsonConvert.SerializeObject(request);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpHelper.RequestFormat);

                    var responseMessage = await client.PutAsync(uri, requestContent);
                    updateFlag = responseMessage.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                }
            }
            return updateFlag;
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
        //async Task<bool> IDataService.UploadAndDetectFaces(MediaFile imageFile)
        //{
        //    CommonHelper.ShowLoader();
        //    // The list of Face attributes to return.
        //    IEnumerable<FaceAttributeType> faceAttributes =
        //        new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

        //    // Call the Face API.
        //    try
        //    {
        //        using (Stream imageFileStream = imageFile.GetStream())
        //        {
        //            var face = await faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
        //            if (face != null && face.Length > 0)
        //                return true;
        //            else
        //                return false;
        //        }
        //    }
        //    // Catch and display Face API errors.
        //    catch (FaceAPIException f)
        //    {
        //        Debug.WriteLine(f.ErrorMessage, f.ErrorCode);
        //        return false;
        //    }
        //    // Catch and display all other errors.
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message, "Error");
        //        return false;
        //    }
        //    finally
        //    {
        //        CommonHelper.DismissLoader();
        //    }
        //}

    }
}
