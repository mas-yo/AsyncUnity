using System.Collections.Generic;
using System.Linq;
using Lua;

namespace AsyncUnity.Scenes.Game1
{
    public enum GameEventType
    {
        EntityOver,
        EntityAttack,
        EntityCollision,
    }
    
    [LuaObject]
    public partial class GameEvent
    {
        public GameEventType EventType { get; init; }
        
        [LuaMember("EventType")]
        public string EventTypeName => EventType.ToString();
        
        [LuaMember]
        public GameEntity ActorEntity { get; private set; }

        public GameEvent(GameEventType eventType, GameEntity actor)
        {
            EventType = eventType;
            ActorEntity = actor;
        }
    }

    public class GameEventListener
    {
        public GameEntity TargetEntity { get; init; }
        public GameEventType EventType { get; init; }
        public LuaFunction EventListener { get; init; }

        public IEnumerable<GameEvent> EnumerateTargetEvents()
        {
            return TargetEntity.GameEvents.Where(e => e.EventType == EventType);
        }
    }
}
