using System.Collections.Generic;
using MasterMemory;
using MessagePack;
using UnityEngine;

namespace Genie.MasterData
{
    [MemoryTable("Character"), MessagePackObject(true)]
    public record CharacterMaster
    {
        [PrimaryKey]
        public long Code { get; init; }
        public string Name { get; init; }
        public string ModelPrefabPath { get; init; }
        public Vector3 InitialPosition { get; init; }
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