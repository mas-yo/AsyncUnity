using System.Collections.Generic;
using MasterMemory;
using MessagePack;
using UnityEngine;

namespace Genie.MasterData
{
    [MemoryTable("Character"), MessagePackObject(false)]
    public record CharacterMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        [Key(1)]
        public string Name { get; init; }
        [Key(2)]
        public string ModelPrefabPath { get; init; }
        [Key(3)]
        public Vector3 InitialPosition { get; init; }
        [Key(4)]
        public float MoveSpeed { get; init; }

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