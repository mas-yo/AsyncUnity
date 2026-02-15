using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using Genie.MasterData;
using Genie.Protocols;
using Genie.Utils;
using Genie.Views;
using Genie.Logics;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static async UniTask<Result> StartAsync(
            MemoryDatabase masterData,
            UserInfo userInfo,
            HomeViewType homeViewType,
            CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("HomeScene", LoadSceneMode.Single);
            var questListView = new QuestListView(Object.FindAnyObjectByType<QuestListViewComponents>(), userInfo.ClearedQuestCodes);
            var presentBoxView = new PresentBoxView(Object.FindAnyObjectByType<PresentBoxViewComponents>());
            var footerView = new FooterMenuView(Object.FindAnyObjectByType<FooterMenuViewComponents>());
            var newQuestOpenedVfx = new NewQuestOpenedVfx(Object.FindAnyObjectByType<NewQuestOpenedVfxComponents>());
            
            questListView.SetActive(false);
            presentBoxView.SetActive(false);
            footerView.SetActive(true);
            newQuestOpenedVfx.SetActive(false);

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
                        questListView.SetActive(true);
                        var prevQuestCodes = LocalStorage.LoadShownQuestCodes();
                        var currentQuestCodes = userInfo.ClearedQuestCodes;
                        if (currentQuestCodes.Except(prevQuestCodes).Any())
                        {
                            newQuestOpenedVfx.SetActive(true);
                            await newQuestOpenedVfx.PlayAsync();
                            newQuestOpenedVfx.SetActive(false);
                            LocalStorage.SaveShownQuestCodes(currentQuestCodes);
                        }
                        var questResult = await UniTaskUtil.WhenAnyWithCancel(token,
                            questListView.OnClickQuestButtonAsync,
                            footerView.OnClickPresentBoxButtonAsync,
                            footerView.OnClickOptionButtonAsync);

                        questListView.SetActive(false);
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
                        presentBoxView.SetActive(true);
                        var result = await UniTaskUtil.WhenAnyWithCancel(token,
                            presentBoxView.OnClickPresentButton(),
                            footerView.OnClickQuestButtonAsync,
                            footerView.OnClickOptionButtonAsync);
                        
                        presentBoxView.SetActive(false);

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
