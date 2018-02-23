using System;
using System.Collections.Generic;
using FaceMeApp.DependencyServices;
using FaceMeApp.ServiceLayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class SubmitAttendancePage : ContentPage
    {
        MediaFile imageFile = null;
        public SubmitAttendancePage()
        {
            InitializeComponent();
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
        }

        /// <summary>
        /// Submits the clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Submit_Clicked(object sender, System.EventArgs e)
        {
            var imagePath = DependencyService.Get<IPersistStoreService>().GetImageFromLocalDirectory();

            var imageStream = DependencyService.Get<IPersistStoreService>().GetJpgStreamFromPath(imagePath);

            if (imageFile != null)
            {
                //IDataService service = new DataService();
                //var result = await service.VerifyFace(imageStream, imageFile.GetStream());
                //if (result)
                //{
                //    await DisplayAlert("Alert!", "Verified face successfully!", "OK");
                //}
                //else
                //{
                //    await DisplayAlert("Alert!", "Invalid Image found, Please try again.", "OK");
                //}

            }
        }
    }

}
