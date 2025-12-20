using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title
{
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync(CancellationToken token)
    {
        await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Single);
        var startButton = GameObject.Find("StartButton").GetComponent<Button>();
        await startButton.OnClickAsync(token);
        return new Result();
    }
}

