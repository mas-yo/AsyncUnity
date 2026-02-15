
using System;
using MessagePack;
using UnityEngine;

namespace Genie.Logics
{
    [MessagePackObject]
    public struct ShownQuestCodes
    {
        [Key(0)]
        public long[] QuestCodes;
    }
    public static class LocalStorage
    {
        public static void SaveShownQuestCodes(long[] questCodes)
        {
            var data = new ShownQuestCodes { QuestCodes = questCodes };
            var bytes = MessagePackSerializer.Serialize(data);
            var str  = System.Convert.ToBase64String(bytes);
            PlayerPrefs.SetString("ShownQuestCodes", str);
        }
        public static long[] LoadShownQuestCodes()
        {
            if (!PlayerPrefs.HasKey("ShownQuestCodes"))
            {
                return Array.Empty<long>();
            }
            var str = PlayerPrefs.GetString("ShownQuestCodes");
            var bytes = System.Convert.FromBase64String(str);
            var data = MessagePackSerializer.Deserialize<ShownQuestCodes>(bytes);
            return data.QuestCodes;
        }
    }
}