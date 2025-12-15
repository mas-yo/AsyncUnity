using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
            await WaitAndDo(
                token,
                (() => Input.GetKey(KeyCode.W), () => robot.transform.position += Vector3.forward * moveAmount),
                (() => Input.GetKey(KeyCode.S), () => robot.transform.position += Vector3.back * moveAmount),
                (() => Input.GetKey(KeyCode.A), () => robot.transform.position += Vector3.left * moveAmount),
                (() => Input.GetKey(KeyCode.D), () => robot.transform.position += Vector3.right * moveAmount),
                (() => Input.GetKey(KeyCode.Escape), () => throw new OperationCanceledException())
            );
        }
        return new Result();
    }

    public static async UniTask WaitAndDo(CancellationToken token, params (Func<bool> predicate, Action action)[] conditionsAndActions)
    {
        var index = await UniTask.WhenAny(conditionsAndActions.Select(x => UniTask.WaitUntil(() => x.predicate(), cancellationToken: token)).ToArray());
        conditionsAndActions[index].action();
    }
}
