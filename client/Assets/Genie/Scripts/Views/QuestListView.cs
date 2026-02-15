using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Utils;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Genie.Views
{
    public class QuestListView
    {
        private readonly QuestListViewComponents _components;
        private readonly long[] _questCodes;
        
        public struct Result
        {
            public long QuestCode;
        }

        public QuestListView(QuestListViewComponents components, long[] questCodes)
        {
            _components = components;
            _questCodes = questCodes;
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
            var buttons = new List<Button>();
            try
            {
                var tasks = new Func<CancellationToken, UniTask<long>>[_questCodes.Length];

                for (var i = 0; i < _questCodes.Length; i++)
                {
                    var questCode = _questCodes[i];
                    var button = Object.Instantiate(_components.QuestListEntryPrefab, _components.ButtonsParent).GetComponent<Button>();
                    button.GetComponentInChildren<Text>().text = $"QUEST: {questCode}";
                    tasks[i] = (t) => button.OnClickAsync(t).ContinueWith(() => questCode);
                    buttons.Add(button);
                }

                var resultQuestCode = await UniTaskUtil.WaitAndCancel(token, tasks);
                return new Result { QuestCode = resultQuestCode };
            }
            finally
            {
                Hide();
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
        }
    }
}