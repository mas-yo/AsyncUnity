using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AsyncUnity.Views
{
    public class GameHud
    {
        private GameHudComponents _components;
        
        public GameHud(GameHudComponents components)
        {
            _components = components;
        }
        
        public void SetScore(int score)
        {
            _components.ScoreText.text = $"Score: {score}";
        }

    }
    
}
