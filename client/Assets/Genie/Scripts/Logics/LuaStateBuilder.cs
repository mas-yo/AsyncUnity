using Cysharp.Threading.Tasks;
using Genie.MasterData;
using Genie.Scripts.MasterData;
using Genie.Views;
using Lua;
using UnityEngine;

namespace Genie.Logics
{
    public static class LuaStateBuilder
    {
        public static async UniTask<LuaState> BuildAsync(MemoryDatabase masterData)
        {
            var luaState = LuaState.Create();

            await luaState.DoFileAsync(Application.streamingAssetsPath + "/LuaScripts/Game.lua");
            
            luaState.Environment["PutPlayer"] = new LuaFunction(async (context, ct) =>
            {
                var playerCode = context.GetArgument<long>(0);
                var playerMaster = masterData.PlayerMasterTable.FindByCode(playerCode);
                var player = await PlayerView.CreateAsync(playerMaster.ModelPrefabPath, playerMaster.InitialPosition);
                return context.Return(player);
            });
            
            luaState.Environment["PutObject"] = new LuaFunction(async (context, ct) =>
            {
                var objectCode = context.GetArgument<long>(0);
                var objectMaster = masterData.ObjectMasterTable.FindByCode(objectCode);
                switch (objectMaster.ObjectType)
                {
                    case ObjectType.Mushroom:
                        var mushroomView = await MushroomView.CreateAsync(objectMaster.ModelPrefabPath, objectMaster.InitialPosition, ct);
                        return context.Return(mushroomView);
                    case ObjectType.Rock:
                        var objectView = await RockView.CreateAsync(objectMaster.ModelPrefabPath, objectMaster.InitialPosition);
                        return context.Return(objectView);
                    default:
                        throw new System.Exception("Unknown object type: " + objectMaster.ObjectType);
                }
            });

            return luaState;
        }
        
    }
}