using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Genie;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Game
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
            Object.Instantiate(groundPrefab);
        }
        
        var robotPrefab = await Resources.LoadAsync<GameObject>("SciFiWarriorPBRHPPolyart/Prefabs/PBRCharacter");
        var robot = (GameObject)Object.Instantiate(robotPrefab);
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
            var xMove = 0f;
            var zMove = 0f;
            if (Input.GetKey(KeyCode.W)) zMove += moveAmount;
            if (Input.GetKey(KeyCode.S)) zMove -= moveAmount;
            if (Input.GetKey(KeyCode.D)) xMove += moveAmount;
            if (Input.GetKey(KeyCode.A)) xMove -= moveAmount;
            robot.transform.Translate(xMove, 0, zMove);
            
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                PlayRunAnimation();
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                var pauseResult = await PauseWindow.StartAsync(token);
                if (pauseResult.IsExit)
                {
                    break;
                }
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
