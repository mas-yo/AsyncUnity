using System.Collections.Generic;
using System.Linq;

namespace Genie.MasterData
{
    public record MasterData
    {
        public CharacterMaster[] Characters;
        public StageMaster[] Stages;
        public ItemMaster[] Items;

        public static MasterData FromDictionary(IEnumerable<(string tableName, IReadOnlyDictionary<string, string> dict)> rows)
        {
            var characters = new List<CharacterMaster>();
            var stages = new List<StageMaster>();
            var items = new List<ItemMaster>();
            foreach (var row in rows)
            {
                if (string.Equals(row.tableName, "CharacterMaster"))
                {
                    characters.Add(CharacterMaster.FromDictionary(row.dict));
                }
                else if (string.Equals(row.tableName, "StageMaster"))
                {
                    stages.Add(StageMaster.FromDictionary(row.dict));
                }
                else if (string.Equals(row.tableName, "ItemMaster"))
                {
                    items.Add(ItemMaster.FromDictionary(row.dict));
                }
            }
            return new MasterData()
            {
                Characters = characters.ToArray(),
                Stages = stages.ToArray(),
                Items = items.ToArray(),
            };
        }
    }
}