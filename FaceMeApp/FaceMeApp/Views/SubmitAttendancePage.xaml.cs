using System;
using System.Collections.Generic;
using FaceMeApp.DependencyServices;
using FaceMeApp.Model;
using FaceMeApp.ServiceLayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class SubmitAttendancePage : ContentPage
    {
        static bool status =false;
        MediaFile imageFile = null;
        string _macAddress = string.Empty;
        EmployeeDetail _employeeDetail = null;
        public SubmitAttendancePage(EmployeeDetail employeeDetail)
        {
            _employeeDetail = employeeDetail;
            _macAddress = employeeDetail.MACAddress;
            InitializeComponent();
            btnSubmit.IsEnabled = false;
            status = false;
        }

        void Camera_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => OpenCamera());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!status)
                btnSubmit.Text = "Check In";
            else
                btnSubmit.Text = "Check Out";

            lblEmpName.Text = _employeeDetail.FirstName + " " + _employeeDetail.LastName;
            lblDesignation.Text = _employeeDetail.Technology;
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
                IDataService service = new DataService();
                var result = await service.VerifyFace(imageStream, imageFile.GetStream());
                if (result)
                {
                    var employeeId = DependencyService.Get<IPersistStoreService>().getEmployeeId();
                    var type = (!status) ? 0 : 1;

                    var dataService = new DataService();
                    var employeeDetail = new EmployeeDetail();
                    employeeDetail.EmployeeId = Convert.ToInt32(employeeId) ;
                    employeeDetail.CheckInType = type;
                    if (!status)
                    {
                        employeeDetail.InTime = DateTime.Now;
                    }
                    else
                    {
                        employeeDetail.OutTime = DateTime.Now;
                    }

                    var detail = await dataService.UpdateEmployeeDetail(employeeDetail);
                    if (detail != null)
                    {
                        status = !status;
                        imgUser.Source = "userPlaceholder.jpg";
                        await Navigation.PushAsync(new AttendanceSummaryPage(employeeDetail));
                    }
                   // await DisplayAlert("Alert!", "Verified face successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Alert!", "Invalid Image found, Please try again.", "OK");
                }

            }
        }
    }

}
