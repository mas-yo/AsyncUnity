using System.Collections.Generic;
using UnityEngine;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
{
    [MemoryTable("Enemy"), MessagePackObject(false)]
    public record EnemyMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        [Key(1)]
        public string PrefabPath { get; init; }
        [Key(2)]
        public float Health { get; init; }
        [Key(3)]
        public float AttackPower { get; init; }
        [Key(4)]
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