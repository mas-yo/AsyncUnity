using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync()
    {
        await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        var prefab = await Resources.LoadAsync<GameObject>("StarShip/StarShip");
        Object.Instantiate(prefab);
        await UniTask.DelayFrame(600);
        return new Result();
    }
}
