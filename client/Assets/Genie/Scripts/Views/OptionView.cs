using System.Threading;
using Cysharp.Threading.Tasks;

namespace Genie.Views
{
    public class OptionView
    {
        public struct Result
        {
            
        }
        public async static UniTask<Result> ShowAsync(CancellationToken token)
        {
            await UniTask.DelayFrame(1, cancellationToken: token);
            return new Result();
        }
    }
}