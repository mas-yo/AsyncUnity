﻿using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace AsyncUnity.MasterData
{
    [MemoryTable("Stage"), MessagePackObject(false)]
    public record StageMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        [Key(1)]
        public string Name { get; init; }
        [Key(2)]
        public string GroundPrefabPath { get; init; }

        public static StageMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new StageMaster()
            {
                Code = long.Parse(dict["Code"]),
                Name = dict["Name"],
                GroundPrefabPath = dict["GroundPrefabPath"],
            };
        }
    }
}