using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Views
{
    public class GameCamera
    {
        private readonly Transform _transform;
        
        public static async UniTask<GameCamera> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("GameCamera/GameCamera");
            var obj = (GameObject)Object.Instantiate(prefab);
            
            return new GameCamera(obj.GetComponent<Transform>());
        }

        private GameCamera(Transform transform)
        {
            _transform = transform;
        }
        
        public void SetTarget(Vector3 target)
        {
            _transform.position = target + new Vector3(0, 16, -8);
            _transform.LookAt(target + new Vector3(0, 2, 0));
        }
        
    }
}