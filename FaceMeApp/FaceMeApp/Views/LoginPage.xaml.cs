using System;
using System.Collections.Generic;
using FaceMeApp.ViewModel;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _viewModel = null;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _viewModel = new LoginViewModel(this.Navigation);
        }
    }
}
