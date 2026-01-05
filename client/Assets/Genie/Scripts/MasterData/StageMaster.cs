using System.Collections.Generic;

namespace Genie.MasterData
{
    public record StageMaster
    {
        public long Code;
        public string Name;
        public string GroundPrefabPath;

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