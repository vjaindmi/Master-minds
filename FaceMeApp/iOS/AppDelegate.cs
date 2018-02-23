using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Acr.UserDialogs;
using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;

namespace FaceMeApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Validate SSL always
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;
            ImageCircleRenderer.Init();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
