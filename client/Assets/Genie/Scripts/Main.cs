using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Genie
{
    public class Main : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            MainLoopAsync().Forget();
        }
    
        private async UniTask MainLoopAsync()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            while (true)
            {
                await Title.StartAsync(token);
                await Game.StartAsync(token);
            }
        
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    
}
