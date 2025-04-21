using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float spawnRate = 2f;
        [SerializeField] private float obstacleXSpacing = 5f;
        
        [Header("Gap Settings")]
        [SerializeField] private float minGapSize = 2f;
        [SerializeField] private float maxGapSize = 4f;
        [SerializeField] private float minYPosition = -2f;
        [SerializeField] private float maxYPosition = 2f;

        [Header("Game Settings")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float despawnDistance = 5f;
        [SerializeField] private int initialObstacleCount = 5;

        private List<GameObject> activeObstacles = new List<GameObject>();
        private float playerStartX;
        private float nextSpawnX;
        private Transform obstaclesParent;

        private void Start()
        {
            // 플레이어 시작 위치 기록
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
                if (playerTransform == null)
                {
                    Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
                    enabled = false;
                    return;
                }
            }

            // 모든 장애물을 담을 부모 오브젝트 생성
            CreateObstaclesParent();

            // 플레이어 시작 위치에서 일정 거리 앞에서부터 장애물 생성 시작
            playerStartX = playerTransform.position.x;
            nextSpawnX = playerStartX + 10f; // 게임 시작 후 10유닛 앞에서 첫 장애물 생성

            // 초기 장애물 생성
            for (int i = 0; i < initialObstacleCount; i++)
            {
                SpawnObstacle();
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // 플레이어가 다음 생성 지점에 가까워지면 새 장애물 생성
            if (playerTransform.position.x + obstacleXSpacing > nextSpawnX)
            {
                SpawnObstacle();
            }

            // 지나간 장애물 제거
            RemovePassedObstacles();
        }

        private void CreateObstaclesParent()
        {
            // 모든 장애물을 담을 부모 오브젝트 생성
            GameObject parent = new GameObject("Obstacles");
            obstaclesParent = parent.transform;
        }

        private void SpawnObstacle()
        {
            if (obstaclePrefab == null)
            {
                Debug.LogError("Obstacle prefab is not assigned!");
                return;
            }

            // 장애물 인스턴스 생성
            GameObject obstacle = Instantiate(obstaclePrefab, obstaclesParent);
            obstacle.transform.position = new Vector3(nextSpawnX, 0, 0);
            
            // 랜덤 간격과 위치 계산
            float gapSize = Random.Range(minGapSize, maxGapSize);
            float centerY = Random.Range(minYPosition, maxYPosition);
            
            // 상단과 하단 장애물 찾기
            Transform topObstacle = obstacle.transform.Find("TopObstacle");
            Transform bottomObstacle = obstacle.transform.Find("BottomObstacle");
            
            if (topObstacle != null && bottomObstacle != null)
            {
                // 간격 적용
                topObstacle.localPosition = new Vector3(0, centerY + gapSize/2, 0);
                bottomObstacle.localPosition = new Vector3(0, centerY - gapSize/2, 0);
            }
            else
            {
                Debug.LogError("Obstacle prefab is missing TopObstacle or BottomObstacle child objects!");
            }
            
            // 활성 장애물 목록에 추가
            activeObstacles.Add(obstacle);
            
            // 다음 생성 위치 업데이트
            nextSpawnX += obstacleXSpacing;
        }

        private void RemovePassedObstacles()
        {
            // 플레이어가 지나간 장애물 제거
            for (int i = activeObstacles.Count - 1; i >= 0; i--)
            {
                if (activeObstacles[i] == null) continue;
                
                // 플레이어보다 despawnDistance 이상 뒤에 있는 장애물 제거
                if (activeObstacles[i].transform.position.x < playerTransform.position.x - despawnDistance)
                {
                    Destroy(activeObstacles[i]);
                    activeObstacles.RemoveAt(i);
                }
            }
        }

        // Unity 에디터에서 설정을 시각화
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying && transform.position != null)
            {
                // 장애물 간격 시각화
                Gizmos.color = Color.blue;
                for (int i = 0; i < 5; i++)
                {
                    Vector3 pos = transform.position + new Vector3(i * obstacleXSpacing, 0, 0);
                    Gizmos.DrawWireSphere(pos, 0.5f);
                }

                // 장애물 갭 시각화
                Gizmos.color = Color.green;
                Vector3 center = transform.position + Vector3.up * ((maxYPosition + minYPosition) / 2);
                Vector3 size = new Vector3(1, maxYPosition - minYPosition, 0);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}