using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Genie.Scenes;

namespace Genie
{
    public class Main
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartMainLoop()
        {
            MainLoopAsync().Forget();
        }
    
        private static async UniTask MainLoopAsync()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            
            var masterData = await LoadMasterData.StartAsync(token);
            
            var characterMaster = masterData.Characters[0];
            // var stageMaster = masterData.Stages[0];
            var apiGate = new ApiGate();
            
            while (true)
            {
                var titleResult = await TitleScene.StartAsync(apiGate, token);
                var stageMaster = masterData.Stages.First(x => x.Code == titleResult.UserInfo.CurrentStageCode);
                
                await GameScene.StartAsync(
                    apiGate,
                    stageMaster.Code,
                    groundPrefabPath: stageMaster.GroundPrefabPath,
                    playerPrefabPath: characterMaster.ModelPrefabPath,
                    playerInitialPosition: characterMaster.InitialPosition,
                    playerMoveSpeed: characterMaster.MoveSpeed,
                    mushRoomParams: masterData.Items.Select(x => (x.prefabPath, x.position)).ToArray(),
                    token: token
                    );
            }
        
        }


    }
    
}
