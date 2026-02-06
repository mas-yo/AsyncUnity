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


            var viewType = homeViewType;
            while (true)
            {
                switch (viewType)
                {
                    case HomeViewType.HomeMenu:
                        await StartHomeMenuAsync(token);
                        break;
                    
                    case HomeViewType.QuestList:
                        var questResult = await UniTask.WhenAny(
                            QuestListView.ShowAsync(questListViewComponents, new [] {1L,2L}, token),
                            footerView.OnClickPresentBoxButtonAsync(token),
                            footerView.OnClickOptionButtonAsync(token)
                            );

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
                        await PresentBoxView.ShowAsync(presentBoxViewComponents, token);
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
