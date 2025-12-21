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
    public record PlayerParameters
    {
        public string modelPrefabPath;
        public Vector3 InitialPosition;
        public float MoveSpeed;
    }
    public record MapParameters
    {
        public string GroundPrefabPath;
    }
    
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync(MapParameters mapParameters, PlayerParameters playerParameters, CancellationToken token)
    {
        await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        {
            var groundPrefab =
                await Resources.LoadAsync<GameObject>(mapParameters.GroundPrefabPath);
            Object.Instantiate(groundPrefab);
        }
        
        var playerPrefab = await Resources.LoadAsync<GameObject>(playerParameters.modelPrefabPath);
        var player = (GameObject)Object.Instantiate(playerPrefab);
        player.transform.position = playerParameters.InitialPosition;
        var robotRegitBody = player.GetComponent<Rigidbody>();
        
        var animator = player.GetComponentInChildren<Animator>();
        animator.Play("Run_guard_AR");//Idle_gunMiddle_AR

        var cameraObj = GameObject.Find("Main Camera");
        var cameraTransform = cameraObj.transform;

        void UpdateCamera()
        {
            cameraTransform.position = player.transform.position + new Vector3(0, 8, -8);
            cameraTransform.LookAt(player.transform.position + new Vector3(0, 2, 0));
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
            // robot.transform.Translate(xMove, 0, zMove);
            // robotRegitBody.linearVelocity = new Vector3(xMove, robotRegitBody.linearVelocity.y, zMove);
            var nextPosition = player.transform.position + new Vector3(xMove, 0, zMove);
            robotRegitBody.MovePosition(nextPosition);
            // robotRegitBody.AddForce(new Vector3(xMove, 0, zMove), ForceMode.Force);
            
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
}
