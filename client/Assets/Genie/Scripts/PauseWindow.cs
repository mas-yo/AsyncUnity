using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Genie
{

    public class PauseWindow
    {
        public struct Result
        {
            public bool IsExit;
        }
        public static async UniTask<Result> StartAsync(CancellationToken token)
        {
            var pauseWindowPrefab = await Resources.LoadAsync<GameObject>("PauseWindow/PauseWindowRoot");
            var pauseWindowObj = (GameObject)Object.Instantiate(pauseWindowPrefab);
            var playButton = GameObject.Find("PlayButton").GetComponent<Button>();
            var exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
            
            var isExit = await WaitAndDo(token,
                ( playButton.OnClickAsync(token), () => false),
                ( exitButton.OnClickAsync(token), () => true)
            );
            
            Object.Destroy(pauseWindowObj);
            return new Result() { IsExit = isExit };
        }
        
        public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (UniTask waitTask, Func<T> action)[] conditionsAndActions)
        {
            var index = await UniTask.WhenAny(conditionsAndActions.Select(x => x.waitTask).ToArray());
            return conditionsAndActions[index].action();
        }
        // public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (Func<bool> condition, Func<T> action)[] conditionsAndActions)
        // {
        //     var index = await UniTask.WhenAny(conditionsAndActions.Select(x => UniTask.WaitUntil(() => x.condition())).ToArray());
        //     return conditionsAndActions[index].action();
        // }

    }
}