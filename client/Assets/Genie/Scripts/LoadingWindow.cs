using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie
{
    public class LoadingWindow
    {
        private readonly GameObject _gameObject;
        
        public static async UniTask<LoadingWindow> StartAsync(CancellationToken token)
        {
            var prefab = await Resources.LoadAsync<GameObject>("LoadingWindow/LoadingWindow");
            var obj = (GameObject)Object.Instantiate(prefab);
            return new LoadingWindow(obj);
        }

        private LoadingWindow(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public void Close()
        {
            Object.Destroy(_gameObject);
        }
        
    }
}