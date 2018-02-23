using System;
using FaceMeApp.CustomRenderer;
using FaceMeApp.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace FaceMeApp.Droid.CustomRenderer
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context):base(context)
        {
            
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
                Control.Gravity = Android.Views.GravityFlags.Start;
                //Control.TextAlignment = Android.Views.TextAlignment.Center;


            }
        }
    }
}
