using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Genie.Logics;
using Genie.Utils;
using Genie.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Genie.Scripts.Scenes
{
    public static class DebugStartScene
    {
        public struct Result
        {
            public bool IsStandalone;
            public string ApiBaseUrl;
        }

        public static async UniTask<Result> StartAsync(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("DebugStartScene", LoadSceneMode.Single);
            
            var debugStartView = new DebugStartView(Object.FindAnyObjectByType<DebugStartViewComponents>());

            while (true)
            {
                var result = await UniTaskUtil.WaitAndCancel(token,
                    debugStartView.OnClickStartButtonAsync,
                    debugStartView.OnClickClearLocalSaveButtonAsync
                );

                switch (result.winArgumentIndex)
                {
                    case 0:
                        return new Result()
                        {
                            IsStandalone = false,
                            ApiBaseUrl = "http://localhost:5000/api/"
                        };
                        
                    case 1:
                        LocalStorage.SaveShownQuestCodes(Array.Empty<long>());
                        break;
                }
            }

            
        }
    }
}