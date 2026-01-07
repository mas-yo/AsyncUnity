using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
{
    [MemoryTable("Stage"), MessagePackObject(true)]
    public record StageMaster
    {
        [PrimaryKey]
        public long Code { get; init; }
        public string Name { get; init; }
        public string GroundPrefabPath { get; init; }
        public string SceneName { get; init; }
        public int Difficulty { get; init; }
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