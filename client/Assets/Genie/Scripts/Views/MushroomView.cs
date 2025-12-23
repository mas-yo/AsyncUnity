using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Genie.Components
{
    public class MushroomView
    {
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly Queue<Collision> _collisionQueue;

        public static async UniTask<MushroomView> CreateAsync(string prefabPath, Vector3 position, CancellationToken token)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);
            
            var collisionQueue = new Queue<Collision>();
            obj.GetAsyncCollisionEnterTrigger()
                .Subscribe(collision => collisionQueue.Enqueue(collision))
                .AddTo(token);
            
            obj.transform.position = position;
            return new MushroomView(
                obj.GetComponent<Animator>(),
                obj.GetComponent<Collider>(),
                collisionQueue
            );
        }
        private MushroomView(Animator animator, Collider collider, Queue<Collision> collisionQueue)
        {
            _animator = animator;
            _collider = collider;
            _collisionQueue = collisionQueue;
        }
        
        public IEnumerable<Collision> DequeueCollisions()
        {
            while (_collisionQueue.Count > 0)
            {
                yield return _collisionQueue.Dequeue();
            }
        }

        public void PlayDisappearAnimation()
        {
            _animator.Play("Disappear");
        }
        // private void OnCollisionEnter(Collision other)
        // {
        //     _collisionQueue.Enqueue(other);
        //     _collider.enabled = false;
        // }
    }
    
}
