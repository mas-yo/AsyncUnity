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
        
        public struct Result
        {
            public long QuestCode;
        }

        public QuestListView(QuestListViewComponents components)
        {
            _components = components;
        }

        public void SetActive(bool active)
        {
            _components.gameObject.SetActive(active);
        }
        public Func<CancellationToken, UniTask<Result>> OnClickQuestButton(long[] questCodes)
        {
            return (token) => OnClickQuestButtonAsync(questCodes, token);
        }
        public async UniTask<Result> OnClickQuestButtonAsync(long[] questCodes, CancellationToken token)
        {
            var buttons = new List<Button>();
            try
            {
                var tasks = new Func<CancellationToken, UniTask<long>>[questCodes.Length];

                for (var i = 0; i < questCodes.Length; i++)
                {
                    var questCode = questCodes[i];
                    var button = Object.Instantiate(_components.QuestListEntryPrefab, _components.ButtonsParent).GetComponent<Button>();
                    tasks[i] = (t) => button.OnClickAsync(t).ContinueWith(() => questCode);
                    buttons.Add(button);
                }

                var resultQuestCode = await UniTaskUtil.WaitAndCancel(token, tasks);
                return new Result { QuestCode = resultQuestCode };
            }
            finally
            {
                _components.gameObject.SetActive(false);
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
        }
    }
}