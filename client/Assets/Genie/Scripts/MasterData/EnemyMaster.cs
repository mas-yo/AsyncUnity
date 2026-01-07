using System.Collections.Generic;
using UnityEngine;

namespace Genie.MasterData
{
    public record EnemyMaster
    {
        public long Code;
        public string PrefabPath;
        public float Health;
        public float AttackPower;
        public Vector3 InitialPosition;

        public static EnemyMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new EnemyMaster()
            {
                Code = long.Parse(dict["Code"]),
                PrefabPath = dict["PrefabPath"],
                Health = float.Parse(dict["Health"]),
                AttackPower = float.Parse(dict["AttackPower"]),
                InitialPosition = new Vector3(float.Parse(dict["InitialPosition.x"]), float.Parse(dict["InitialPosition.y"]), float.Parse(dict["InitialPosition.z"])),
            };
        }
    }
}