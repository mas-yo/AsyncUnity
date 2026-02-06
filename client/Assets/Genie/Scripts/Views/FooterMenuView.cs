
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
    
        public async UniTask<QuestButtonClicked> OnClickQuestButtonAsync(CancellationToken token)
        {
            return await _components.QuestButton.OnClickAsync(token).ContinueWith(() => new QuestButtonClicked());
        }

        public async UniTask<PresentBoxButtonClicked> OnClickPresentBoxButtonAsync(CancellationToken token)
        {
            return await _components.PresentBoxButton.OnClickAsync(token).ContinueWith(() => new PresentBoxButtonClicked());
        }
        public async UniTask<OptionButtonClicked> OnClickOptionButtonAsync(CancellationToken token)
        {
            return await _components.OptionButton.OnClickAsync(token).ContinueWith(() => new OptionButtonClicked());
        }
    }
}