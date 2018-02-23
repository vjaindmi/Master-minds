using System;
using System.Windows.Input;
using FaceMeApp.DependencyServices;
using FaceMeApp.Helper;
using Xamarin.Forms;

namespace FaceMeApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand GuestUserCommand { get; set; }

        private string _mobileNumber;
        private string _password;

        private INavigation _navigation;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                _mobileNumber = value;
                OnPropertyChanged("MobileNumber");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new Command(LoginProcess);
            MobileNumber = "OnePlus5";
            Password = "Rajat@2012";
        }

        /// <summary>
        /// Logins the process.
        /// </summary>
        void LoginProcess()
        {
            try
            {
                CommonHelper.ShowLoader();
                if (IsValid())
                {
                    var macAddress = DependencyService.Get<IPersistStoreService>().ConnectWpa(MobileNumber, Password);
                    //if (!string.IsNullOrEmpty(macAddress))
                    //{
                        var imagePath = DependencyService.Get<IPersistStoreService>().getUserData();

                        //if(!string.IsNullOrEmpty(imagePath))
                        //    App.Current.MainPage = new NavigationPage(new Views.SubmitAttendancePage(macAddress));
                        //else
                            App.Current.MainPage = new NavigationPage(new Views.EmplyeeRegisterPage(macAddress));
                    //}
                    //else
                        //Device.BeginInvokeOnMainThread(() => CommonHelper.ShowAlert("Failed to connect, Try again later!"));

                }
                else
                    Device.BeginInvokeOnMainThread(() => CommonHelper.ShowAlert("Invalid Details"));

            }
            catch (Exception ex)
            {

            }
            finally
            {
                CommonHelper.DismissLoader();
            }

        }

        bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(MobileNumber))
            {
                isValid = false;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
