using UnityEngine;

namespace FlappyBird
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float moveSpeed = 3f;
        
        private Rigidbody2D rb;
        private bool isAlive = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!isAlive) return;

            // 모바일 터치 입력 처리
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Jump();
                }
            }

            // 에디터에서 테스트를 위한 마우스 클릭 처리
            #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
            #endif
        }

        private void FixedUpdate()
        {
            if (!isAlive) return;

            // 일정한 속도로 오른쪽으로 이동
            Vector2 velocity = rb.linearVelocity;
            velocity.x = moveSpeed;
            rb.linearVelocity = velocity;
        }

        private void Jump()
        {
            // y축 속도만 초기화하고 일정한 힘으로 위로 점프
            Vector2 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 충돌 시 게임 오버 처리
            isAlive = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}