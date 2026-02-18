using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Lua;
using UnityEngine;

namespace AsyncUnity.Views
{
    [LuaObject]
    public partial class MushroomView
    {
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly Queue<Collision> _collisionQueue;

        public static async UniTask<MushroomView> CreateAsync(string prefabPath, Vector3 position, CancellationToken token)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);

            var collider = obj.GetComponent<Collider>();
            var collisionQueue = new Queue<Collision>();
            obj.GetAsyncCollisionEnterTrigger()
                .Subscribe(collision =>
                {
                    collider.enabled = false;
                    collisionQueue.Enqueue(collision);
                })
                .AddTo(token);
            
            obj.transform.position = position;
            return new MushroomView(
                obj.GetComponent<Animator>(),
                collider,
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
