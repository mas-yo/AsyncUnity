using System.Collections.Generic;
using Genie.Utils;
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

        public static CharacterMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new CharacterMaster()
            {
                Code = long.Parse(dict["Code"]),
                Name = dict["Name"],
                ModelPrefabPath = dict["ModelPrefabPath"],
                InitialPosition = new Vector3(float.Parse(dict["InitialPosition.x"]), float.Parse(dict["InitialPosition.y"]), float.Parse(dict["InitialPosition.z"])),
                MoveSpeed = float.Parse(dict["MoveSpeed"]),
            };
        }
    }
}