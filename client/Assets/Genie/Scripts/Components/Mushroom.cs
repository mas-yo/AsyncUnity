using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Components
{
    public class Mushroom : MonoBehaviour
    {
        private Animator _animator;
        private Collider _collider;
        private Queue<Collision> _collisionQueue = new Queue<Collision>();

        public async static UniTask<Mushroom> CreateAsync(string prefabPath, Vector3 position)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Instantiate(prefab);
            
            obj.transform.position = position;
            return obj.GetComponent<Mushroom>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
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
        private void OnCollisionEnter(Collision other)
        {
            _collisionQueue.Enqueue(other);
            _collider.enabled = false;
        }
    }
    
}
