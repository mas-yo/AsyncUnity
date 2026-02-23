using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsyncUnity.Logics.EcsCore
{
    public class ComponentContainer<T> : IEnumerable<Component<T>>
    {
        private readonly List<Component<T>> _components = new ();
        
        public Component<T> GetComponent(EntityId entityId)
        {
            return _components.Find(component => component.EntityId.Value == entityId.Value);
        }

        public void SetComponent(EntityId entityId, T data)
        {
            var index = _components.FindIndex(c => c.EntityId.Equals(entityId));
            _components[index] = new Component<T>(entityId, data);
        }
        public void AddComponent(EntityId entityId, T data)
        {
            _components.Add(new Component<T>(entityId, data));
        }

        public IEnumerator<Component<T>> GetEnumerator()
        {
            return _components.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void UpdateWith<TOther>(ComponentContainer<TOther> other, Func<TOther, T> func)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                var otherComponent = other.GetComponent(_components[i].EntityId);
                var nextData = func(otherComponent.Data);
                _components[i] = new Component<T>(_components[i].EntityId, nextData);
            }
        }
        public void UpdateWith<TOther1, TOther2>(ComponentContainer<TOther1> other1, ComponentContainer<TOther2> other2, Func<TOther1, TOther2, T> func)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                var otherComponent1 = other1.GetComponent(_components[i].EntityId);
                var otherComponent2 = other2.GetComponent(_components[i].EntityId);
                var nextData = func(otherComponent1.Data, otherComponent2.Data);
                _components[i] = new Component<T>(_components[i].EntityId, nextData);
            }
        }
    }
}