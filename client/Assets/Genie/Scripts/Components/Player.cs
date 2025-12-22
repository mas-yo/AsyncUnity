using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Components
{
    public class Player : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;
        
        public static async UniTask<Player> CreateAsync(string prefabPath, Vector3 initialPosition)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);
            obj.name = "Player";
            obj.transform.position = initialPosition;
            return obj.GetComponent<Player>();
        }
        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        void Update()
        {
        
        }

        public void PlayRunAnimation()
        {
            _animator.Play("Run_guard_AR");
        }
        public void PlayIdleAnimation()
        {
            _animator.Play("Idle_gunMiddle_AR");
        }

        public void Move(Vector3 moveAmount)
        {
            _rigidbody.MovePosition(transform.position + moveAmount);
        }

    }
}