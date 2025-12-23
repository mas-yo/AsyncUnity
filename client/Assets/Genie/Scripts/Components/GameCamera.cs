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
        
        public void SetTarget(Vector3 target)
        {
            transform.position = target + new Vector3(0, 16, -8);
            transform.LookAt(target + new Vector3(0, 2, 0));
        }
        
    }
}