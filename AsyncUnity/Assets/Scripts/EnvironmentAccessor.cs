using System;
using AsyncUnity.Scenes.Game1;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public struct EnvironmentAccessor
    {
        public Func<string, GameEntity> FindEntityByNameFunc;
        public Func<string, GameEntity[]> GetEntitiesByCodeFunc;
        public Func<Vector3Int, GameEntity[]> GetEntitiesOfPositionFunc;
    }
}