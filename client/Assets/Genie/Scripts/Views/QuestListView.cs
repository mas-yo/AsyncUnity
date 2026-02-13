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
    public static class QuestListView
    {
        public struct Result
        {
            public long QuestCode;
        }

        public static Func<CancellationToken, UniTask<Result>> ShowAsync(QuestListViewComponents components, long[] questCodes)
        {
            return (token) => ShowAsync(components, questCodes, token);
        }
        public static async UniTask<Result> ShowAsync(QuestListViewComponents components, long[] questCodes,
            CancellationToken token)
        {
            components.gameObject.SetActive(true);
            var buttons = new List<Button>();
            try
            {
                var tasks = new Func<CancellationToken, UniTask<long>>[questCodes.Length];

                for (var i = 0; i < questCodes.Length; i++)
                {
                    var questCode = questCodes[i];
                    var button = Object.Instantiate(components.QuestListEntryPrefab, components.ButtonsParent).GetComponent<Button>();
                    tasks[i] = (token) => button.OnClickAsync(token).ContinueWith(() => questCode);
                    buttons.Add(button);
                }

                var resultQuestCode = await UniTaskUtil.WaitAndCancel(token, tasks);
                return new Result { QuestCode = resultQuestCode };
            }
            finally
            {
                components.gameObject.SetActive(false);
                foreach (var button in buttons)
                {
                    Object.Destroy(button.gameObject);
                }
            }
        }
    }
}