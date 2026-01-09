using System.Collections.Generic;
using Genie.MasterData;
using MasterMemory;

namespace Genie.Logics
{
    public static class MasterMemoryBuilder
    {
        public static MemoryDatabase Build(IEnumerable<(string tableName, IReadOnlyDictionary<string, string> dict)> rows)
        {
            var builder = new DatabaseBuilder();


            var characters = new List<PlayerMaster>();
            var stages = new List<StageMaster>();
            var items = new List<ItemMaster>();
            foreach (var row in rows)
            {
                if (string.Equals(row.tableName, "PlayerMaster"))
                {
                    characters.Add(PlayerMaster.FromDictionary(row.dict));
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

            builder.Append(characters);
            builder.Append(stages);
            builder.Append(items);

            return new MemoryDatabase(builder.Build());
        }
    }
}