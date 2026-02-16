using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Genie;
using Genie.MasterData;
using Genie.Views;
using Genie.Windows;
using Lua;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Genie.Scenes
{
    public class GameScene
    {
        public struct Result
        {
        }

        public static async UniTask<Result> StartAsync(
            MemoryDatabase masterData,
            LuaState luaState,
            string groundPrefabPath,
            CancellationToken token
            )
        {
            await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

            var loadingWindow = await LoadingView.CreateAsync(token);

            {
                var groundPrefab =
                    await Resources.LoadAsync<GameObject>(groundPrefabPath);
                Object.Instantiate(groundPrefab);
            }

            var camera = await GameCamera.CreateAsync();

            var results = await luaState.DoStringAsync("return OnStart()");
            var player = results[0].Read<PlayerView>();

            var gameHud = new GameHud(Object.FindAnyObjectByType<GameHudComponents>());

            await UniTask.Delay(1000);
            
            loadingWindow.Close();

            var score = 0;
            
            await foreach(var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
            //     foreach (var mushroom in mushrooms)
            //     {
            //         foreach (var collision in mushroom.DequeueCollisions())
            //         {
            //             if (player.IsSame(collision.gameObject))
            //             {
            //                 mushroom.PlayDisappearAnimation();
            //                 score += 10;
            //                 gameHud.SetScore(score);
            //             }
            //         }
            //     }
            //     
                camera.SetTarget(player.Position);
            //     
                var xMove = 0f;
                var zMove = 0f;
                if (Input.GetKey(KeyCode.W)) zMove += 0.1f;
                if (Input.GetKey(KeyCode.S)) zMove -= 0.1f;
                if (Input.GetKey(KeyCode.D)) xMove += 0.1f;
                if (Input.GetKey(KeyCode.A)) xMove -= 0.1f;
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
                    var pauseResult = await PauseWindow.ShowAsync(token);
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
    
}
