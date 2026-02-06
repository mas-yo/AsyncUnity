using System.Threading;
using Cysharp.Threading.Tasks;

namespace Genie.Views
{
    public class PresentBoxView
    {
        public struct Result
        {
            public int dummy;
        }
        public static async UniTask<Result> ShowAsync(PresentBoxViewComponents components, CancellationToken token)
        {
            await UniTask.DelayFrame(1, cancellationToken: token);
            return new Result { dummy = 0 }; 
        }
    }
}