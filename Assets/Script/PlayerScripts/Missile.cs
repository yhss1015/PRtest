using UnityEngine;

public class Missile : MonoBehaviour
{
    public float power = 5f;
    public float detectionRange = 5f;  // 사거리
    public float speed = 10f;  // 미사일 이동 속도
    public bool targeting = true;  // 추적 여부
    private Transform target;  // 목표 (가장 가까운 몬스터)
    private Vector3 initialDirection;  // 처음 지정된 방향

    public GameObject boom_Effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("미사일 몬스터 타격");
            Instantiate(boom_Effect, transform.position, Quaternion.identity);

            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(power);
            }

            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 가장 가까운 "Monster" 찾기
        target = FindClosestMonster();

        if (target != null)
        {
            // 목표 방향 계산
            Vector3 direction = target.position - transform.position;

            // 🔹 오른쪽(→) 기준으로 목표 방향을 향하도록 회전
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 직선 이동 모드일 때, 초기 방향 저장
            initialDirection = direction.normalized;
        }
        else
        {
            // 🔹 처음부터 타겟이 없으면 그냥 직진
            initialDirection = transform.right;  // 현재 미사일의 오른쪽 방향을 초기 방향으로 설정
            targeting = false;  // 추적 모드 비활성화
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (targeting && target != null)
        {
            // 🔹 추적 모드: 목표가 있으면 따라감
            MoveTowardsTarget();
        }
        else
        {
            // 🔹 비추적 모드 또는 타겟을 잃었을 때: 직선 이동
            MoveInInitialDirection();
        }
    }

    // 가장 가까운 "Monster"를 찾는 함수
    private Transform FindClosestMonster()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Transform closestMonster = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
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

    // 🔹 목표를 향해 이동하는 함수 (추적 모드)
    private void MoveTowardsTarget()
    {
        if (target == null)
        {
            // 🔹 타겟을 잃으면 비추적 모드로 변경
            targeting = false;            
            return;
        }

        // 목표 방향 계산
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 목표를 향해 미사일 회전 (기본 방향이 오른쪽이므로 적용)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 🔹 직선으로 이동하는 함수 (비추적 모드)
    private void MoveInInitialDirection()
    {
        transform.position += initialDirection * speed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}