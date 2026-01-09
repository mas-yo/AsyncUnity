using Cysharp.Threading.Tasks;
using Lua;
using UnityEngine;

namespace Genie.Views
{
    [LuaObject]
    public partial class RockView
    {
        public static async UniTask<RockView> CreateAsync(string prefabPath, Vector3 initialPosition)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);
            obj.transform.position = initialPosition;

            return new RockView();
        }
    }
}