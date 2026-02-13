using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Genie.Views
{
    public static class QuestListView
    {
        public struct Result
        {
            public long QuestCode;
        }

        public static async UniTask<Result> ShowAsync(QuestListViewComponents components, long[] questCodes,
            CancellationToken token)
        {
            var buttons = new List<Button>();
            try
            {
                var tasks = new UniTask<long>[questCodes.Length];

                for (var i = 0; i < questCodes.Length; i++)
                {
                    var questCode = questCodes[i];
                    var button = Object.Instantiate(components.QuestListEntryPrefab, components.ButtonsParent).GetComponent<Button>();
                    tasks[i] = button.OnClickAsync(token).ContinueWith(() => questCode);
                    buttons.Add(button);
                }

                var resultQuestCode = await UniTask.WhenAny(tasks);
                return new Result { QuestCode = resultQuestCode.result };
            }
            finally
            {
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
        }
    }
}