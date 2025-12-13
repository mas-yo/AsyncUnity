using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync(CancellationToken token)
    {
        await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        var prefabRequest = Resources.LoadAsync<GameObject>("StarShip/StarShip");
        await prefabRequest;
        var prefab = (GameObject)prefabRequest.asset;
        var starShip = Object.Instantiate(prefab);

        var moveAmount = 1.0f;
        while (true)
        {
            var wTask = UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.W), cancellationToken: token);
            var sTask = UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.S), cancellationToken: token);
            var aTask = UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.A), cancellationToken: token);
            var dTask = UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.D), cancellationToken: token);
            var escTask = UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Escape), cancellationToken: token);

            var winIndex = await UniTask.WhenAny(wTask, sTask, aTask, dTask, escTask);

            if (winIndex == 4)
            {
                break;
            }

            switch (winIndex)
            {
                case 0:
                    starShip.transform.position += Vector3.forward * moveAmount;
                    break;
                case 1:
                    starShip.transform.position += Vector3.back * moveAmount;
                    break;
                case 2:
                    starShip.transform.position += Vector3.left * moveAmount;
                    break;
                case 3:
                    starShip.transform.position += Vector3.right * moveAmount;
                    break;
            }
        }
        return new Result();
    }
}
