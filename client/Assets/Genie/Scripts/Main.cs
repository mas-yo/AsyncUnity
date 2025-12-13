using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie
{
    public class Main : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartAsync().Forget();
        }
    
        private async UniTask StartAsync()
        {
            while (true)
            {
                await Title.StartAsync();
                await Game.StartAsync();
            }
        
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    
}
