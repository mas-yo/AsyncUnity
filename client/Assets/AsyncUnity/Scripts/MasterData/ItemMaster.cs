﻿using System.Collections.Generic;
using UnityEngine;
using MasterMemory;
using MessagePack;

namespace AsyncUnity.MasterData
{
    [MemoryTable("Item"), MessagePackObject(false)]
    public record ItemMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        [Key(1)]
        public string Name { get; init; }
        [Key(2)]
        public string Description { get; init; }
        [Key(3)]
        public string IconPath { get; init; }
        [Key(4)]
        public int Rarity { get; init; }
        [Key(5)]
        public string PrefabPath { get; init; }
        [Key(6)]
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