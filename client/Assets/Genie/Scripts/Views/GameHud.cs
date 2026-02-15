using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Genie.Views
{
    public class GameHud
    {
        private Text _scoreText;
        
        public static async UniTask<GameHud> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("GameHud/GameHud");
            var obj = (GameObject)Object.Instantiate(prefab);
            
            return new GameHud(obj.transform.Find("Canvas/Score").GetComponent<Text>());
        }
        private GameHud(Text scoreText)
        {
            _scoreText = scoreText; // transform.Find("Canvas/Score").GetComponent<Text>();// GetComponentInChildren<Text>();
        }
        
        public void SetScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }

    }
    
}
