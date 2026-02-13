
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Genie.Views
{
    public class FooterMenuView
    {
        public struct QuestButtonClicked { }
        public struct PresentBoxButtonClicked { }
        public struct OptionButtonClicked { }
        
        private readonly FooterMenuViewComponents _components;
    
        public static async UniTask<FooterMenuView> CreateAsync(CancellationToken token)
        {
            var bridge = Object.FindAnyObjectByType<FooterMenuViewComponents>();
            return new FooterMenuView(bridge);
        }

        public FooterMenuView(FooterMenuViewComponents components)
        {
            _components = components;
        }
    
        public Func<CancellationToken, UniTask<QuestButtonClicked>> OnClickQuestButtonAsync()
        {
            return (token) => _components.QuestButton.OnClickAsync(token).ContinueWith(() => new QuestButtonClicked());
        }

        public Func<CancellationToken, UniTask<PresentBoxButtonClicked>> OnClickPresentBoxButtonAsync()
        {
            return (token) => _components.PresentBoxButton.OnClickAsync(token).ContinueWith(() => new PresentBoxButtonClicked());
        }
        public Func<CancellationToken, UniTask<OptionButtonClicked>> OnClickOptionButtonAsync()
        {
            return (token) => _components.OptionButton.OnClickAsync(token).ContinueWith(() => new OptionButtonClicked());
        }
    }
}