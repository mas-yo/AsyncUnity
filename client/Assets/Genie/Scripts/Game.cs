using UnityEngine;
using Cysharp.Threading.Tasks;

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
        // Gameオブジェクトを取得
        var gameObj = GameObject.Find("Game");
        // 必要なUnityの機能をここで利用可能
        // ...
        return new Result();
    }
}

