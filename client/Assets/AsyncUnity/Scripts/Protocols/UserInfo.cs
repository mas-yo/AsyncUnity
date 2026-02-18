﻿using System.Collections.Generic;
using MessagePack;

namespace AsyncUnity.Protocols
{
    [MessagePackObject(false)]
    public record UserInfo()
    {
        [Key(0)]
        public string UserId;
        [Key(1)]
        public string UserName;
        [Key(2)]
        public long[] ClearedQuestCodes;
    }
}