using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Genie;
using Genie.Logics;
using Genie.Protocols;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Genie.Scenes
{
    public class TitleScene
    {
        public struct Result
        {
            public UserInfo UserInfo;
        }

        public static async UniTask<Result> StartAsync(string apiBaseUrl, string apiVersion, CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Single);
            var startButton = GameObject.Find("StartButton").GetComponent<Button>();
            await startButton.OnClickAsync(token);
            // var userInfo = await apiGate.LoginAsync();
        
            var header = WebApi.CreateHeader("", apiVersion);
            var requestBytes = Serializer.Serialize(new Login.Request()
            {
                dummy = "dummy"
            });
            var downloadHandler = await WebApi.RequestAsync(apiBaseUrl + Login.endPoint, requestBytes, header, 10);
            
            var response = Serializer.Deserialize<Login.Response>(downloadHandler.data);
            
            return new Result() { UserInfo = response.UserInfo };
        }
    }
}
