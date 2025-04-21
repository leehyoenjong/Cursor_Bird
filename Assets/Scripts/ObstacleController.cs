using UnityEngine;

namespace FlappyBird
{
    public class ObstacleController : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private float gapSize = 4f;         // 상하 장애물 사이의 간격
        [SerializeField] private float minHeight = -2f;      // 장애물 높이 최소값
        [SerializeField] private float maxHeight = 2f;       // 장애물 높이 최대값
        
        [Header("References")]
        [SerializeField] private Transform topObstacle;      // 상단 장애물
        [SerializeField] private Transform bottomObstacle;   // 하단 장애물
        [SerializeField] private BoxCollider2D topCollider;  // 상단 충돌 박스
        [SerializeField] private BoxCollider2D bottomCollider; // 하단 충돌 박스

        public void Initialize()
        {
            // 장애물 높이를 랜덤하게 설정
            float height = Random.Range(minHeight, maxHeight);
            
            // 충돌 영역 설정
            if (topCollider != null && bottomCollider != null)
            {
                // 스프라이트 렌더러에서 실제 크기 가져오기
                SpriteRenderer topRenderer = topObstacle.GetComponent<SpriteRenderer>();
                SpriteRenderer bottomRenderer = bottomObstacle.GetComponent<SpriteRenderer>();
                
                if (topRenderer != null && bottomRenderer != null)
                {
                    float obstacleHeight = topRenderer.sprite.bounds.size.y;
                    
                    // 상단 장애물 위치 설정
                    topObstacle.localPosition = new Vector3(0, height + gapSize/2, 0);
                    
                    // 하단 장애물 위치 설정
                    bottomObstacle.localPosition = new Vector3(0, height - gapSize/2, 0);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Player가 장애물을 통과했을 때 점수 추가 로직
            if (other.CompareTag("Player"))
            {
                // 점수 관리자가 있으면 점수 추가
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddScore(1);
                }
            }
        }
    }
}