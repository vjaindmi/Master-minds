using System;
using System.Windows.Input;
using FaceMeApp.DependencyServices;
using FaceMeApp.Helper;
using FaceMeApp.ServiceLayer;
using Xamarin.Forms;

namespace FaceMeApp.ViewModel
{
    public class EmployeeRegistrationViewModel:BaseViewModel
    {
        public ICommand ContinueCommand { get; set; }
        private INavigation _navigation;
        private string _employeeID;
        private string _macAddress = string.Empty;

        public string EmployeeID
        {
            get { return _employeeID; }
            set
            {
                _employeeID = value;
                OnPropertyChanged("EmployeeID");
            }
        }
        public EmployeeRegistrationViewModel(INavigation navigation, string MacAddress)
        {
            _macAddress = MacAddress;
            _navigation = navigation;
            ContinueCommand = new Command(GetEmployeeDetails);
        }
        /// <summary>
        /// Logins the process.
        /// </summary>
        async void GetEmployeeDetails()
        {
            try
            {
               
                if (IsValid())
                {

                    DataService service = new DataService();
                    var result=  await service.GetEmployeeDetails(EmployeeID,_macAddress);
                    if(result.EmployeeId>0)
                    {
                        DependencyService.Get<IPersistStoreService>().saveEmployeeId(EmployeeID);
                      var path=  DependencyService.Get<IPersistStoreService>().getImagePath();
                        if(!string.IsNullOrEmpty(path))
                            App.Current.MainPage = new NavigationPage(new Views.SubmitAttendancePage(result));
                        else
                        App.Current.MainPage = new NavigationPage(new Views.LandingPage(result));
                    }
                }

                else
                    Device.BeginInvokeOnMainThread(() => CommonHelper.ShowAlert("Invalid Employee ID"));

            }
            catch (Exception ex)
            {

            }
            finally
            {
                
            }

        }
        bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(EmployeeID))
            {
                isValid = false;
            }
           
            return isValid;
        }
    }
}
