using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Genie.Scenes
{
    public static class HomeScene
    {
        public struct Result
        {
            public bool IsHome { get; init; }
        }

        public static async UniTask<Result> StartAsync(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("HomeScene", LoadSceneMode.Single);
            var homeMenuPrefab = await Resources.LoadAsync<GameObject>("HomeMenu");
            Object.Instantiate(homeMenuPrefab);
            var questButton = GameObject.Find("QuestButton").GetComponent<Button>();
            await questButton.OnClickAsync(token);
            return new Result
            {
                IsHome = true
            };
        }
    }
}
