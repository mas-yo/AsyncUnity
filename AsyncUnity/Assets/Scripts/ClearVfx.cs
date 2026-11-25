using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lua;
using UnityEngine;

namespace AsyncUnity.Views
{
    [LuaObject]
    public partial class ClearVfx
    {
        private readonly ClearVfxComponents _components;

        public static async UniTask<ClearVfx> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("ClearVfx/ClearVfx");
            var obj = (GameObject)Object.Instantiate(prefab);
            var components = obj.GetComponent<ClearVfxComponents>();
            return new ClearVfx(components);

        }

        public ClearVfx(ClearVfxComponents components)
        {
            _components = components;
            _components.gameObject.SetActive(false);
        }

        [LuaMember(name:"Show")]
        public async UniTask ShowAsync()
        {
            if (_components == null)
            {
                return;
            }

            _components.gameObject.SetActive(true);
            var root = _components.ClearImage.transform;
            root.localScale = Vector3.zero;
            await root.DOScale(Vector3.one, 1f).SetEase(Ease.InCubic).AsyncWaitForCompletion().AsUniTask();

            // _components.gameObject.SetActive(false);
        }
    }
}
