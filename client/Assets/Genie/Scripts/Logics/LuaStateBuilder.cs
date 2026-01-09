using Cysharp.Threading.Tasks;
using Genie.MasterData;
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
            
            return luaState;
        }
        
    }
}