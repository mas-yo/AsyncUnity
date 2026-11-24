using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace AsyncUnity
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

            while (true)
            {

                var titleResult = await TitleScene.StartAsync("", "1.0.0", token);
                //
                var masterData = LoadMasterData(token);

                var mapCode = titleResult.stageCode;

                while(true)
                {
                    var gameResult = await Game1Scene.StartAsync(
                        masterData: masterData,
                        mapCode,
                        token: token
                    );

                    switch (gameResult.ResultNext)
                    {
                        case Game1Scene.ResultNext.NoStage:
                            mapCode = 1;
                            break;
                        case Game1Scene.ResultNext.NextStage:
                            mapCode++;
                            break;
                        case Game1Scene.ResultNext.Retry:
                            break;
                        default:
                            throw new NotImplementedException($"Unsupported ResultNext: {gameResult.ResultNext}");
                    }
                }
                
            }
        
        }
        
        private static MemoryDatabase LoadMasterData(CancellationToken token)
        {
            var masterData = Logics.MasterMemoryBuilder.Build(DataTableProcessor.ConvertRowsToDictionary(rows));
            return masterData;
        }


    }
    
}
