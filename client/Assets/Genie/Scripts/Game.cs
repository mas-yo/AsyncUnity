using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // シーンの戻り値
    public struct Result
    {
        // 必要に応じてフィールドを追加
    }

    // シーンの処理起点
    public static async UniTask<Result> StartAsync()
    {
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        // var startButton = GameObject.Find("StartButton").GetComponent<Button>();
        // await startButton.OnClickAsync(token);
        await UniTask.DelayFrame(60); // ダミー待機
        return new Result();
    }
}

