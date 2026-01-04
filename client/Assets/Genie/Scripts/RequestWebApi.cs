using Cysharp.Threading.Tasks;
using Genie.Protocols;
using UnityEngine.Networking;

namespace Genie
{
    public class RequestWebApi
    {
        public static async UniTask<DownloadHandler> DoAsync(string url, byte[] requestBytes, (string key, string value)[] headers, int timeout)
        {
            var webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            
            webRequest.uploadHandler = new UploadHandlerRaw(requestBytes);
            foreach (var (key, value) in headers)
            {
                webRequest.SetRequestHeader(key, value);
            }
            
            webRequest.timeout = timeout;
            var response = await webRequest.SendWebRequest().ToUniTask();
            return response.downloadHandler;
            
        }
    }
}