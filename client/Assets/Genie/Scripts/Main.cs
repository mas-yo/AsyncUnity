using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Logics;
using Genie.MasterData;
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
            
            var rows = ExcelReader.EnumerateExcelReaders(Application.persistentDataPath)
                .SelectMany(ExcelReader.EnumerateRows);
            var masterData = MasterData.MasterData.FromDictionary(DataTableProcessor.ConvertRowsToDictionary(rows));

            var test = Logics.MasterMemoryBuilder.Build(DataTableProcessor.ConvertRowsToDictionary(rows));
            var a = test.CharacterMasterTable.FindByCode(0);
            
            var characterMaster = masterData.Characters[0];
            
            while (true)
            {
                var titleResult = await TitleScene.StartAsync("https://genie.co.jp/", "1.0.0", token);
                var stageMaster = masterData.Stages.First(x => x.Code == titleResult.UserInfo.CurrentStageCode);
                
                await GameScene.StartAsync(
                    stageMaster.Code,
                    groundPrefabPath: stageMaster.GroundPrefabPath,
                    playerPrefabPath: characterMaster.ModelPrefabPath,
                    playerInitialPosition: characterMaster.InitialPosition,
                    playerMoveSpeed: characterMaster.MoveSpeed,
                    mushRoomParams: masterData.Items.Select(x => (prefabPath: x.PrefabPath, position: x.Position)).ToArray(),
                    token: token
                    );
            }
        
        }


    }
    
}
