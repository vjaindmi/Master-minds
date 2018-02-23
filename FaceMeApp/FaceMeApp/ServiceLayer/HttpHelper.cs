using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ModernHttpClient;

namespace FaceMeApp.ServiceLayer
{
    public class HttpHelper
    {
        public static string APIBaseAddress = "http://192.168.1.191/faceAuth/";
        public const string RequestFormat = "application/json";
        public const string EncodingFormat = "utf-8";

        public static HttpClient GetHttpClient()
        {
            var handler = new NativeMessageHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            var client = new HttpClient(handler);
           // client.BaseAddress = new Uri(APIBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(RequestFormat));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(EncodingFormat));

            return client;
        }
    }
}
