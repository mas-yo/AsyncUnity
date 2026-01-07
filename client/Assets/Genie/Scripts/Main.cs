using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Logics;
using Genie.MasterData;
using UnityEngine;
using Genie.Scenes;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Genie
{
    public class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SetupMessagePackResolver()
        {
            StaticCompositeResolver.Instance.Register(new[]{
                MasterMemoryResolver.Instance,
                StandardResolver.Instance
            });
            
            var options = MessagePackSerializerOptions.Standard.WithResolver(CompositeResolver.Create(
                new IMessagePackFormatter[]
                {
                    new Utils.Vector3Formatter(),
                },
                new IFormatterResolver[]
                {
                    StandardResolver.Instance
                }));
            
            MessagePackSerializer.DefaultOptions = options;
        }
        
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
            // var masterData = MasterData.MasterData.FromDictionary(DataTableProcessor.ConvertRowsToDictionary(rows));

            var masterData = Logics.MasterMemoryBuilder.Build(DataTableProcessor.ConvertRowsToDictionary(rows));
            
            var characterMaster = masterData.CharacterMasterTable.FindByCode(1);
            
            while (true)
            {
                var titleResult = await TitleScene.StartAsync("https://genie.co.jp/", "1.0.0", token);
                var stageMaster = masterData.StageMasterTable.FindByCode(titleResult.UserInfo.CurrentStageCode);
                
                await GameScene.StartAsync(
                    stageMaster.Code,
                    groundPrefabPath: stageMaster.GroundPrefabPath,
                    playerPrefabPath: characterMaster.ModelPrefabPath,
                    playerInitialPosition: characterMaster.InitialPosition,
                    playerMoveSpeed: characterMaster.MoveSpeed,
                    mushRoomParams: masterData.ItemMasterTable.All.Select(x => (prefabPath: x.PrefabPath, position: x.Position)).ToArray(),
                    token: token
                    );
            }
        
        }


    }
    
}
