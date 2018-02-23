using System;

namespace FaceMeApp.Helper
{
    public static class CommonHelper
    {
        public static void ShowLoader()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
        }

        public static void DismissLoader()
        {
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }

        public static void ShowAlert(string msg)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowError(msg);
        }
    }
}
