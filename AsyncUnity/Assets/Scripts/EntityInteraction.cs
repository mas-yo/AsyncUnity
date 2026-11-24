using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public static class EntityInteraction
    {
        public static bool IsOtherEntityUnder(IReadOnlyGameEntity self, IReadOnlyGameEntity other)
        {
            if (self.View.GridPosition.y <= 0)
            {
                return true;
            }
            return other.View.Name != self.View.Name &&
                   self.View.GridPosition - other.View.GridPosition == new Vector3Int(0, 1, 0);
        }

        public static bool IsOtherEntityIsOver(IReadOnlyGameEntity self, IReadOnlyGameEntity other)
        {
            return other.View.Name != self.View.Name &&
                   other.View.GridPosition == self.View.GridPosition;
        }
        public static async UniTask<IReadOnlyGameEntity> WaitOtherEntityOverAsync(IReadOnlyGameEntity entity, IEnumerable<IReadOnlyGameEntity> otherEntities, CancellationToken token)
        {
            IReadOnlyGameEntity? triggered = null;
            
            await UniTask.WaitUntil(() =>
            {
                foreach (var other in otherEntities)
                {
                    if (IsOtherEntityIsOver(entity, other))
                    {
                        triggered = other;
                        break;
                    }
                }
                return triggered != null;
            }, cancellationToken: token);

            return triggered!;
        }

        public static async UniTask WaitNothingIsOverAsync(IReadOnlyGameEntity entity, IEnumerable<IReadOnlyGameEntity> otherEntities, CancellationToken token)
        {
            await UniTask.WaitUntil(() =>
                    otherEntities.All(other => !IsOtherEntityIsOver(entity, other)),
                cancellationToken: token);
        }

        public static bool IsOtherEntityHit(IReadOnlyGameEntity self, IEnumerable<IReadOnlyGameEntity> otherEntities)
        {
            return otherEntities.Any(other =>
                other.View.Name != self.View.Name
                && other.Master.Altitude == self.Master.Altitude
                && other.View.RoundedPosition == self.View.RoundedPosition
                && other.Master.Weight >= self.Master.Weight);
        }
        
        public static bool IsLookingAt(IReadOnlyGameEntity self, IReadOnlyGameEntity other)
        {
            return self.View.FrontPosition == other.View.GridPosition;
        }
        
    }
}