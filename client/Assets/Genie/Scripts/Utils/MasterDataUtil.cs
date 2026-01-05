using System.Collections.Generic;
using UnityEngine;

namespace Genie.Utils
{
    public static class MasterDataUtil
    {
        public static Vector3 Vector3FromDictionary(IReadOnlyDictionary<string, object> dict)
        {
            return new Vector3(
                float.Parse((string)dict["x"]),
                float.Parse((string)dict["y"]),
                float.Parse((string)dict["z"])
            );
        }
        
    }
}