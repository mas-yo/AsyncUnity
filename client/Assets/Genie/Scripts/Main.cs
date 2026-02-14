using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Logics;
using Genie.MasterData;
using UnityEngine;
using Genie.Scenes;
using Genie.Scripts.Scenes;
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

            while (true)
            {
                var apiBaseUrl = "https://genie-api.example.com/";
                #if DEBUG_START
                var debugStartResult = await DebugStartScene.StartAsync(token);
                apiBaseUrl = debugStartResult.ApiBaseUrl;
                #endif
                
                var titleResult = await TitleScene.StartAsync(apiBaseUrl, "1.0.0", token);
                
                var masterData = LoadMasterData(token);
                var stageMaster = masterData.StageMasterTable.FindByCode(titleResult.UserInfo.CurrentStageCode);

                var homeResult = await HomeScene.StartAsync(HomeScene.HomeViewType.QuestList, token);
                
                var gameResult = await GameScene.StartAsync(
                    masterData: masterData,
                    luaState: await LuaStateBuilder.BuildAsync(masterData),
                    groundPrefabPath: stageMaster.GroundPrefabPath,
                    token: token
                    );
            }
        
        }
        
        private static MemoryDatabase LoadMasterData(CancellationToken token)
        {
            var masterDataPath = Application.persistentDataPath + "/MasterData";
            if (Directory.Exists(masterDataPath) == false)
            {
                masterDataPath = Application.streamingAssetsPath + "/MasterData";
            }

            var rows = ExcelReader.EnumerateExcelReaders(masterDataPath, Application.temporaryCachePath)
                .SelectMany(ExcelReader.EnumerateRows);

            var masterData = Logics.MasterMemoryBuilder.Build(DataTableProcessor.ConvertRowsToDictionary(rows));
            return masterData;
        }


    }
    
}
