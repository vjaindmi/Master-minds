using System;
using FaceMeApp.Droid.DependencyServices;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Net.Wifi;
using FaceMeApp.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(PersistStoreService))]
namespace FaceMeApp.Droid.DependencyServices
{
    public class PersistStoreService : IPersistStoreService
    {
        string User_KEY = "userImage";
        public PersistStoreService()
        {
        }

        public async Task<bool> ConnectWpa(string ssid, string password)
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
                    return false;

                wifiManager.Disconnect();
                wifiManager.EnableNetwork(netId, true);
                wifiManager.Reconnect();

                if (wifiManager.ConnectionInfo.NetworkId == -1)//Known network with invalid credentials
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string GetImageFromLocalDirectory()
        {
            throw new NotImplementedException();
        }

        public Stream GetJpgStreamFromPath(string path)
        {
            throw new NotImplementedException();
        }

        public string getUserData()
        {
            var storage = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);

            return storage.GetString("image", "");

        }

        public void SaveImageToLocalDirectory(byte[] byteArray)
        {

        }

        public void saveUserData(string imageFile)
        {
            var prefs = Android.App.Application.Context.GetSharedPreferences(User_KEY, FileCreationMode.Private);
            var storage = prefs.Edit();

            storage.PutString("image", imageFile);
            storage.Commit();
        }
    }
}
