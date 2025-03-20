using UnityEngine;

public class Missile : MonoBehaviour
{
    public float detectionRange = 5f;  // 사거리
    public float speed = 10f;  // 미사일 이동 속도
    public bool targeting = true;  // 추적 여부
    private Transform target;  // 목표 (가장 가까운 몬스터)
    private Vector3 initialDirection;  // 처음 지정된 방향

    public GameObject boom_Effect;

    void Start()
    {
        // Start에서 "Monster" 태그를 가진 가장 가까운 객체를 찾기
        target = FindClosestMonster();

        if (target != null)
        {
            // 목표를 향해 미사일이 바로 향하도록 회전시킴
            Vector3 direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // Z축을 기준으로 회전

            // 직선 날아가기 모드일 때, 처음 방향을 설정
            initialDirection = direction.normalized;
        }
        else
        {
            Debug.LogWarning("No Monster found within range.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            if (targeting)
            {
                // 추적 모드
                MoveTowardsTarget();
            }
            else
            {
                // 직선 비추적 모드
                MoveInInitialDirection();
            }
        }
    }

    // 가장 가까운 "Monster"를 찾는 함수 (2D 물리 사용)
    private Transform FindClosestMonster()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Transform closestMonster = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMonster = collider.transform;
                }
            }
        }

        return closestMonster;
    }

    // 목표 방향으로 미사일을 이동시키는 함수
    private void MoveTowardsTarget()
    {
        // 목표 방향으로 미사일 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 목표를 향해 미사일 회전
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 회전 속도 조절
    }

    // 초기 방향으로만 미사일을 이동시키는 함수
    private void MoveInInitialDirection()
    {
        // 직선으로 목표를 향한 방향으로 이동
        transform.position += initialDirection * speed * Time.deltaTime;

        // 미사일이 직선으로 목표 방향으로 날아가도록 회전은 하지 않음
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("미사일 몬스터 타격");
            Instantiate(boom_Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}