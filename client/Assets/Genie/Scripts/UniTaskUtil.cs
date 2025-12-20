using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Genie
{
    public static class UniTaskUtil
    {
        public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (UniTask waitTask, Func<T> action)[] conditionsAndActions)
        {
            var index = await UniTask.WhenAny(conditionsAndActions.Select(x => x.waitTask).ToArray());
            return conditionsAndActions[index].action();
        }
        
    }
}