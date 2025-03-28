using System.Threading;
using UnityEngine;

public class NinjaStar : Weapon_All
{
    private Transform player;
    private Transform closestEnemy;

    public float maxSpeed = 10f;  // 초기 속도
    public float maxDistance = 7f;  // 최대 거리
    public float detectRange = 10f;  // 플레이어 주위 탐지 범위

    private Vector3 direction;
    private float speed;
    private bool isThrown = false;
    private float distanceTraveled = 0f;
    private Vector3 initialPosition;

    //public GameObject hitEffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Instantiate(hitEffect, transform.position, Quaternion.identity);
            if (collision.GetComponent<Monster>())
            {
                Monster monster = collision.GetComponent<Monster>();
                monster.TakeDamage(AttackPower);
            }
            else if(collision.GetComponent<EventMonster>())
            {
                EventMonster monster = collision.GetComponent<EventMonster>();
                monster.TakeDamage(AttackPower);
            }
            //Monster monster = collision.GetComponent<Monster>() ? collision.GetComponent<Monster>() : collision.GetComponent<EventMonster>();
            //monster.TakeDamage(AttackPower);

        }
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FindClosestEnemy();  // 게임 시작 시 범위 내 가장 가까운 적을 찾음
        initialPosition = transform.position;
        speed = maxSpeed;  // 시작 속도는 maxSpeed
    }

    void Update()
    {
        /*if (closestEnemy != null)
        {
            Move();  // 자연스럽게 속도가 감소하면서 이동
        }*/
        Move();  // 자연스럽게 속도가 감소하면서 이동
    }

    void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.position, detectRange);  // 범위 내 모든 콜라이더 찾기
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))  // 태그가 "Enemy"인 오브젝트만 처리
            {
                float distance = Vector3.Distance(player.position, collider.transform.position);  // 3D 거리 계산
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = collider.transform;
                }
            }
        }

        // 가장 가까운 적이 있다면 그 적을 선택
        if (closest != null)
        {
            closestEnemy = closest;
            direction = (closest.position - player.position).normalized;  // 목표 방향 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        // 속도 감소: 매 프레임마다 일정 비율로 속도가 감소
        speed = Mathf.Max(-maxSpeed, speed - Time.deltaTime * 20f);  // 2f는 속도 감소 비율을 조정하는 값

        // 표창 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        distanceTraveled += Mathf.Abs(speed) * Time.deltaTime;

        // 일정 거리 이상 날아갔다면 그냥 계속 날아가도록
        if (distanceTraveled >= maxDistance)
        {
            // 표창은 계속 날아가도록 그냥 둡니다.
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
