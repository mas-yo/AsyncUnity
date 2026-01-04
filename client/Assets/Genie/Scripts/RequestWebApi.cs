using Cysharp.Threading.Tasks;
using Genie.Protocols;
using UnityEngine.Networking;

namespace Genie
{
    public class RequestWebApi
    {
        // public async static UniTask<TResponse> DoAsync<TRequest, TResponse>(string baseUri, string endPoint, TRequest request, (string key, string value)[] headers, int timeout)
        // {
        //     
            // var requestBytes = Serializer.Serialize(request);
            // var webRequest = new UnityWebRequest(baseUri + endPoint, UnityWebRequest.kHttpVerbPOST);
            // webRequest.downloadHandler = new DownloadHandlerBuffer();
            //
            // webRequest.uploadHandler = new UploadHandlerRaw(requestBytes);
            // foreach (var (key, value) in headers)
            // {
            //     webRequest.SetRequestHeader(key, value);
            // }
            //
            // webRequest.timeout = timeout;
            //
            // var response = await webRequest.SendWebRequest().ToUniTask();
        //         
        // }
    }
}