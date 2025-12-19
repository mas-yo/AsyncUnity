using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
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

        {
            var groundPrefab =
                await Resources.LoadAsync<GameObject>("SimpleNaturePack/Prefabs/Ground_01");
            Instantiate(groundPrefab);
        }
        
        var robotPrefab = await Resources.LoadAsync<GameObject>("SciFiWarriorPBRHPPolyart/Prefabs/PBRCharacter");
        var robot = (GameObject)Instantiate(robotPrefab);
        var animator = robot.GetComponentInChildren<Animator>();
        animator.Play("Run_guard_AR");//Idle_gunMiddle_AR

        var cameraObj = GameObject.Find("Main Camera");
        var cameraTransform = cameraObj.transform;

        void UpdateCamera()
        {
            cameraTransform.position = robot.transform.position + new Vector3(0, 8, -8);
            cameraTransform.LookAt(robot.transform.position + new Vector3(0, 2, 0));
        }
        void PlayRunAnimation()
        {
            animator.Play("Run_guard_AR");
        }
        void PlayIdleAnimation()
        {
            animator.Play("Idle_gunMiddle_AR");
        }

        var moveAmount = 0.1f;
        
        await foreach(var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            UpdateCamera();
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                robot.transform.position += new Vector3(
                    (Input.GetKey(KeyCode.D) ? moveAmount : 0) + (Input.GetKey(KeyCode.A) ? -moveAmount : 0),
                    0,
                    (Input.GetKey(KeyCode.W) ? moveAmount : 0) + (Input.GetKey(KeyCode.S) ? -moveAmount : 0)
                );
                PlayRunAnimation();
            }
            else
            {
                PlayIdleAnimation();
            }
        }
        return new Result();
    }

    public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (UniTask waitTask, Func<T> action)[] conditionsAndActions)
    {
        var index = await UniTask.WhenAny(conditionsAndActions.Select(x => x.waitTask).ToArray());
        return conditionsAndActions[index].action();
    }
}
