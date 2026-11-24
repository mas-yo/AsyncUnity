using System.Collections.Generic;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public class EntityContainer
    {
        private List<GameEntity> _listEntities = new();
        private Dictionary<Vector3Int, HashSet<GameEntity>> _positionIndexedEntities = new();

        private void SetEntityToPosition(Vector3Int pos, GameEntity entity)
        {
            if (_positionIndexedEntities.TryGetValue(pos, out var entitySet))
            {
                entitySet.Add(entity);
            }
            else
            {
                _positionIndexedEntities.Add(pos, new() {entity});
            }
        }
        private void RemoveEntityFromPosition(Vector3Int pos, GameEntity entity)
        {
            if (_positionIndexedEntities.TryGetValue(pos, out var entitySet))
            {
                entitySet.Remove(entity);
            }
        }
        public void AddNewEntity(GameEntity entity)
        {
            _listEntities.Add(entity);
            SetEntityToPosition(entity.View.GridPosition, entity);
        }
        public void MoveEntityPosition(Vector3Int beforePos, Vector3Int currentPos, GameEntity entity)
        {
            RemoveEntityFromPosition(beforePos, entity);
            SetEntityToPosition(currentPos, entity);
        }
        public IEnumerable<GameEntity> EnumerateEntities()
        {
            return _listEntities;
        }
        public IEnumerable<GameEntity> EnumerateNeighborEntities(Vector3Int pos, int range)
        {
            for (var z = -range; z <= range; z++)
            {
                for (var y = -range; y < range; y++)
                {
                    for (var x = -range; x <= range; x++)
                    {
                        if (_positionIndexedEntities.TryGetValue(pos + new Vector3Int(x, y, z), out var entitySet))
                        {
                            foreach (var entity in entitySet)
                            {
                                yield return entity;
                            }
                        }
                    }
                }
            }
        }
    }
}