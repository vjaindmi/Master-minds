using System;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using FaceMeApp.DependencyServices;
using FaceMeApp.iOS.DependencyServices;
using Foundation;
using NetworkExtension;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PersistStoreService))]
namespace FaceMeApp.iOS.DependencyServices
{
    public class PersistStoreService : IPersistStoreService
    {
        string User_KEY = "userImage";
        public PersistStoreService()
        {
        }

        public string getUserData()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(User_KEY) ?? "";
        }

        public void saveUserData(string imageFile)
        {
            NSUserDefaults.StandardUserDefaults.SetString(imageFile, User_KEY);
        }

        public void SaveImageToLocalDirectory(byte[] byteArray)
        {
            var data = NSData.FromArray(byteArray);
            var uiimage = UIImage.LoadFromData(data);

            var documentsDirectory = Environment.GetFolderPath
                         (Environment.SpecialFolder.Personal);
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, "Photo.jpg"); // hardcoded filename, overwritten each time
            NSData imgData = uiimage.AsJPEG();
            NSError err = null;
            if (imgData.Save(jpgFilename, false, out err))
            {
                Console.WriteLine("saved as " + jpgFilename);
            }
            else
            {
                Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
            }

        }

        public string GetImageFromLocalDirectory()
        {

            var documentsDirectory = Environment.GetFolderPath
                         (Environment.SpecialFolder.Personal);
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, "Photo.jpg"); // hardcoded filename, overwritten each time
            return jpgFilename;
        }

        public Stream GetJpgStreamFromPath(string path)
        {
            if (path == null || path.Equals("") || !File.Exists(path))
            {
                return null;
            }
            else
            {
                byte[] imageAsBytes = File.ReadAllBytes(path);
                UIKit.UIImage images = new UIKit.UIImage(Foundation.NSData.FromArray(imageAsBytes));
                images = MaxResizeImage(images, 500, 500);
                byte[] bytes = images.AsJPEG().ToArray();
                Stream imgStream = new MemoryStream(bytes);
                return imgStream;
            }
        }

        public UIImage ResizeUIImage(UIImage sourceImage, float widthToScale, float heightToScale)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(widthToScale / sourceSize.Width, heightToScale / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize(width, height));
            sourceImage.Draw(new CGRect(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize(width, height));
            sourceImage.Draw(new CGRect(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public async Task<bool> ConnectWpa(string ssid, string password)
        {
            try
            {

                var config = new NEHotspotConfiguration(ssid, password, false) { JoinOnce = true };
                var configManager = new NEHotspotConfigurationManager();
                await configManager.ApplyConfigurationAsync(config);

                Console.WriteLine("Connected!!!!!");

                return true;
            }
            catch (Foundation.NSErrorException error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        bool IPersistStoreService.ConnectWpa(string ssid, string password)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes()
        {
            var documentsDirectory = Environment.GetFolderPath
                         (Environment.SpecialFolder.Personal);
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, "icon.png");
            byte[] img = File.ReadAllBytes(jpgFilename);
            return img;
        }
       
    }
}
