using System;
using System.IO;
using System.Threading.Tasks;

namespace FaceMeApp.DependencyServices
{
    public interface IPersistStoreService
    {
        /// <summary>
        /// Saves the user data.
        /// </summary>
        /// <param name="imageFile">Image file.</param>
        void saveUserData(string imageFile);
        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <returns>The user data.</returns>
        string getUserData();
        /// <summary>
        /// Saves the image to local directory.
        /// </summary>
        /// <param name="byteArray">Byte array.</param>
        void SaveImageToLocalDirectory(byte[] byteArray);
        /// <summary>
        /// Gets the image from local directory.
        /// </summary>
        /// <returns>The image from local directory.</returns>
        string GetImageFromLocalDirectory();
        /// <summary>
        /// Gets the jpg stream from path.
        /// </summary>
        /// <returns>The jpg stream from path.</returns>
        /// <param name="path">Path.</param>
        Stream GetJpgStreamFromPath(string path);
        /// <summary>
        /// Connects the wpa.
        /// </summary>
        /// <returns>The wpa.</returns>
        /// <param name="ssid">Ssid.</param>
        /// <param name="password">Password.</param>
        Task<bool> ConnectWpa(string ssid, string password);
    }
}
