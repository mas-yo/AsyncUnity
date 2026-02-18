using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Genie.Views
{
    public class LoadingView
    {
        private readonly LoadingViewComponents _components;
        
        public LoadingView(LoadingViewComponents components)
        {
            _components = components;
        }

        public async UniTask<T> StartLoadingViewAsync<T>(CancellationToken token, Func<CancellationToken, LoadingView, UniTask<T>> callback)
        {
            GameObject obj = null;
            try
            {
                obj = Object.Instantiate(_components._loadingViewPrefab, _components.gameObject.transform);
                return await callback(token, this);
            }
            finally
            {
                Object.Destroy(obj);
            }
        }
    }
}