using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Genie;
using Genie.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Game
{
    public struct Result
    {
    }

    public static async UniTask<Result> StartAsync(
        string groundPrefabPath,
        string playerPrefabPath,
        Vector3 playerInitialPosition,
        float playerMoveSpeed,
        (string prefabPath, Vector3 position)[] mushRoomParams,
        CancellationToken token
        )
    {
        await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        {
            var groundPrefab =
                await Resources.LoadAsync<GameObject>(groundPrefabPath);
            Object.Instantiate(groundPrefab);
        }

        var camera = await GameCamera.CreateAsync();
        
        var player = await Player.CreateAsync(playerPrefabPath, playerInitialPosition);
        // var mushroom = await Mushroom.CreateAsync("SimpleNaturePack/Prefabs/Mushroom_02", new Vector3(1f, 0.8f, 1f));
        
        var mushrooms = await UniTask.WhenAll(
            mushRoomParams.Select(async param =>
                await Mushroom.CreateAsync(param.prefabPath, param.position)
            )
        );
        
        var moveAmount = 0.1f;
        
        await foreach(var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            foreach (var mushroom in mushrooms)
            {
                foreach (var collision in mushroom.DequeueCollisions())
                {
                    if (collision.gameObject == player.gameObject)
                    {
                        mushroom.PlayDisappearAnimation();
                    }
                }
            }
            
            camera.SetTarget(player.transform);
            
            var xMove = 0f;
            var zMove = 0f;
            if (Input.GetKey(KeyCode.W)) zMove += moveAmount;
            if (Input.GetKey(KeyCode.S)) zMove -= moveAmount;
            if (Input.GetKey(KeyCode.D)) xMove += moveAmount;
            if (Input.GetKey(KeyCode.A)) xMove -= moveAmount;
            player.Move(new Vector3(xMove, 0, zMove));
            
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                player.PlayRunAnimation();
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
                player.PlayIdleAnimation();
            }
            
        }
        return new Result();
    }
}
