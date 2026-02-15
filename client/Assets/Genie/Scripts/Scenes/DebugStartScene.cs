using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Genie.Logics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            var standalponeToggle = GameObject.Find("StandaloneToggle").GetComponent<Toggle>();
            var startButtonObj = GameObject.Find("StartButton");
            var startButton = startButtonObj.GetComponent<Button>();
            await startButton.OnClickAsync(token);

            LocalStorage.SaveShownQuestCodes(Array.Empty<long>());
            
            return new Result()
            {
                IsStandalone = standalponeToggle.isOn,
                ApiBaseUrl = "http://localhost:5000/api/"
            };
        }
    }
}