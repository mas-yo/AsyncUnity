using UnityEngine;
using Cysharp.Threading.Tasks;

public class Title : MonoBehaviour
{
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync()
    {
        // Title GameObjectを取得
        var titleObj = GameObject.Find("Title");
        // 必要な初期化やUI表示などをここで行う
        // ...
        // 結果を返す
        return new Result();
    }
}

