using AsyncUnity.Logics.Components;
using UnityEngine;

namespace AsyncUnity.Logics.Systems
{
    public static class VelocitySystem
    {
        public static Velocity Calculate(MoveInput moveInput, Speed speed)
        {
            return new Velocity { Value = moveInput.Value * speed.Value };
        }
    }
}