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
            
            var playerParameters = new Game.PlayerParameters()
            {
                modelPrefabPath = "SciFiWarriorPBRHPPolyart/Prefabs/PBRCharacter",
                InitialPosition = new Vector3(0, 5, 0),
                MoveSpeed = 0.1f,
            };
            var mapParameters = new Game.MapParameters()
            {
                GroundPrefabPath = "SimpleNaturePack/Prefabs/Ground_01",
            };
            while (true)
            {
                await Title.StartAsync(token);
                await Game.StartAsync(mapParameters, playerParameters, token);
            }
        
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    
}
