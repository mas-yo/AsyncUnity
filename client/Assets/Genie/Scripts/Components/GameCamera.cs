using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Components
{
    public class GameCamera : MonoBehaviour
    {
        public static async UniTask<GameCamera> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("GameCamera/GameCamera");
            var obj = (GameObject)Instantiate(prefab);
            return obj.GetComponent<GameCamera>();
        }
        
        public void SetTarget(Transform target)
        {
            transform.position = target.position + new Vector3(0, 16, -8);
            transform.LookAt(target.position + new Vector3(0, 2, 0));
        }
        
    }
}