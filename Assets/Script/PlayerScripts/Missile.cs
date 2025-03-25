using UnityEngine;

public class Missile : Weapon_All
{
    [Header("미사일 스탯")]
    public float detectionRange = 5f;  // 사거리
    public float attackRange = 5f;     // 범위 공격 반경
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

            // attackRange 내의 모든 적들에게 피해 적용
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Monster monster = enemy.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(AttackPower);
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    void Start()
    {
        target = FindClosestMonster();

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            initialDirection = direction.normalized;
        }
        else
        {
            initialDirection = transform.right;
            targeting = false;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (targeting && target != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            MoveInInitialDirection();
        }
    }

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

    private void MoveTowardsTarget()
    {
        if (target == null)
        {
            targeting = false;
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MoveInInitialDirection()
    {
        transform.position += initialDirection * speed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}