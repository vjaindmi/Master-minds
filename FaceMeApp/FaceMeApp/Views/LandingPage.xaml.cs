using System;
using System.Collections.Generic;
using System.IO;
using FaceMeApp.DependencyServices;
using FaceMeApp.Helper;
using FaceMeApp.Model;
using FaceMeApp.ServiceLayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class LandingPage : ContentPage
    {
        MediaFile imageFile = null;
        EmployeeDetail _employeeDetail = null;
        public LandingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);

            btnSubmit.IsEnabled = true;
        }
        public LandingPage(EmployeeDetail empModel)
        {
            try{
                InitializeComponent();
                _employeeDetail = empModel;
                NavigationPage.SetHasNavigationBar(this, false);

                btnSubmit.IsEnabled = true;
                if (!ReferenceEquals(_employeeDetail, null))
                {
                    lblEmpName.Text = _employeeDetail.FirstName + " " + _employeeDetail.LastName;
                    lblDesignation.Text = _employeeDetail.Technology;
                }  
            }
            catch (Exception ex)
            {
                
            }
        }

        void Camera_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => OpenCamera());
               
        }

        private async void OpenCamera()
        {
            imageFile = null;
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            // File is not null
            //Showing image
           
            btnSubmit.IsEnabled = true;

            imageFile = file;

            imgUser.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            ////saving image to local directory
            //DependencyService.Get<IPersistStoreService>().SaveImageToLocalDirectory(ReadFully(file.GetStream()));
        }

        /// <summary>
        /// Submits the clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Submit_Clicked(object sender, System.EventArgs e)
        {
         //var fil=   DependencyService.Get<IPersistStoreService>().GetBytes();
            if (imageFile != null && _employeeDetail.EmployeeId>0)
            {
                var bytes = ReadFully(imageFile.GetStream());

                var files = DependencyService.Get<IPersistStoreService>().ResizeImageAndroid(bytes,100,100);

                _employeeDetail.ImageContent = files;

                DataService service = new DataService();
                //var result = service.UpdateEmployeeDetails(_employeeDetail);
                //                  if(result.Result)
                //{
                //    DependencyService.Get<IPersistStoreService>().saveUserData("Registered");
                //}
                // else
                   
                //{
                //    Device.BeginInvokeOnMainThread(() => CommonHelper.ShowAlert("Failed to Upload Details!")); 
                //}

                var result=  await service.UploadAndDetectFaces(imageFile.GetStream());
                if (result)
                {
                    DependencyService.Get<IPersistStoreService>().SaveImageToLocalDirectory(ReadFully(imageFile.GetStream()));

                    //DependencyService.Get<IPersistStoreService>().saveUserData("Registered");  
                    //await Navigation.PushAsync(new SubmitAttendancePage("D0:22:BE:40:6A:70"));
                    await Navigation.PushAsync(new SubmitAttendancePage(_employeeDetail));

                }
                else
                {
                    await DisplayAlert("Alert!", "Invalid Image", "OK");
                }
            }
            else
            {
                CommonHelper.ShowAlert("Invalid Request");
            }
           
        }

        /// <summary>
        /// Reads the fully.
        /// </summary>
        /// <returns>The fully.</returns>
        /// <param name="input">Input.</param>
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    } 
}
