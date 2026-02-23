namespace AsyncUnity.Logics.EcsCore
{
    public readonly struct Component<T>
    {
        public readonly EntityId EntityId;
        public readonly T Data;
        
        public Component(EntityId entityId, T data)
        {
            EntityId = entityId;
            Data = data;
        }
    }
}