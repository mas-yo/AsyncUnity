using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        animator.Play("Run_guard_AR");//Idle_gunMiddle_AR


        var moveAmount = 0.1f;
        while (true)
        {
            var isEsc = await WaitAndDo(
                token,
                (() => Input.GetKey(KeyCode.W), () =>
                {
                    robot.transform.position += Vector3.forward * moveAmount;
                    animator.Play("Run_guard_AR");
                    return false;
                }),
                (() => Input.GetKey(KeyCode.S), () =>
                {
                    robot.transform.position += Vector3.back * moveAmount;
                    animator.Play("Run_guard_AR");
                    return false;
                }),
                (() => Input.GetKey(KeyCode.A), () =>
                {
                    robot.transform.position += Vector3.left * moveAmount;
                    animator.Play("Run_guard_AR");
                    return false;
                }),
                (() => Input.GetKey(KeyCode.D), () =>
                {
                    robot.transform.position += Vector3.right * moveAmount;
                    animator.Play("Run_guard_AR");
                    return false;
                }),
                (() => Input.anyKey == false, () =>
                {
                    animator.Play("Idle_gunMiddle_AR");
                    return false;
                }),
                (() => Input.GetKey(KeyCode.Escape), () => { return true; })
            );
            if (isEsc)
            {
                break;
            }
        }
        
        return new Result();

    }

    public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (Func<bool> condition, Func<T> action)[] conditionsAndActions)
    {
        var index = await UniTask.WhenAny(conditionsAndActions.Select(x => UniTask.WaitUntil(() => x.condition(), cancellationToken: token)).ToArray());
        return conditionsAndActions[index].action();
    }
}
