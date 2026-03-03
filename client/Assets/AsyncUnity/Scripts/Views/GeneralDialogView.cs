using System.Threading;
using Cysharp.Threading.Tasks;
using AsyncUnity.Utils;

namespace AsyncUnity.Views
{
    public class GeneralDialogView
    {
        public struct Result
        {
            public bool IsOk;
        }

        private readonly GeneralDialogViewComponents _components;

        public GeneralDialogView(GeneralDialogViewComponents components)
        {
            _components = components;
        }

        public void Show(string title, string message)
        {
            _components.TitleText.text = title;
            _components.MessageText.text = message;
            _components.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _components.gameObject.SetActive(false);
        }

        public async UniTask<Result> OnClickButtonsAsync(CancellationToken token)
        {
            return await UniTaskUtil.WaitAndCancel<Result>(token,
                t => _components.OkButton.OnClickAsync(t).ContinueWith(() => new Result { IsOk = true }),
                t => _components.CancelButton.OnClickAsync(t).ContinueWith(() => new Result { IsOk = false })
            );
        }
    }
}
