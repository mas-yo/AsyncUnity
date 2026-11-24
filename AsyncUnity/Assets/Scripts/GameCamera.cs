using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AsyncUnity.Views
{
    public class GameCamera
    {
        private readonly GameCameraComponents _components;
        private Func<Vector3>? _getTargetPositionFunc;
        
        public static async UniTask<GameCamera> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("GameCamera/GameCamera");
            var obj = (GameObject)Object.Instantiate(prefab);
            var components = obj.GetComponent<GameCameraComponents>();
            
            return new GameCamera(components);
        }

        private GameCamera(GameCameraComponents components)
        {
            _components = components;
        }

        public void SetTargetPositionFunc(Func<Vector3> getTargetPositionFunc)
        {
            _getTargetPositionFunc = getTargetPositionFunc;
        }

        public void UpdateTargetPosition()
        {
            if (_getTargetPositionFunc is null)
            {
                return;
            }
            var target = _getTargetPositionFunc();
            var t = new Vector3(target.x, 0f, target.z);
            _components.transform.position = t + new Vector3(0, 14, -3f);
            _components.transform.LookAt(t + new Vector3(0, 0, 5.5f));
        }
        
        public void SetTarget(Vector3 target)
        {
            var t = new Vector3(target.x, 0f, target.z);
            _components.transform.position = t + new Vector3(0, 14, -2f);
            _components.transform.LookAt(t + new Vector3(0, 0, 6.5f));
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            return _components.Camera.WorldToScreenPoint(position);
        }
        
    }
}