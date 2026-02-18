﻿﻿using System.Collections.Generic;
using AsyncUnity.MasterData;
using AsyncUnity.Scripts.MasterData;
using MasterMemory;

namespace AsyncUnity.Logics
{
    public static class MasterMemoryBuilder
    {
        public static MemoryDatabase Build(IEnumerable<(string tableName, IReadOnlyDictionary<string, string> dict)> rows)
        {
            var builder = new DatabaseBuilder();


            var characters = new List<PlayerMaster>();
            var stages = new List<StageMaster>();
            var items = new List<ItemMaster>();
            var objects = new List<ObjectMaster>();
            var quests = new List<QuestMaster>();
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
                else if (string.Equals(row.tableName, "ObjectMaster"))
                {
                    objects.Add(ObjectMaster.FromDictionary(row.dict));
                }
                else if (string.Equals(row.tableName, "QuestMaster"))
                {
                    quests.Add(QuestMaster.FromDictionary(row.dict));
                }
            }

            builder.Append(characters);
            builder.Append(stages);
            builder.Append(items);
            builder.Append(objects);
            builder.Append(quests);

            return new MemoryDatabase(builder.Build());
        }
    }
}