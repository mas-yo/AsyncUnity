using UnityEngine;

namespace Genie.MasterData
{
    public record CharacterMaster
    {
        public long Code;
        public string Name;
        public string ModelPrefabPath;
        public Vector3 InitialPosition;
        public float MoveSpeed;
    }
}