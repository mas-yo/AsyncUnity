using System.Threading;
using Cysharp.Threading.Tasks;
using Genie.MasterData;
using UnityEngine;

namespace Genie
{
    public static class LoadMasterData
    {
        public static async UniTask<MasterData.MasterData> DoAsync(CancellationToken token)
        {
            var masterData = new MasterData.MasterData()
            {
                Stages = new StageMaster[]
                {
                    new StageMaster()
                        { Code = 1, Name = "Stage 1", GroundPrefabPath = "SimpleNaturePack/Prefabs/Ground_01" },
                },
                Characters = new CharacterMaster[]
                {
                    new CharacterMaster() 
                    {
                        Code = 1, 
                        Name = "Hero", 
                        ModelPrefabPath = "SciFiWarriorPBRHPPolyart/Prefabs/PBRCharacter", 
                        InitialPosition = new Vector3(0f, 1f, 0f),
                        MoveSpeed = 0.1f 
                    },
                },
                Items = new ItemMaster[]
                {
                    new ItemMaster() { Code = 1, PrefabPath = "SimpleNaturePack/Prefabs/Mushroom_02", Position = new Vector3(1f, 1f, 1f)},
                    new ItemMaster() { Code = 2, PrefabPath = "SimpleNaturePack/Prefabs/Mushroom_02", Position = new Vector3(2f, 1f, 0f)},
                    new ItemMaster() { Code = 3, PrefabPath = "SimpleNaturePack/Prefabs/Mushroom_02", Position = new Vector3(1f, 1f, 2f)},
                    
                }
            };
            // Simulate loading master data
            await UniTask.Delay(1000, cancellationToken: token);

            return masterData;
        }        
    }
}