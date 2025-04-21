using UnityEngine;

namespace FlappyBird
{
    public class BackgroundController : MonoBehaviour
    {
        [Header("Background Settings")]
        [SerializeField] private SpriteRenderer[] backgroundRenderers;
        
        private Transform playerTransform;
        private float[] backgroundWidths;
        private int lastBackgroundIndex = 0;
        private float lastReposition = 0f;

        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null)
            {
                Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
                return;
            }

            // 각 배경의 실제 너비 계산
            backgroundWidths = new float[backgroundRenderers.Length];
            for (int i = 0; i < backgroundRenderers.Length; i++)
            {
                if (backgroundRenderers[i] != null && backgroundRenderers[i].sprite != null)
                {
                    // 스프라이트의 실제 월드 크기 계산
                    backgroundWidths[i] = backgroundRenderers[i].sprite.bounds.size.x * backgroundRenderers[i].transform.localScale.x;
                    
                    // 초기 위치 설정
                    float previousWidth = 0;
                    for (int j = 0; j < i; j++)
                    {
                        previousWidth += backgroundWidths[j];
                    }
                    backgroundRenderers[i].transform.position = new Vector3(previousWidth, 0, 0);
                }
            }
        }

        private void Update()
        {
            if (playerTransform == null || backgroundRenderers.Length == 0) return;

            // 현재 활성화된 배경들 중 가장 왼쪽과 오른쪽 위치 찾기
            float leftmostX = float.MaxValue;
            float rightmostX = float.MinValue;
            int leftmostIndex = 0;

            for (int i = 0; i < backgroundRenderers.Length; i++)
            {
                float currentX = backgroundRenderers[i].transform.position.x;
                if (currentX < leftmostX)
                {
                    leftmostX = currentX;
                    leftmostIndex = i;
                }
                if (currentX + backgroundWidths[i] > rightmostX)
                {
                    rightmostX = currentX + backgroundWidths[i];
                }
            }

            // 플레이어가 왼쪽 배경의 중간을 지나갔는지 체크
            if (playerTransform.position.x > leftmostX + backgroundWidths[leftmostIndex])
            {
                // 가장 왼쪽 배경을 가장 오른쪽으로 이동
                float newX = rightmostX;
                backgroundRenderers[leftmostIndex].transform.position = new Vector3(newX, 0, 0);
            }
        }

        // Unity 에디터에서 배경 크기를 시각적으로 확인하기 위한 기즈모
        private void OnDrawGizmos()
        {
            if (backgroundRenderers == null) return;

            foreach (var renderer in backgroundRenderers)
            {
                if (renderer != null && renderer.sprite != null)
                {
                    Gizmos.color = Color.yellow;
                    Vector3 center = renderer.transform.position;
                    Vector3 size = renderer.sprite.bounds.size;
                    Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 0));
                }
            }
        }
    }
}