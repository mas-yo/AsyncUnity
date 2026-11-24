using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using AsyncUnity.Scenes.Game1;
using AsyncUnity.Scenes.Game1.MasterData;
using AsyncUnity.Scenes.Game1.MasterData.Tables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Lua;

namespace AsyncUnity.Scenes
{
    public static class Game1Scene
    {

        public enum ResultNext
        {
            NoStage,
            NextStage,
            Retry,
        }
        public struct Result
        {
            public ResultNext ResultNext;
        }

        private static GridMap CreateGridMap(GridMapMasterTable mapMaster, long mapCode)
        {
            var map = mapMaster.All.Where(x => x.Code == mapCode);
            return new GridMap(map.Select(g => g.Columns).Reverse().ToArray());
        }

        private static async UniTask<GameCamera> CreateGameCameraAsync(GridMap gridMap)
        {
            var minX = gridMap.EnumerateCells().Min(c => c.x);
            var maxX = gridMap.EnumerateCells().Max(c => c.x);
            var minY = gridMap.EnumerateCells().Min(c => c.y);
            var maxY = gridMap.EnumerateCells().Max(c => c.y);
            var camera = await GameCamera.CreateAsync();
            camera.SetTarget(new Vector3((minX + maxX) / 2f, 0, (minY + maxY) / 2f));
            return camera;
        }
        
        private static async UniTask<NameLabelView> CreateNameLabelAsync(IReadOnlyGameEntityView entityView, string stageName, GameCamera camera, CancellationToken token)
        {
            var nameLabelView = await NameLabelView.CreateAsync(() =>
            {
                return camera.WorldToScreenPoint(entityView.Position) + new Vector3(0, 36f, 0);
            });
            nameLabelView.SetNameLabel(entityView.Name);
            // UniTaskAsyncEnumerable.EveryUpdate().ForEachAsync(_ => {
            //     nameLabelView.SetPosition(camera.WorldToScreenPoint(entityView.Position) + new Vector3(0, 36f, 0));
            // }, token).Forget();

            // UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            // {
            //     await nameLabelView.OnClilckButtonAsync(token);
            //     var scriptFilePath = Scripting.CalcScriptFilePath(stageName, entityView.Name);
            //     await Scripting.OpenScriptEditorAsync(scriptFilePath, token);
            // }, token).Forget();

            return nameLabelView;
        }
        
        private static async UniTask CreateEntityInfoViewAsync(
            GameEntity player, 
            IReadOnlyList<GameEntity> entities,
            string stageName,
            CancellationToken token)
        {
            var entityInfoView = await EntityInfoView.CreateAsync();
            
            UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            {
                await entityInfoView.OnClickEditScriptButtonAsync(token);
                var scriptFilePath = entityInfoView.ScriptFilePath;
                await Scripting.OpenScriptEditorAsync(scriptFilePath, token);
            }, token).Forget();
            
            UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            {
                entityInfoView.Hide();

                GameEntity? lookedEntity = null;
                await UniTask.WaitUntil(() =>
                {
                    foreach (var e in entities)
                    {
                        if (e.Master.IsScriptControlled && EntityInteraction.IsLookingAt(player, e))
                        {
                            lookedEntity = e;
                            break;
                        }
                    }
                    return lookedEntity is not null;
                }, cancellationToken: token);
                
                entityInfoView.SetEntityName(lookedEntity!.View.Name);
                entityInfoView.ScriptFilePath = Scripting.CalcScriptFilePath(stageName, lookedEntity.View.Name);

                entityInfoView.Show();

                await UniTask.WaitUntil(() =>
                {
                    return !entities.Any(e => e.Master.IsScriptControlled && EntityInteraction.IsLookingAt(player, e));
                }, cancellationToken: token);

            }, token).Forget();
            
        }

        public static async UniTask<Result> StartAsync(
            MemoryDatabase masterData,
            long mapCode,
            CancellationToken token
        )
        {
            if (masterData.GridMapMasterTable.All.Any(x => x.Code == mapCode) == false)
            {
                return new Result()
                {
                    ResultNext = ResultNext.NoStage
                };
            }
            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            
            #region Initialize

            var stageName = mapCode.ToString();
            
            await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

            var keyGuideView = await KeyGuideView.CreateAsync();
            // var clearVfx = await ClearVfx.CreateAsync();
            var stageStartVfx = await StageStartVfx.CreateAsync((int)mapCode);
            
            var gridMap = CreateGridMap(masterData.GridMapMasterTable, mapCode);
            var camera = await CreateGameCameraAsync(gridMap);

            var entityCounter = new EntityCounter();
            // var positionIndexedEntities = new Dictionary<Vector3Int, HashSet<GameEntity>>();
            var entityContainer = new EntityContainer();
            var nameLabelViews = new List<NameLabelView>();

            var entityMasterGround = masterData.GameEntityMasterTable.FindByCode("Ground");

            List<GameEntity> players = new();
            var environmentAccessor = new EnvironmentAccessor()
            {
                FindEntityByNameFunc = (name) => entityContainer.EnumerateEntities().First(e => e.View.Name == name),
                GetEntitiesByCodeFunc = (code) => entityContainer.EnumerateEntities().Where(e => e.Master.Code == code).ToArray(),
                GetEntitiesOfPositionFunc = (pos) =>
                    entityContainer.EnumerateEntities().Where(e => e.View.GridPosition == pos).ToArray(),
            };
            
            foreach (var cell in gridMap.EnumerateCells())
            {
                entityContainer.AddNewEntity(await GameEntity.CreateAsync(
                    entityMasterGround,
                    new Vector3Int(cell.x, entityMasterGround.Altitude, cell.y),
                    entityCounter.CalcNextName(entityMasterGround.Code)));
                    
                if (!string.IsNullOrEmpty(cell.entityCode))
                {
                    var master = masterData.GameEntityMasterTable.FindByCode(cell.entityCode);
                    var entity = await GameEntity.CreateAsync(
                        master,
                        new Vector3Int(cell.x, master.Altitude, cell.y),
                        entityCounter.CalcNextName(master.Code));

                    if (master.ShowNameLabel)
                    {
                        nameLabelViews.Add(await CreateNameLabelAsync(entity.View, stageName, camera, tokenSource.Token));
                    }
                    if (master.IsPlayerControlled)
                    {
                        players.Add(entity);
                    }
                    entityContainer.AddNewEntity(entity);
                }
                
            }

            if (players.Count > 0)
            {
                // await CreateEntityInfoViewAsync(players.First(), gameEntities, stageName, tokenSource.Token);

                camera.SetTargetPositionFunc(() =>
                {
                    var position = Vector3.zero;
                    foreach (var p in players)
                    {
                        position += p.View.Position;
                    }
                    return position / players.Count;
                });
            }

            var retry = false;
            var nextStage = false;
            
            var gameResult = new Result
            {
                ResultNext = ResultNext.NextStage
            };

            #endregion

            
            UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PostLateUpdate).ForEachAwaitAsync(async _ =>
            {
                camera.UpdateTargetPosition();
                foreach (var n in nameLabelViews)
                {
                    n.UpdatePosition();
                }
            }, cancellationToken:tokenSource.Token).Forget();


            bool openScriptFolder = false;
            UniTaskAsyncEnumerable.EveryUpdate().ForEachAwaitAsync(async _ =>
            {
                await keyGuideView.OnEditScriptButtonClickedAsync();
                openScriptFolder = true;
            }, tokenSource.Token).Forget();

            await Game.SetupGameAsync(
                stageName,
                environmentAccessor,
                () => retry = true,
                () => nextStage = true,
                tokenSource.Token);

            await stageStartVfx.ShowAsync();

            foreach (var e in entityContainer.EnumerateEntities())
            {
                await e.StartAsync(environmentAccessor, stageName, token);
            }
            
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                foreach (var e in entityContainer.EnumerateEntities())
                {
                    e.ClearEvents();
                    foreach (var otherEntity in entityContainer.EnumerateNeighborEntities(e.View.GridPosition, 2))
                    {
                        e.UpdateEvents(otherEntity);
                    }
                }
                foreach (var e in entityContainer.EnumerateEntities())
                { 
                    e.UpdatePhysics(entityContainer.EnumerateNeighborEntities(e.View.GridPosition, 2));
                }


                foreach (var e in entityContainer.EnumerateEntities())
                {
                    if (e.View.PreviousGridPosition != e.View.GridPosition)
                    {
                        entityContainer.MoveEntityPosition(e.View.PreviousGridPosition, e.View.GridPosition, e);
                    }
                }

                if (Keyboard.current.escapeKey.isPressed || nextStage)
                {
                    break;
                }
                if (Keyboard.current.rKey.isPressed || retry)
                {
                    gameResult.ResultNext = ResultNext.Retry;
                    break;
                }
                if (Keyboard.current.eKey.isPressed || openScriptFolder)
                {
                    Scripting.OpenScriptDirectory(stageName);
                    openScriptFolder = false;
                }

            }
            
            tokenSource.Cancel();

            return gameResult;
        }
        
    }
}