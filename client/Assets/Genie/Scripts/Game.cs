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
        var prefabRequest = Resources.LoadAsync<GameObject>("SciFiWarriorPBRHPPolyart/Prefabs/PBRCharacter");
        await prefabRequest;
        var prefab = (GameObject)prefabRequest.asset;
        var robot = Object.Instantiate(prefab);
        var animator = robot.GetComponentInChildren<Animator>();
        animator.Play("Run_guard_AR");
        

        var moveAmount = 0.1f;
        while (true)
        {
            var wTask = UniTask.WaitUntil(() => Input.GetKey(KeyCode.W), cancellationToken: token);
            var sTask = UniTask.WaitUntil(() => Input.GetKey(KeyCode.S), cancellationToken: token);
            var aTask = UniTask.WaitUntil(() => Input.GetKey(KeyCode.A), cancellationToken: token);
            var dTask = UniTask.WaitUntil(() => Input.GetKey(KeyCode.D), cancellationToken: token);
            var escTask = UniTask.WaitUntil(() => Input.GetKey(KeyCode.Escape), cancellationToken: token);

            var winIndex = await UniTask.WhenAny(wTask, sTask, aTask, dTask, escTask);

            if (winIndex == 4)
            {
                break;
            }

            switch (winIndex)
            {
                case 0:
                    robot.transform.position += Vector3.forward * moveAmount;
                    break;
                case 1:
                    robot.transform.position += Vector3.back * moveAmount;
                    break;
                case 2:
                    robot.transform.position += Vector3.left * moveAmount;
                    break;
                case 3:
                    robot.transform.position += Vector3.right * moveAmount;
                    break;
            }
        }
        return new Result();
    }
}
