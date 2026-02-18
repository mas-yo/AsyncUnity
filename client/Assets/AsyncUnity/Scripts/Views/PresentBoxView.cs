﻿﻿﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using AsyncUnity.Utils;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsyncUnity.Views
{
    public class PresentBoxView
    {
        private readonly PresentBoxViewComponents _components;
        public struct Result
        {
            public long dummy;
        }
        
        public PresentBoxView(PresentBoxViewComponents components)
        {
            _components = components;
        }

        public void Show()
        {
            _components.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _components.gameObject.SetActive(false);
        }
        public Func<CancellationToken, UniTask<Result>> OnClickPresentButton()
        {
            return OnClickPresentButtonAsync;
        }
        public async UniTask<Result> OnClickPresentButtonAsync(CancellationToken token)
        {
            var dummyPresentCodes = new long[] { 1001, 1002, 1003 };
            var buttons = new List<Button>();
            try
            {
                var tasks = new Func<CancellationToken, UniTask<long>>[dummyPresentCodes.Length];

                for (var i = 0; i < dummyPresentCodes.Length; i++)
                {
                    var code = dummyPresentCodes[i];
                    var button = Object.Instantiate(_components.PresentBoxEntryPrefab, _components.ButtonsParent).GetComponent<Button>();
                    button.GetComponentInChildren<Text>().text = $"PRESENT: {code}";
                    tasks[i] = (t) => button.OnClickAsync(t).ContinueWith(() => code);
                    buttons.Add(button);
                }

                var resultCode = await UniTaskUtil.WaitAndCancel(token, tasks);
                return new Result { dummy = resultCode };
            }
            finally
            {
                Hide();
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
            
        }
    }
}