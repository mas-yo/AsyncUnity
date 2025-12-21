using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie.Components
{
    public class Mushroom : MonoBehaviour
    {
        private Animator _animator;
        private Collider _collider;

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

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "Player")
            {
                _collider.enabled = false;
                _animator.Play("Disappear");
                Destroy(gameObject, 1.0f);
            }
        }
    }
    
}
