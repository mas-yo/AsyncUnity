using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using AsyncUnity.Scenes.Game1;
using AsyncUnity.Utils;
using UnityEngine.InputSystem;

namespace AsyncUnity.Scenes.Game1
{
    public static class PlayerControl
    {
        public enum Input
        {
            None,
            MoveNorth,
            MoveSouth,
            MoveEast,
            MoveWest,
            PutObject1,
            PutObject2,
            PutObject3,
        }

        // private static async UniTask<PlayerInput> WaitPlayerInput(CancellationToken token)
        // {
        //     return await UniTaskUtil.WaitAndCancel(token,
        //         (c) => KeyInputUtil.WaitKeyPressedAsync(c,
        //             Keyboard.current.escapeKey, 
        //             Keyboard.current.wKey,
        //             Keyboard.current.aKey,
        //             Keyboard.current.sKey,
        //             Keyboard.current.dKey),
        //         (c) => KeyInputUtil.WaitKeyReleasedAsync(c,
        //             Keyboard.current.digit1Key,
        //             Keyboard.current.digit2Key,
        //             Keyboard.current.digit3Key)
        //     ).ContinueWith(result => result switch
        //     {
        //         { winArgumentIndex: 0, result1: Key.W } => PlayerInput.MoveNorth,
        //         { winArgumentIndex: 0, result1: Key.A } => PlayerInput.MoveWest,
        //         { winArgumentIndex: 0, result1: Key.S } => PlayerInput.MoveSouth,
        //         { winArgumentIndex: 0, result1: Key.D } => PlayerInput.MoveEast,
        //         { winArgumentIndex: 1, result2: Key.Digit1 } => PlayerInput.PutObject1,
        //         { winArgumentIndex: 1, result2: Key.Digit2 } => PlayerInput.PutObject2,
        //         { winArgumentIndex: 1, result2: Key.Digit3 } => PlayerInput.PutObject3,
        //         _ => throw new InvalidOperationException("Unsupported player input.")
        //     });
        // }

        public static Input GetPlayerInput()
        {
            if (Keyboard.current.wKey.isPressed) return Input.MoveNorth;
            if (Keyboard.current.aKey.isPressed) return Input.MoveWest;
            if (Keyboard.current.sKey.isPressed) return Input.MoveSouth;
            if (Keyboard.current.dKey.isPressed) return Input.MoveEast;
            if (Keyboard.current.digit1Key.wasReleasedThisFrame) return Input.PutObject1;
            if (Keyboard.current.digit2Key.wasReleasedThisFrame) return Input.PutObject2;
            if (Keyboard.current.digit3Key.wasReleasedThisFrame) return Input.PutObject3;
            return Input.None;
        }
        
    }
}