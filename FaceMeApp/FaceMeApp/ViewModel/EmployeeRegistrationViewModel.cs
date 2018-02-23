using System;
using System.Windows.Input;
using FaceMeApp.Helper;
using Xamarin.Forms;

namespace FaceMeApp.ViewModel
{
    public class EmployeeRegistrationViewModel:BaseViewModel
    {
        public ICommand ContinueCommand { get; set; }
        private INavigation _navigation;
        private string _employeeID;
       

        public string EmployeeID
        {
            get { return _employeeID; }
            set
            {
                _employeeID = value;
                OnPropertyChanged("EmployeeID");
            }
        }
        public EmployeeRegistrationViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ContinueCommand = new Command(GetEmployeeDetails);
        }
        /// <summary>
        /// Logins the process.
        /// </summary>
        void GetEmployeeDetails()
        {
            try
            {
               
                if (IsValid())
                {
                   

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
