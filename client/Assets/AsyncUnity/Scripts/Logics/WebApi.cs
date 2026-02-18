﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace AsyncUnity.Logics
{
    public class WebApi
    {
        public static async UniTask<DownloadHandler> RequestAsync(string url, byte[] requestBytes, (string key, string value)[] headers, int timeout)
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

        public static (string key, string value)[] CreateHeader(string authToken, string apiVersion)
        {
            var headers = new List<(string key, string value)>()
            {
                ("Content-Type", "application/x-msgpack"),
                ("Accept", "application/x-msgpack"),
                ("X-Polka-ApiVersion", apiVersion),
            };
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                headers.Add(("Authorization", $"Bearer {authToken}"));
            }

            return headers.ToArray();
        }

    }
}