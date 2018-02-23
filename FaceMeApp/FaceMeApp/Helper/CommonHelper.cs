using System;
using Acr.UserDialogs;

namespace FaceMeApp.Helper
{
    public static class CommonHelper
    {
        public static void ShowLoader()
        {
            UserDialogs.Instance.ShowLoading("Please wait...");
        }

        public static void DismissLoader()
        {
            UserDialogs.Instance.HideLoading();
        }

        public static void ShowAlert(string msg)
        {
            UserDialogs.Instance.ShowError(msg);
        }
    }
}
