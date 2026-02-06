using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Genie.Views
{
    public class QuestListView
    {
        public struct Result
        {
            public long QuestCode;
        }
        public static async UniTask<Result> ShowAsync(QuestListViewComponents components, long[] questCodes, CancellationToken token)
        {
            var tasks = questCodes.Select(code =>
            {
                var button =
                    ((GameObject)Object.Instantiate(components.QuestListEntryPrefab, components.ButtonsParent)).GetComponent<Button>();
                return button.OnClickAsync(token).ContinueWith(() => code);
            }).ToArray();

            return await UniTask.WhenAny(tasks).ContinueWith(x => new Result{ QuestCode = x.result });
        }
    }
}