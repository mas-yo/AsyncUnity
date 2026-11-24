using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using AsyncUnity.Scenes.Game1;
using System.Threading;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Object = UnityEngine.Object;

namespace AsyncUnity.Views
{
    public interface IReadOnlyGameEntityView
    {
        public string Name { get; }
        public Vector3Int PreviousGridPosition { get; }
        public Vector3Int GridPosition { get; }
        public Vector3Int GridDirection { get; }
        public Vector3Int FrontPosition { get; }
        public Vector3Int MovingDirection { get; }
        
        public Vector3 Position { get; }
        public Vector3Int RoundedPosition { get; }
        public bool IsMoving { get; }
        public float MovingProgress { get; }
        public Vector3Int CurrentMovingTarget { get; }
        public Vector3Int PreviousPosition { get; }
        public Quaternion Rotation { get; }

        public IReadOnlyList<string>? AnimationEvents { get; }
        
    }
    public class GameEntityView : IReadOnlyGameEntityView
    {
        private const float MoveDuration = 0.2f;

        public string Name { get; set; }
        public Vector3Int PreviousGridPosition { get; private set; }
        public Vector3Int GridPosition { get; private set; }

        public Vector3Int GridDirection
        {
            get
            {
                var f = _components.transform.forward;
                return new Vector3Int(Mathf.RoundToInt(f.x), Mathf.RoundToInt(f.y), Mathf.RoundToInt(f.z));
            }
        }
         
        public Vector3Int FrontPosition => GridPosition + GridDirection;
        public Vector3Int MovingDirection => CurrentMovingTarget - GridPosition;
        public Vector3 Position => _components.Transform.position;
        public Vector3Int RoundedPosition => new Vector3Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y), Mathf.RoundToInt(Position.z));
        public bool IsMoving => _currentMoveTween != null && _currentMoveTween.IsActive() && _currentMoveTween.IsPlaying();
        public Vector3Int CurrentMovingTarget => GridMap.WorldPositionToGrid(_currentMoveTween?.endValue ?? Position);
        public Vector3Int PreviousPosition => GridMap.WorldPositionToGrid(_currentMoveTween?.startValue ?? Position);

        public IReadOnlyList<string>? AnimationEvents => _components.AnimationEventListener?.Events;
        
        private TweenerCore<Vector3, Vector3, VectorOptions>? _currentMoveTween;
        private TweenerCore<Quaternion, Quaternion, NoOptions>? _currentRotateTween;
        public float MovingProgress => _currentMoveTween?.ElapsedPercentage() ?? 0f;

        public Quaternion Rotation => _components.Transform.rotation;
        
        private GameEntityViewComponents _components;
        // private Queue<string> _animationEvents = new Queue<string>();

        private void UpdateGridPosition()
        {
            PreviousGridPosition = GridPosition;
            GridPosition = GridMap.WorldPositionToGrid(_components.Transform.position);   
        }
        
        public static async UniTask<GameEntityView> CreateAsync(string prefabPath, Vector3Int position, string name)
        {
            var prefab = await Resources.LoadAsync<GameObject>(prefabPath);
            var obj = (GameObject)Object.Instantiate(prefab);
            obj.name = name;
            obj.transform.position = GridMap.GridPositionToWorld(position);
            var components = obj.GetComponent<GameEntityViewComponents>();
            
            return new GameEntityView(components, name);
        }

        private GameEntityView(GameEntityViewComponents components, string name)
        {
            Name = name;
            _components = components;
            UpdateGridPosition();
        }

        // public void Dispose()
        // {
        //     if (_components.AnimationEventListener != null)
        //     {
        //         _components.AnimationEventListener.OnEvent = null;
        //     }
        //     DOTween.Kill(_components.transform);
        // }
        //
        // public void SetOnAnimationEvent(Action<string> onAnimationEvent)
        // {
        //     if (_components.AnimationEventListener != null)
        //     {
        //         _components.AnimationEventListener.OnEvent = onAnimationEvent;
        //     }
        // }

        // public string[] DequeAnimationEvents()
        // {
        //     var events = _animationEvents.ToArray();
        //     _animationEvents.Clear();
        //     return events;
        // }

        public void PlayAnimation(string name)
        {
            _components.Animator.Play(name, -1, 0f);
        }

        public async UniTask PlayAnimationAndWaitAsync(string name)
        {
            PlayAnimation(name);
            await WaitAnimationEndAsync(name);
        }

        public async UniTask WaitAnimationEndAsync(string name)
        {
            await UniTask.WaitUntil(() => 
            {
                var stateInfo = _components.Animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName(name) && stateInfo.normalizedTime >= 1.0f;
            });
        }

        public void ChangeMoveTarget(Vector3Int pos)
        {
            ChangeMoveTarget(GridMap.GridPositionToWorld(pos));
        }
        public void ChangeMoveTarget(Vector3 pos)
        {
            if (_currentMoveTween != null)
            {
                _currentMoveTween.ChangeEndValue(pos, snapStartValue: true);
            }
        }

        public async UniTask MoveAsync(Vector3Int pos, CancellationToken token)
        {
            await MoveAsync(GridMap.GridPositionToWorld(pos), token);
        }
        public async UniTask MoveAsync(Vector3 nextPos, CancellationToken token)
        {
            if (_currentMoveTween != null)
            {
                _currentMoveTween.Kill();
            }
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _components.GetCancellationTokenOnDestroy());
            _currentMoveTween = _components.Transform.DOMove(nextPos, MoveDuration).SetEase(Ease.Linear);
            await _currentMoveTween.ToUniTask(TweenCancelBehaviour.Kill, linkedTokenSource.Token);
            UpdateGridPosition();
        }
        public async UniTask RotateAsync(Quaternion nextRot, CancellationToken token)
        {
            if (_currentRotateTween != null)
            {
                _currentRotateTween.Kill();
            }
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _components.GetCancellationTokenOnDestroy());
            _currentRotateTween = _components.Transform.DORotateQuaternion(nextRot, MoveDuration / 2f);
            await _currentRotateTween.ToUniTask(TweenCancelBehaviour.Kill, linkedTokenSource.Token);
        }
        // public async UniTask MoveAsync(Vector3 nextPos, Quaternion nextRot, CancellationToken token)
        // {
        //     if (_currentMoveTween != null)
        //     {
        //         _currentMoveTween.Kill();
        //     }
        //     if (_currentRotateTween != null)
        //     {
        //         _currentRotateTween.Kill();
        //     }
        //     _currentMoveTween = _components.Transform.DOMove(nextPos, MoveDuration).SetEase(Ease.Linear);
        //     _currentRotateTween = _components.Transform.DORotateQuaternion(nextRot, MoveDuration / 2f);
        //     await UniTask.WhenAll(
        //         _currentMoveTween.ToUniTask(TweenCancelBehaviour.Kill, token),
        //         _currentRotateTween.ToUniTask(TweenCancelBehaviour.Kill, token)
        //     );
        //     UpdateGridPosition();
        // }
        public async UniTask MoveNorthAsync(CancellationToken token)
        {
            var nextPos = _components.transform.position + new Vector3(0, 0, 1);
            var nextRot = Quaternion.Euler(new Vector3(0, 0, 0));

            await UniTask.WhenAll(MoveAsync(nextPos, token), RotateAsync(nextRot, token));
        }
        public async UniTask MoveSouthAsync(CancellationToken token)
        {
            var nextPos = _components.transform.position + new Vector3(0, 0, -1);
            var nextRot = Quaternion.Euler(new Vector3(0, 180, 0));
            await UniTask.WhenAll(MoveAsync(nextPos, token), RotateAsync(nextRot, token));
        }
        public async UniTask MoveEastAsync(CancellationToken token)
        {
            var nextPos = _components.transform.position + new Vector3(1, 0, 0);
            var nextRot = Quaternion.Euler(new Vector3(0, 90, 0));
            await UniTask.WhenAll(MoveAsync(nextPos, token), RotateAsync(nextRot, token));
        }
        public async UniTask MoveWestAsync(CancellationToken token)
        {
            var nextPos = _components.transform.position + new Vector3(-1, 0, 0);
            var nextRot = Quaternion.Euler(new Vector3(0, -90, 0));
            await UniTask.WhenAll(MoveAsync(nextPos, token), RotateAsync(nextRot, token));
        }

        public async UniTask MoveForward(CancellationToken token)
        {
            var nextPos = Position + Rotation * new Vector3(0, 0, 1);
            await MoveAsync(nextPos, token);
        }
        public async UniTask MoveBackward(CancellationToken token)
        {
            var nextPos = Position + Rotation * new Vector3(0, 0, -1);
            await MoveAsync(nextPos, token);
        }
        public async UniTask TurnLeft(CancellationToken token)
        {
            var nextRot = Rotation * Quaternion.Euler(new Vector3(0, -90, 0));
            await RotateAsync(nextRot, token);
        }
        public async UniTask TurnRight(CancellationToken token)
        {
            var nextRot = Rotation * Quaternion.Euler(new Vector3(0, 90, 0));
            await RotateAsync(nextRot, token);
        }
        public async UniTask TurnBack(CancellationToken token)
        {
            var nextRot = Rotation * Quaternion.Euler(new Vector3(0, 180, 0));
            await RotateAsync(nextRot, token);
        }

        // public async UniTask MoveTo(Vector3Int position, CancellationToken token)
        // {
        //     PreviousPosition = GridPosition;
        //     var nextPos = GridMap.GridPositionToWorld(position);
        //     await _components.Transform.DOMove(nextPos, MoveDuration).SetEase(Ease.Linear).ToUniTask(TweenCancelBehaviour.Kill, token);
        //     UpdateGridPosition();
        // }
        
        // public async UniTask MoveToPreviousPositionAsync(CancellationToken token)
        // {
        //     var nextPos = GridMap.GridPositionToWorld(PreviousPosition);
        //     PreviousPosition = GridPosition;
        //     await _components.Transform.DOMove(nextPos, MoveDuration).SetEase(Ease.Linear).ToUniTask(TweenCancelBehaviour.Kill, token);
        //     UpdateGridPosition();
        // }
        
        private async UniTask TurnAsync(Vector3 eulerAngles)
        {
            var nextRot = Quaternion.Euler(eulerAngles);
            await _components.Transform
                .DORotateQuaternion(nextRot, MoveDuration / 2f)
                .AsyncWaitForCompletion()
                .AsUniTask();
            UpdateGridPosition();
        }

        public async UniTask TurnNorthAsync()
        {
            await TurnAsync(new Vector3(0, 0, 0));
        }

        public async UniTask TurnSouthAsync()
        {
            await TurnAsync(new Vector3(0, 180, 0));
        }

        public async UniTask TurnEastAsync()
        {
            await TurnAsync(new Vector3(0, 90, 0));
        }

        public async UniTask TurnWestAsync()
        {
            await TurnAsync(new Vector3(0, -90, 0));
        }
    }
}