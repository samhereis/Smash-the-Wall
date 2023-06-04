using DataClasses;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Helpers
{
    public static class WebRequestHelper
    {
        public static async Task<UnityWebRequest> Post(string uri, string json)
        {
            UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(uri, "POST");

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)) as UploadHandler;

            var sentRequest = webRequest.SendWebRequest();

            while (sentRequest.isDone == false) await AsyncHelper.Delay();

            return sentRequest.webRequest;
        }

        public static async Task<UnityWebRequest> Get(string uri, Token token = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);

            if (token != null) webRequest.SetRequestHeader("Authorization", token.token);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            webRequest.url = uri;

            webRequest.uri = new System.Uri(uri);
            webRequest.url = uri;

            var sentRequest = webRequest.SendWebRequest();

            while (sentRequest.isDone == false) await AsyncHelper.Delay();

            return sentRequest.webRequest;
        }

        public static async Task<UnityWebRequest> Update(string uri, string json, Token token = null, string method = "PUT")
        {
            UnityWebRequest webRequest = UnityWebRequest.Put(uri, method);

            webRequest.method = method;

            if (token != null) webRequest.SetRequestHeader("Authorization", token.token);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)) as UploadHandler;

            var sentRequest = webRequest.SendWebRequest();

            while (sentRequest.isDone == false) await AsyncHelper.Delay();

            return sentRequest.webRequest;
        }

        public static async Task<UnityWebRequest> Delete(string uri, Token token = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Delete(uri);

            if (token != null) webRequest.SetRequestHeader("Authorization", token.token);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            var sentRequest = webRequest.SendWebRequest();

            while (sentRequest.isDone == false) await AsyncHelper.Delay();

            return sentRequest.webRequest;
        }

        public class ForceAcceptAll : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
    }
}
