using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace Genie.Components
{
    public class GameHud : MonoBehaviour
    {
        private Text _scoreText;
        
        public static async UniTask<GameHud> CreateAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("GameHud/GameHud");
            var obj = (GameObject)Instantiate(prefab);  
            return obj.GetComponent<GameHud>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _scoreText = transform.Find("Canvas/Score").GetComponent<Text>();// GetComponentInChildren<Text>();
        
        }
        
        public void SetScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
    
}
