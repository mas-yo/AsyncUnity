using System;

namespace AsyncUnity.Logics.EcsCore
{
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public readonly int Value;

        public EntityId(int value)
        {
            Value = value;
        }
        
        public bool Equals(EntityId other)
        {
            return Value == other.Value;
        }
        
    };
}