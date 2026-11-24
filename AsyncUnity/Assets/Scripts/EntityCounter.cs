using System.Collections.Concurrent;

namespace AsyncUnity.Scenes.Game1
{
    public class EntityCounter
    {
        private ConcurrentDictionary<string, int> _counts = new();

        public string CalcNextName(string name)
        {
            if (!_counts.TryAdd(name, 1))
            {
                _counts[name]++;
            }

            return $"{name}{_counts[name]:00}";
        }
        
    }
}