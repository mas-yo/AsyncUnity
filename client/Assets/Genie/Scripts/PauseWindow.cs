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
            
            var isExit = await UniTaskUtil.WaitAndDo(token,
                ( playButton.OnClickAsync(token), () => false),
                ( exitButton.OnClickAsync(token), () => true)
            );
            
            Object.Destroy(pauseWindowObj);
            return new Result() { IsExit = isExit };
        }
        

    }
}