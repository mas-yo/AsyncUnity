using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Utils
{
    public static class UniTaskUtil
    {
        public static async UniTask<T> WaitAndDo<T>(CancellationToken token, params (UniTask waitTask, Func<T> action)[] conditionsAndActions)
        {
            var index = await UniTask.WhenAny(conditionsAndActions.Select(x => x.waitTask).ToArray());
            return conditionsAndActions[index].action();
        }

        public static async UniTask<(int winArgumentIndex, T1 result1, T2 result2)> WaitAndCancel<T1, T2>(
            CancellationToken token, Func<CancellationToken, UniTask<T1>> func1, Func<CancellationToken, UniTask<T2>> func2)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var result = await UniTask.WhenAny(func1(linkedCts.Token), func2(linkedCts.Token));
            linkedCts.Cancel();
            return result;
        }

        public static async UniTask<(int winArgumentIndex, T1 result1, T2 result2, T3 result3)> WhenAnyWithCancel<T1, T2, T3>(
            CancellationToken token, Func<CancellationToken, UniTask<T1>> func1, Func<CancellationToken, UniTask<T2>> func2, Func<CancellationToken, UniTask<T3>> func3)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var result = await UniTask.WhenAny(func1(linkedCts.Token), func2(linkedCts.Token), func3(linkedCts.Token));
            linkedCts.Cancel();
            return result;
        }

        public static async UniTask<(int winArgumentIndex, T1 result1, T2 result2, T3 result3, T4 result4)> WaitAndCancel<T1, T2, T3, T4>(
            CancellationToken token, Func<CancellationToken, UniTask<T1>> func1, Func<CancellationToken, UniTask<T2>> func2, Func<CancellationToken, UniTask<T3>> func3, Func<CancellationToken, UniTask<T4>> func4)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var result = await UniTask.WhenAny(func1(linkedCts.Token), func2(linkedCts.Token), func3(linkedCts.Token), func4(linkedCts.Token));
            linkedCts.Cancel();
            return result;
        }


        public static async UniTask<T> WaitAndCancel<T>(CancellationToken token, Func<CancellationToken, UniTask<T>>[] funcs)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var result = await UniTask.WhenAny(funcs.Select(t => t(linkedCts.Token)));
            linkedCts.Cancel();
            return result.result;
        }
        
        public static async UniTask WaitAnimationComplete(
            this Animator animator,
            int layer = 0,
            CancellationToken cancellationToken = default)
        {
            await UniTask.WaitUntil(() =>
            {
                var state = animator.GetCurrentAnimatorStateInfo(layer);
                return state.normalizedTime >= 1f && !animator.IsInTransition(layer);
            }, cancellationToken: cancellationToken);
        }

        
    }
}