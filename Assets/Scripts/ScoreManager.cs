using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private int currentScore = 0;
        
        private static ScoreManager instance;
        
        public static ScoreManager Instance
        {
            get { return instance; }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            ResetScore();
        }
        
        public void AddScore(int points)
        {
            currentScore += points;
            UpdateScoreDisplay();
        }
        
        public void ResetScore()
        {
            currentScore = 0;
            UpdateScoreDisplay();
        }
        
        private void UpdateScoreDisplay()
        {
            if (scoreText != null)
            {
                scoreText.text = currentScore.ToString();
            }
        }
        
        public int GetScore()
        {
            return currentScore;
        }
    }
}