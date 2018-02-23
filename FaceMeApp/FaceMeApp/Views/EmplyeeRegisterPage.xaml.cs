﻿using System;
using System.Collections.Generic;
using FaceMeApp.ViewModel;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class EmplyeeRegisterPage : ContentPage
    {
        EmployeeRegistrationViewModel _viewModel = null;
        public EmplyeeRegisterPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new EmployeeRegistrationViewModel(this.Navigation);
        }
    }
}