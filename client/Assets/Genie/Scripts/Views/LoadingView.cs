using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Views
{
    public class LoadingView
    {
        private readonly GameObject _gameObject;
        
        public static async UniTask<LoadingView> CreateAsync(CancellationToken token)
        {
            var prefab = await Resources.LoadAsync<GameObject>("LoadingView/LoadingView");
            var obj = (GameObject)Object.Instantiate(prefab);
            return new LoadingView(obj);
        }

        private LoadingView(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public void Close()
        {
            Object.Destroy(_gameObject);
        }
        
    }
}