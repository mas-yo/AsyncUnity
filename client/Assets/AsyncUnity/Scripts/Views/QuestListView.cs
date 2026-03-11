using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using AsyncUnity.Utils;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsyncUnity.Views
{
    public class QuestListView
    {
        private readonly QuestListViewComponents _components;
        private readonly long[] _questCodes;
        private readonly Func<CancellationToken, UniTask<long>>[] _onClickTasks;

        public struct Result
        {
            public long QuestCode;
        }

        public QuestListView(QuestListViewComponents components, long[] questCodes)
        {
            _components = components;
            _questCodes = questCodes;
            _onClickTasks = new Func<CancellationToken, UniTask<long>>[questCodes.Length];

            for (var i = 0; i < questCodes.Length; i++)
            {
                var questCode = questCodes[i];
                var button = Object.Instantiate(components.QuestListEntryPrefab, components.ButtonsParent).GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = $"QUEST: {questCode}";
                _onClickTasks[i] = (t) => button.OnClickAsync(t).ContinueWith(() => questCode);
            }
        }

        public void Show()
        {
            _components.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _components.gameObject.SetActive(false);
        }
        public async UniTask<Result> OnClickQuestButtonAsync(CancellationToken token)
        {
            var resultQuestCode = await UniTaskUtil.WaitAndCancel(token, _onClickTasks);
            return new Result { QuestCode = resultQuestCode };
        }
    }
}