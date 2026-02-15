﻿
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
    
        public FooterMenuView(FooterMenuViewComponents components)
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
        public UniTask<QuestButtonClicked> OnClickQuestButtonAsync(CancellationToken token)
        {
            return _components.QuestButton.OnClickAsync(token).ContinueWith(() => new QuestButtonClicked());
        }

        public UniTask<PresentBoxButtonClicked> OnClickPresentBoxButtonAsync(CancellationToken token)
        {
            return _components.PresentBoxButton.OnClickAsync(token).ContinueWith(() => new PresentBoxButtonClicked());
        }
        public UniTask<OptionButtonClicked> OnClickOptionButtonAsync(CancellationToken token)
        {
            return _components.OptionButton.OnClickAsync(token).ContinueWith(() => new OptionButtonClicked());
        }
    }
}