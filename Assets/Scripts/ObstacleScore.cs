using UnityEngine;

namespace FlappyBird
{
    public class ObstacleScore : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 1;
        [SerializeField] private bool hasScored = false;
        
        private void OnTriggerExit2D(Collider2D other)
        {
            // 플레이어가 장애물을 통과한 경우 한 번만 점수 추가
            if (other.CompareTag("Player") && !hasScored)
            {
                // ScoreManager 참조
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddScore(scoreValue);
                    hasScored = true;
                }
                else
                {
                    Debug.LogWarning("ScoreManager not found! Cannot add score.");
                }
            }
        }
    }
}