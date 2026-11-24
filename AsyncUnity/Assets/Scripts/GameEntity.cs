using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using AsyncUnity.Scenes.Game1.MasterData;
using AsyncUnity.Views;
using Lua;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AsyncUnity.Scenes.Game1
{
    public interface IReadOnlyGameEntity
    {
        public GameEntityMaster Master { get; }
        public IReadOnlyGameEntityView View { get; }
        public string Name => View.Name;
    }
    [LuaObject]
    public partial class GameEntity : IReadOnlyGameEntity
    {
        public GameEntityMaster Master { get; init; }
        
        private readonly GameEntityView _view;
        public IReadOnlyGameEntityView View => _view;
        
        // private LuaState? _luaState = null;
        // private string _scriptFilePath;

        private readonly List<GameEvent> _gameEvents = new();
        private readonly List<GameEventListener> _eventListeners = new();
        
        [LuaMember("Events")]
        public IReadOnlyList<GameEvent> GameEvents => _gameEvents;

        public static async UniTask<GameEntity> CreateAsync(
            GameEntityMaster master,
            Vector3Int position,
            string name)
        {
            var view = await GameEntityView.CreateAsync(master.PrefabPath, position, name);
            var entity = new GameEntity(master, view);

            return entity;
        }

        private GameEntity(GameEntityMaster gameEntityMaster,
            GameEntityView gameEntityView)
        {
            Master = gameEntityMaster;
            _view = gameEntityView;
        }

        public void UpdateEvents(GameEntity otherEntity)
        {
            if (otherEntity.View.AnimationEvents is not null)
            {
                if (otherEntity.View.AnimationEvents.Any(e => e.Equals("AttackHit"))
                    && otherEntity.View.FrontPosition == this.View.RoundedPosition)
                {
                    _gameEvents.Add(new GameEvent(GameEventType.EntityAttack, otherEntity));
                }
            }
            if (EntityInteraction.IsOtherEntityIsOver(this, otherEntity))
            {
                _gameEvents.Add(new GameEvent(GameEventType.EntityOver, otherEntity));
            }


        }
        public void UpdatePhysics(IEnumerable<IReadOnlyGameEntity> otherEntities)
        {
            Vector3Int? moveTarget = null;
            bool exitsUnderEntity = false;
            float minDistance = float.MaxValue;

            foreach (var otherEntity in otherEntities)
            {
                if (this.View.Name == otherEntity.View.Name)
                {
                    continue;
                }

                if (EntityInteraction.IsOtherEntityUnder(this, otherEntity))
                {
                    exitsUnderEntity = true;
                }
                
                // selfEntity.UpdateEvents(otherEntity);
                if (this.Master.HasCollision
                    && otherEntity.Master.HasCollision
                    && this.View.IsMoving
                    && this.View.MovingProgress >= 0.5f
                    && this.View.CurrentMovingTarget == otherEntity.View.CurrentMovingTarget
                    && this.Master.Weight <= otherEntity.Master.Weight)
                {
                    var d = (this.View.Position - otherEntity.View.Position).sqrMagnitude;
                    if (d < minDistance)
                    {
                        d = minDistance;
                    }
                    moveTarget = otherEntity.View.IsMoving
                        ? this.View.CurrentMovingTarget + otherEntity.View.FrontPosition
                        : this.View.PreviousPosition;
                }

            }

            if (!exitsUnderEntity)
            {
                moveTarget = this.View.GridPosition + new Vector3Int(0, -1, 0);
            }
            
            if (moveTarget.HasValue)
            {
                if (this.View.IsMoving)
                {
                    this._view.ChangeMoveTarget(moveTarget.Value);
                }
                else
                {
                    this._view.MoveAsync(moveTarget.Value, CancellationToken.None).Forget();
                }
            }
        }

        public void ClearEvents()
        {
            _gameEvents.Clear();
        }



        public void SetupUserControl(Func<bool> isStartGameFunc, CancellationToken parentCancellationToken)
        {
            UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            {
                if (!isStartGameFunc())
                {
                    return;
                }
                var playerInput = PlayerControl.GetPlayerInput();
                


                switch (playerInput)
                {
                    case PlayerControl.Input.MoveNorth:
                        await MoveNorthAsync();
                        break;
                    case PlayerControl.Input.MoveEast:
                        await MoveEastAsync();
                        break;
                    case PlayerControl.Input.MoveSouth:
                        await MoveSouthAsync();
                        break;
                    case PlayerControl.Input.MoveWest:
                        await MoveWestAsync();
                        break;
                }
            }, parentCancellationToken).Forget();
        }

        public async UniTask StartAsync(EnvironmentAccessor environmentAccessor, string stageName, CancellationToken token)
        {
            if (Master.IsScriptControlled)
            {
                var luaState = Scripting.CreateLuaState();
                luaState.Environment["this"] = this;

                luaState.SetupBasicFunctions(environmentAccessor,
                    (l) => _eventListeners.Add(l));
            
                var scriptFilePath = Scripting.CalcScriptFilePath(stageName, View.Name);
                // var updateScriptPaths = Scripting.CalcUpdateScriptFilePaths(stageName, View.Name).ToArray();

                if (!File.Exists(scriptFilePath))
                {
                    scriptFilePath = Scripting.CalcScriptFilePath(stageName, Master.Code);
                }

                await luaState.StartScriptAsync(scriptFilePath, _eventListeners, token);
            }

            if (Master.IsPlayerControlled)
            {
                UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
                {
                    var playerInput = PlayerControl.GetPlayerInput();


                    switch (playerInput)
                    {
                        case PlayerControl.Input.MoveNorth:
                            await MoveNorthAsync();
                            break;
                        case PlayerControl.Input.MoveEast:
                            await MoveEastAsync();
                            break;
                        case PlayerControl.Input.MoveSouth:
                            await MoveSouthAsync();
                            break;
                        case PlayerControl.Input.MoveWest:
                            await MoveWestAsync();
                            break;
                    }
                }, token).Forget();
            }
        }

        [LuaMember]
        public string GetName()
        {
            return Master.Code;
        }

        [LuaMember(name: "DoAction")]
        public async UniTask DoActionAsync(string actionName)
        {
            await _view.PlayAnimationAndWaitAsync(actionName);
        }

        [LuaMember(name: "MoveNorth")]
        public async UniTask MoveNorthAsync()
        {
            await _view.MoveNorthAsync(CancellationToken.None);
        }
        [LuaMember(name: "MoveSouth")]
        public async UniTask MoveSouthAsync()
        {
            await _view.MoveSouthAsync(CancellationToken.None);
        }
        [LuaMember(name: "MoveEast")]
        public async UniTask MoveEastAsync()
        {
            await _view.MoveEastAsync(CancellationToken.None);
        }
        [LuaMember(name: "MoveWest")]
        public async UniTask MoveWestAsync()
        {
            await _view.MoveWestAsync(CancellationToken.None);
        }
        [LuaMember(name: "MoveForward")]
        public async UniTask MoveForwardAsync()
        {
            await _view.MoveForward(CancellationToken.None);
        }
        [LuaMember(name: "MoveBackward")]
        public async UniTask MoveBackwardAsync()
        {
            await _view.MoveBackward(CancellationToken.None);
        }
        [LuaMember(name: "TurnLeft")]
        public async UniTask TurnLeftAsync()
        {
            await _view.TurnLeft(CancellationToken.None);
        }
        [LuaMember(name: "TurnRight")]
        public async UniTask TurnRightAsync()
        {
            await _view.TurnRight(CancellationToken.None);
        }
        [LuaMember(name: "TurnBack")]
        public async UniTask TurnBackAsync()
        {
            await _view.TurnBack(CancellationToken.None);
        }

        [LuaMember]
        public LuaValue GetFrontPosition()
        {
            var table = new LuaTable();
            table["x"] = View.FrontPosition.x;
            table["y"] = View.FrontPosition.y;
            table["z"] = View.FrontPosition.z;
            return table;
        }

    }
}