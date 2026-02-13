using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Genie.Utils;
using Genie.Views;
using Genie.Windows;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Genie.Scenes
{
    public static class HomeScene
    {
        public enum HomeViewType
        {
            HomeMenu,
            QuestList,
            PresentBox,
            Option
        }
        public struct Result
        {
            public long StartQuestCode { get; init; }
        }

        public static async UniTask<Result> StartAsync(HomeViewType homeViewType, CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("HomeScene", LoadSceneMode.Single);
            var footerView = await FooterMenuView.CreateAsync(token);
            var questListViewComponents = Object.FindAnyObjectByType<QuestListViewComponents>();
            var presentBoxViewComponents = Object.FindAnyObjectByType<PresentBoxViewComponents>();
            questListViewComponents.gameObject.SetActive(false);
            presentBoxViewComponents.gameObject.SetActive(false);

            var viewType = homeViewType;
            while (true)
            {
                switch (viewType)
                {
                    case HomeViewType.HomeMenu:
                        await StartHomeMenuAsync(token);
                        return new Result() { StartQuestCode = 0 };
                        break;
                    
                    case HomeViewType.QuestList:
                        var questResult = await UniTaskUtil.WhenAnyWithCancel(token,
                            QuestListView.ShowAsync(questListViewComponents, new[] { 1l, 2l }),
                            footerView.OnClickPresentBoxButtonAsync(),
                            footerView.OnClickOptionButtonAsync());

                        switch (questResult.winArgumentIndex)
                        {
                            case 0:
                                return new Result
                                {
                                    StartQuestCode = questResult.result1.QuestCode
                                };
                                break;
                            case 1:
                                viewType = HomeViewType.PresentBox;
                                break;
                            case 2:
                                viewType = HomeViewType.Option;
                                break;
                        }
                        break;
                    
                    case HomeViewType.PresentBox:
                        var result = await UniTaskUtil.WhenAnyWithCancel(token,
                            (t) => PresentBoxView.ShowAsync(presentBoxViewComponents, t),
                            footerView.OnClickQuestButtonAsync(),
                            footerView.OnClickOptionButtonAsync());

                        viewType = result.winArgumentIndex switch
                        {
                            0 => HomeViewType.PresentBox,
                            1 => HomeViewType.QuestList,
                            2 => HomeViewType.PresentBox,
                            _ => viewType
                        };
                        break;
                }
                
            }

            
        }
        private static async UniTask StartHomeMenuAsync(CancellationToken token)
        {
            // Home Menuï\é¶èàóù
        }

    }
}
