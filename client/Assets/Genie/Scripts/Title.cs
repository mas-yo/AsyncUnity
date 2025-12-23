using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Genie;
using Genie.Protocols;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title
{
    public struct Result
    {
        public UserInfo UserInfo;
    }

    public static async UniTask<Result> StartAsync(ApiGate apiGate, CancellationToken token)
    {
        await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Single);
        var startButton = GameObject.Find("StartButton").GetComponent<Button>();
        await startButton.OnClickAsync(token);
        var userInfo = await apiGate.LoginAsync();
        
        return new Result() { UserInfo = userInfo };
    }
}

