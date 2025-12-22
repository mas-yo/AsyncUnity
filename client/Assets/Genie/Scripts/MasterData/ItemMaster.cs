using UnityEngine;

namespace Genie.MasterData
{
    public record ItemMaster
    {
        public long Code;
        public string prefabPath;
        public Vector3 position;
    }
}