using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace Genie.MasterData
{
    [MemoryTable("Quest"), MessagePackObject(false)]
    public record QuestMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        
        [Key(1)]
        public string Name { get; init; }
        
        [Key(2)]
        public string Description { get; init; }
        
        [SecondaryKey(0)]
        [Key(3)]
        public long StageCode { get; init; }
        
        [Key(4)]
        public long RequiredQuestCode { get; init; }
        
        [Key(5)]
        public int RewardExp { get; init; }
        
        [Key(6)]
        public int RewardGold { get; init; }
        
        [Key(7)]
        public long RewardItemCode { get; init; }
        
        [Key(8)]
        public int SortOrder { get; init; }

        public static QuestMaster FromDictionary(IReadOnlyDictionary<string, string> dict)
        {
            return new QuestMaster()
            {
                Code = long.Parse(dict["Code"]),
                Name = dict["Name"],
                Description = dict["Description"],
                StageCode = long.Parse(dict["StageCode"]),
                RequiredQuestCode = dict.TryGetValue("RequiredQuestCode", out var reqQuestCode) && !string.IsNullOrEmpty(reqQuestCode) 
                    ? long.Parse(reqQuestCode) 
                    : 0,
                RewardExp = int.Parse(dict["RewardExp"]),
                RewardGold = int.Parse(dict["RewardGold"]),
                RewardItemCode = dict.TryGetValue("RewardItemCode", out var itemCode) && !string.IsNullOrEmpty(itemCode) 
                    ? long.Parse(itemCode) 
                    : 0,
                SortOrder = int.Parse(dict["SortOrder"]),
            };
        }
    }
}

