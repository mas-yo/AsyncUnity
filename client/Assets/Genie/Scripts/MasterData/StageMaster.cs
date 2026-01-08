using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
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
        [Key(3)]
        public string SceneName { get; init; }
        [Key(4)]
        public int Difficulty { get; init; }
        [Key(5)]
        public string Description { get; init; }

        public static StageMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new StageMaster()
            {
                Code = long.Parse(dict["Code"]),
                Name = dict["Name"],
                GroundPrefabPath = dict["GroundPrefabPath"],
                SceneName = dict["SceneName"],
                Difficulty = int.Parse(dict["Difficulty"]),
                Description = dict["Description"],
            };
        }
    }
}