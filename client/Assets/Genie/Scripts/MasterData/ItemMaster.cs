using System.Collections.Generic;
using UnityEngine;

namespace Genie.MasterData
{
    public record ItemMaster
    {
        public long Code;
        public string PrefabPath;
        public Vector3 Position;

        public static ItemMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new ItemMaster()
            {
                Code = long.Parse(dict["Code"]),
                PrefabPath = dict["PrefabPath"],
                Position = new Vector3(float.Parse(dict["Position.x"]), float.Parse(dict["Position.y"]), float.Parse(dict["Position.z"])),
            };
        }
    }
    
}