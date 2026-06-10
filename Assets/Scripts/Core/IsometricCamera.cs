using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 20f;
    [SerializeField] private float height = 15f;
    [SerializeField] private float angle = 45f; // 아이소메트릭 각도
    [SerializeField] private float smoothSpeed = 5f;
    
    private Vector3 offset;

    void Start()
    {
        // 아이소메트릭 카메라 초기 설정
        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            UpdateCameraPosition();
        }
    }

    void UpdateCameraPosition()
    {
        // 아이소메트릭 뷰: 45도 각도
        float angleRad = angle * Mathf.Deg2Rad;
        Vector3 desiredPosition = target.position + new Vector3(
            Mathf.Cos(angleRad) * distance,
            height,
            Mathf.Sin(angleRad) * distance
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 2f);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
