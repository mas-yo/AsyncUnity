using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Genie;
using Genie.Logics;
using Genie.Protocols;
using MessagePack;
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
        
            // var header = WebApi.CreateHeader("", apiVersion);
            // var requestBytes = MessagePackSerializer.Serialize(new Login.Request()
            // {
            //     dummy = "dummy"
            // });
            // var downloadHandler = await WebApi.RequestAsync(apiBaseUrl + Login.endPoint, requestBytes, header, 10);
            //
            // var response = MessagePackSerializer.Deserialize<Login.Response>(downloadHandler.data);
            
            return new Result() { UserInfo = new UserInfo() { UserId = "TestUser", UserName = "Test User", CurrentStageCode = 1} };
        }
    }
}
