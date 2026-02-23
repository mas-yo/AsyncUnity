using AsyncUnity.Logics.Components;
using AsyncUnity.Logics.EcsCore;
using AsyncUnity.Logics.Systems;

namespace AsyncUnity.Logics
{
    public class World
    {
        private ComponentContainer<Velocity> _velocities = new ComponentContainer<Velocity>();
        private ComponentContainer<MoveInput> _moveInputs = new ComponentContainer<MoveInput>();
        private ComponentContainer<Speed> _speeds = new ComponentContainer<Speed>();
        
        public void Update()
        {
            _velocities.UpdateWith(_moveInputs, _speeds, VelocitySystem.Calculate);
        }
    }
}