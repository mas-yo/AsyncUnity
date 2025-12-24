using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.Protocols;

namespace Genie
{
    public class ApiGate
    {
        public async UniTask<UserInfo> LoginAsync()
        {
            await UniTask.Delay(1000);
            return new UserInfo
            {
                UserId = "user_12345",
                UserName = "GenieUser",
                CurrentStageCode = 1,
            };
        }
        public async UniTask SaveScoreAsync(long stageCode, int score, CancellationToken token)
        {
            await UniTask.Delay(500);
        }
    }
}