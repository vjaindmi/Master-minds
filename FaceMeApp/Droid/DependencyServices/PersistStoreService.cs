using System;
using FaceMeApp.Droid.DependencyServices;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Net.Wifi;
using FaceMeApp.DependencyServices;
using Android.Graphics;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PersistStoreService))]
namespace FaceMeApp.Droid.DependencyServices
{
    public class PersistStoreService : IPersistStoreService
    {
        string User_KEY = "userImage";
        public PersistStoreService()
        {
        }

        public string ConnectWpa(string ssid, string password)
        {
            try
            {
                string networkSSID = ssid;
                string networkPass = password;
                WifiConfiguration wifiConfig = new WifiConfiguration();
                wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
                wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

                WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);

                if (wifiManager.WifiState == Android.Net.WifiState.Disabled)
                    wifiManager.SetWifiEnabled(true);

                // Use ID
                int netId = wifiManager.AddNetwork(wifiConfig);
                if (netId == -1)
                    return "";

                wifiManager.Disconnect();
                wifiManager.EnableNetwork(netId, true);
                var reconnected = wifiManager.Reconnect();

                if (reconnected)
                {
                    if (wifiManager.ConnectionInfo.NetworkId == -1)//Known network with invalid credentials
                        return "";

                    return wifiManager.ConnectionInfo.MacAddress;  
                }
                return "D0:22:BE:40:6A:70";
            }
            catch (Exception ex)
            { 
                return "";
            }

        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public string GetImageFromLocalDirectory()
        {
            var storage = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);

            return storage.GetString("path", "");
        }

        public Stream GetJpgStreamFromPath(string path)
        {
            Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(path));

            System.IO.Stream input = Forms.Context.ContentResolver.OpenInputStream(uri);
            return input;

        }

        public string getUserData()
        {
            var storage = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);

            return storage.GetString("image", "");

        }

        public void SaveImageToLocalDirectory(byte[] byteArray)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;
            //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
            string name = "test.jpg";
            string filePath = System.IO.Path.Combine(pictures, name);
            try
            {
                System.IO.File.WriteAllBytes(filePath, byteArray);
                //mediascan adds the saved image into the gallery
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                saveImagePath(filePath);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }
        public void saveImagePath(string imagePath)
        {
            var prefs = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);
            var storage = prefs.Edit();

            storage.PutString("path", imagePath);
            storage.Commit();
        }
        public string getImagePath()
        {
            var storage = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);

            return storage.GetString("path", "");


        }
        public void saveUserData(string imageFile)
        {
            var prefs = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);
            var storage = prefs.Edit();

            storage.PutString("image", imageFile);
            storage.Commit();
        }
        public void saveEmployeeId(string id)
        {
            var prefs = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);
            var storage = prefs.Edit();

            storage.PutString("id", id);
            storage.Commit();
        }
        public string getEmployeeId()
        {
            var storage = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);

            return storage.GetString("id", "");
        }
        public  byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            //
            float ZielHoehe = 0;
            float ZielBreite = 0;
            //
            var Hoehe = originalImage.Height;
            var Breite = originalImage.Width;
            //
            if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
            {
                ZielHoehe = height;
                float teiler = Hoehe / height;
                ZielBreite = Breite / teiler;
            }
            else // Breite (61 für Avatar) ist Master
            {
                ZielBreite = width;
                float teiler = Breite / width;
                ZielHoehe = Hoehe / teiler;
            }
            //
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)ZielBreite, (int)ZielHoehe, false);
            // 
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}
