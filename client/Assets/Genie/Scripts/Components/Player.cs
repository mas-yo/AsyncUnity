using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Genie.Components
{
    public class Player
    {
        private readonly Transform _transform;
        private readonly Animator _animator;
        private readonly Rigidbody _rigidbody;
        
        public Vector3 Position => _transform.position;
        
        
        public static async UniTask<Player> CreateAsync(string prefabPath, Vector3 initialPosition)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);
            obj.name = "Player";
            obj.transform.position = initialPosition;
            var transform = obj.GetComponent<Transform>();
            var animator = obj.GetComponent<Animator>();
            var rigidbody = obj.GetComponent<Rigidbody>();
            
            return new Player(transform, animator, rigidbody);
        }

        public Player(Transform transform, Animator animator, Rigidbody rigidbody)
        {
            _transform = transform;
            _animator = animator;
            _rigidbody = rigidbody;
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
            _rigidbody.MovePosition(_transform.position + moveAmount);
        }
        
        public bool IsSame(GameObject obj)
        {
            return _transform.gameObject == obj;
        }

    }
}