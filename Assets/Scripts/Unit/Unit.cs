using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private Renderer unitRenderer;
    
    private Vector3 targetPosition;
    private bool isMoving = false;
    private FogOfWarSystem fogOfWar;
    private Animator animator;

    void Start()
    {
        fogOfWar = FindObjectOfType<FogOfWarSystem>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        
        // FogOfWar 시스템에 유닛 등록
        if (fogOfWar != null)
            fogOfWar.RegisterUnit(this);
    }

    void Update()
    {
        MoveTowards(targetPosition);
        UpdateVisibility();
    }

    void MoveTowards(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);
        
        if (distance > stoppingDistance)
        {
            isMoving = true;
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            
            // 캐릭터가 이동 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else
        {
            isMoving = false;
        }

        // 애니메이션 업데이트
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }

    void UpdateVisibility()
    {
        // Fog of War 시스템에 따라 시각적 표현 조정
        if (fogOfWar != null)
        {
            bool isVisible = fogOfWar.IsVisible(transform.position);
            
            if (unitRenderer != null)
            {
                // 보이는 상태와 안 보이는 상태를 구분
                Color rendererColor = unitRenderer.material.color;
                if (!isVisible)
                {
                    rendererColor.a = 0.3f; // 투명하게
                }
                else
                {
                    rendererColor.a = 1f; // 완전히 보임
                }
                unitRenderer.material.color = rendererColor;
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
    }

    void OnDestroy()
    {
        if (fogOfWar != null)
            fogOfWar.UnregisterUnit(this);
    }
}
