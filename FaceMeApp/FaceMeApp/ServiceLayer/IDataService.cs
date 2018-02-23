using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace FaceMeApp.ServiceLayer
{
    public interface IDataService
    {
        Task<bool> VerifyFace(Stream registeredFace, Stream faceToVerify);
        Task<bool> UploadAndDetectFaces(Stream imageStream);

    }
}
