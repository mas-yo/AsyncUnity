using System.Collections.Generic;
using UnityEngine;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
{
    [MemoryTable("Item"), MessagePackObject(true)]
    public record ItemMaster
    {
        [PrimaryKey]
        public long Code { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string IconPath { get; init; }
        public int Rarity { get; init; }
        public string PrefabPath { get; init; }
        public Vector3 Position { get; init; }

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