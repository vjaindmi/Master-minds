using System;
using FaceMeApp.CustomRenderer;
using FaceMeApp.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace FaceMeApp.iOS.CustomRenderer
{
    public class ExtendedEntryRenderer: EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BackgroundColor = UIColor.Clear;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 0;
                //Control.TextAlignment = UITextAlignment.;
            }
        }
    }
}
