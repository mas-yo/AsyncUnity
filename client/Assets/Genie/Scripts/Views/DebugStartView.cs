using System.Threading;
using Cysharp.Threading.Tasks;

namespace Genie.Views
{
    public class DebugStartView
    {
        public struct StartButtonClicked { }
        public struct ClearLocalSaveButtonClicked { }
        private readonly DebugStartViewComponents _components;
        
        public DebugStartView(DebugStartViewComponents components)
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
        
        public UniTask<StartButtonClicked> OnClickStartButtonAsync(CancellationToken token)
        {
            return _components.StartButton.OnClickAsync(token).ContinueWith(() => new StartButtonClicked());
        }

        public UniTask<ClearLocalSaveButtonClicked> OnClickClearLocalSaveButtonAsync(CancellationToken token)
        {
            return _components.ClearLocalSaveButton.OnClickAsync(token).ContinueWith(() => new ClearLocalSaveButtonClicked());
        }

    }
}