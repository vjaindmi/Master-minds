using System;
using System.Collections.Generic;
using System.IO;
using FaceMeApp.DependencyServices;
using FaceMeApp.ServiceLayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class LandingPage : ContentPage
    {
        MediaFile imageFile = null;
        public LandingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);

            btnSubmit.IsEnabled = false;
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

            //saving image to local directory
            DependencyService.Get<IPersistStoreService>().SaveImageToLocalDirectory(ReadFully(file.GetStream()));
        }

        /// <summary>
        /// Submits the clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Submit_Clicked(object sender, System.EventArgs e)
        {
            if (imageFile != null)
            {
                //IDataService service = new DataService();
                //var result=  await service.UploadAndDetectFaces(imageFile);
                //if (result)
                //{
                //    DependencyService.Get<IPersistStoreService>().saveUserData("Registered");  
                //    await Navigation.PushAsync(new SubmitAttendancePage());
                //}
                //else
                //{
                //    await DisplayAlert("Alert!", "Invalid Image", "OK");
                //}
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
