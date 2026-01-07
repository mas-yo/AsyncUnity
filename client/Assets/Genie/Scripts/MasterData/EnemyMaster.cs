using System.Collections.Generic;
using UnityEngine;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
{
    [MemoryTable("Enemy"), MessagePackObject(true)]
    public record EnemyMaster
    {
        [PrimaryKey]
        public long Code { get; init; }
        public string PrefabPath { get; init; }
        public float Health { get; init; }
        public float AttackPower { get; init; }
        public Vector3 InitialPosition { get; init; }

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