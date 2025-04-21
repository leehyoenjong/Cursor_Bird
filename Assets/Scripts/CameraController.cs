using UnityEngine;

namespace FlappyBird
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 1, -10);
        
        private void LateUpdate()
        {
            if (target == null) return;

            // 카메라의 목표 위치 계산 (y축과 z축은 고정)
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x,
                offset.y,
                offset.z
            );

            // 부드러운 이동을 위한 보간
            Vector3 smoothedPosition = Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

            // 카메라 위치 업데이트
            transform.position = smoothedPosition;
        }
    }
}