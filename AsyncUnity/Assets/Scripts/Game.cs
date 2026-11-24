using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using AsyncUnity.Views;
using Lua;

namespace AsyncUnity.Scenes.Game1
{
    public static class Game
    {

        public static async UniTask SetupGameAsync(string stageName,
            EnvironmentAccessor environmentAccessor,
            Action setRetryFunc,
            Action setNextStageFunc,
            CancellationToken token)
        {
            var clearVfx = await ClearVfx.CreateAsync();

            var gameLuaState = Scripting.CreateLuaState();
            var gameEventListeners = new List<GameEventListener>();
            gameLuaState.SetupBasicFunctions(
                environmentAccessor,
                (l) => gameEventListeners.Add(l)
            );

            var startScriptPath = Scripting.CalcScriptFilePath(stageName, "Game");
            // var updateScriptPaths = Scripting.CalcUpdateScriptFilePaths(stageName, "Game").ToArray();
            // gameLuaState.SetupStartUpdateFunctions(startScriptPath, isGameStartedFunc, gameEventListeners, token);

            gameLuaState.Environment["clearVfx"] = clearVfx;
            gameLuaState.Environment["Retry"] = new LuaFunction(async (context, token) =>
            {
                setRetryFunc();
                return 0;
            });
            gameLuaState.Environment["NextStage"] = new LuaFunction(async (context, token) =>
            {
                setNextStageFunc();
                return 0;
            });

            // gameLuaState.State.Environment["ForEachEntities"] = new LuaFunction(async (context, token) =>
            // {
            //     var func = context.GetArgument<LuaFunction>(0);
            //
            //     foreach(var e in entityContainer.EnumerateEntities())
            //     {
            //         await context.State.CallAsync(func, new LuaValue[] { e }, token);
            //     }
            //     return 0;
            // });
            gameLuaState.Environment["IsOtherEntityIsOver"] = new LuaFunction(async (context, token) =>
            {
                var self = context.GetArgument<GameEntity>(0);
                var other = context.GetArgument<GameEntity>(1);

                return context.Return(EntityInteraction.IsOtherEntityIsOver(self, other));

            });

            await gameLuaState.StartScriptAsync(startScriptPath, gameEventListeners, token);

        }
        
    }
}