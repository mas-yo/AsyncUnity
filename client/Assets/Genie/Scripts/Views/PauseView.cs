using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Utils;

namespace Genie.Views
{
    public class PauseView
    {
        public struct Result
        {
            public bool ToTitle;
        }

        private PauseViewComponents _components;

        public PauseView(PauseViewComponents components)
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

        public async UniTask<Result> OnClickButtonsAsync(CancellationToken token)
        {
            return await UniTaskUtil.WaitAndCancel<Result>(token, 
                t => _components.ResumeButton.OnClickAsync(t).ContinueWith(() => new Result() { ToTitle = false }),
                    t => _components.ToTitleButton.OnClickAsync(t).ContinueWith(() => new Result() { ToTitle = true })
                );
            
        }
    }
}