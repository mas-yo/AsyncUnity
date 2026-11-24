using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Lua;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public static class LuaStateExtensions
    {
        public static void SetupBasicFunctions(this LuaState luaState,
            EnvironmentAccessor environmentAccessor,
            Action<GameEventListener> addEventListenerFunc)
        {
            luaState.Environment["FindEntityByName"] = new LuaFunction((context, token) =>
            {
                var name = context.GetArgument<string>(0);
                var entity = environmentAccessor.FindEntityByNameFunc(name);
                var count = context.Return(entity);
                return new(count);
            });

            luaState.Environment["GetEntitiesByCode"] = new LuaFunction((context, token) =>
            {
                var code = context.GetArgument<string>(0);
                var entities = environmentAccessor.GetEntitiesByCodeFunc(code);
                var array = new LuaTable(entities.Length, 0);
                for (int i = 0; i < entities.Length; i++)
                {
                    array[i + 1] = LuaValue.FromObject(entities[i]);
                }
                var count = context.Return(array);
                return new(count);
            });

            luaState.Environment["GetEntitiesOfPosition"] = new LuaFunction((context, token) =>
            {
                var position = context.GetArgument<LuaTable>(0);
                Vector3Int posVec3;

                if (position.TryGetValue("x", out var x)
                    && position.TryGetValue("y", out var y)
                    && position.TryGetValue("z", out var z))
                {
                    var entities =  environmentAccessor.GetEntitiesOfPositionFunc(new Vector3Int(x.Read<int>(), y.Read<int>(), z.Read<int>()));
                    var count = context.Return(entities.Select(e => new LuaValue(e)).ToArray());
                    return new(count);
                }
                
                throw new LuaRuntimeException(luaState, new Exception("Invalid position table. Expected keys: x, y, z."));
            });

            luaState.Environment["SetEventListener"]= new LuaFunction(async (context, token) =>
            {
                var entity = context.GetArgument<GameEntity>(0);
                var eventType = Enum.Parse<GameEventType>(context.GetArgument<string>(1));
                var func = context.GetArgument<LuaFunction>(2);
                
                addEventListenerFunc(new GameEventListener
                {
                    TargetEntity = entity,
                    EventType = eventType,
                    EventListener = func,
                });
                return 0;
            });

            // luaState.Environment["WaitEntityOver"]= new LuaFunction(async (context, token) =>
            // {
            //     var entity1 = context.GetArgument<GameEntity>(0);
            //     var entity2 = context.GetArgument<GameEntity>(1);
                
            //     await UniTask.WaitUntil(() => EntityInteraction.IsOtherEntityIsOver(entity1, entity2), cancellationToken: token);
                
            //     return 0;
            // });
            // luaState.Environment["WaitNoEntityOver"]= new LuaFunction(async (context, token) =>
            // {
            //     var entity1 = context.GetArgument<GameEntity>(0);
                
            //     await UniTask.WaitUntil(() => enumerateNeighborEntityFunc(entity1).All(other => !EntityInteraction.IsOtherEntityIsOver(entity1, other)), cancellationToken: token);
                
            //     return 0;
            // });

        }

        public static async UniTask StartScriptAsync(this LuaState luaState,
            string scriptPath,
            IEnumerable<GameEventListener> eventListeners,
            CancellationToken token)
        {
            if (!File.Exists(scriptPath))
            {
                return;
            }
            await luaState.DoScriptFileAsync(scriptPath, token);

            // var startCalled = false;
            UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            {
                // if (!startCalled)
                // {
                //     await UniTask.WaitUntil(() => isGameStartedFunc(), cancellationToken: token);
                //     await luaState.DoScriptFileAsync(scriptPath, token);
                //     startCalled = true;
                // }

                await luaState.CallEventListenersAsync(eventListeners, token);

            }, cancellationToken: token).Forget();

        }
        
        public static async UniTask CallEventListenersAsync(this LuaState luaState, IEnumerable<GameEventListener> listeners, CancellationToken token)
        {
            foreach (var listener in listeners)
            {
                foreach (var ev in listener.EnumerateTargetEvents().ToArray())
                {
                    var args = new LuaValue[] {
                        ev.ActorEntity
                    };
                    await luaState.CallAsync(listener.EventListener, args, token);
                }
            }
        }

        public static async UniTask DoScriptFileAsync(this LuaState luaState, string filePath, CancellationToken token)
        {
            try
            {
                await luaState.DoFileAsync(filePath, token);
            }
            catch (LuaCompileException ce)
            {
                Debug.LogWarning($"LuaCompileException: {ce.Message}");
            }
            catch (LuaRuntimeException re)
            {
                Debug.LogWarning($"LuaRuntimeException: {re.Message} {re.LuaTraceback}");
            }
            catch (LuaCanceledException ce)
            {
                // Debug.LogWarning($"{Name}: DoScriptFileAsync canceled: {filePath}");
            }

        }
    }
}