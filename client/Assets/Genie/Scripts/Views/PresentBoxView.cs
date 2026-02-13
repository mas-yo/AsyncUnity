using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Utils;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Genie.Views
{
    public class PresentBoxView
    {
        public struct Result
        {
            public long dummy;
        }
        public static async UniTask<Result> ShowAsync(PresentBoxViewComponents components, CancellationToken token)
        {
            components.gameObject.SetActive(true);
            var dummyPresentCodes = new long[] { 1001, 1002, 1003 };
            var buttons = new List<Button>();
            try
            {
                var tasks = new Func<CancellationToken, UniTask<long>>[dummyPresentCodes.Length];

                for (var i = 0; i < dummyPresentCodes.Length; i++)
                {
                    var code = dummyPresentCodes[i];
                    var button = Object.Instantiate(components.PresentBoxEntryPrefab, components.ButtonsParent).GetComponent<Button>();
                    tasks[i] = (token) => button.OnClickAsync(token).ContinueWith(() => code);
                    buttons.Add(button);
                }

                var resultCode = await UniTaskUtil.WaitAndCancel(token, tasks);
                return new Result { dummy = resultCode };
            }
            finally
            {
                components.gameObject.SetActive(false);
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
            
        }
    }
}