using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float HP = 20;
    public float Speed = 3;
    public float Attack = 5;
    public float KnockBack = 10f; // 피격시 넉백 정도


    public GameObject target;  //플레이어
    public GameObject pos;
    public GameObject Missileprefab;
    public GameObject Missileprefab2;
    public Transform center;
    Vector2 Dir;
    Vector2 DirNo;

    public Animator Mob_Ani;

    public bool isDead = false; // 몬스터가 죽었는지 확인하는 변수

    public bool isAttacking = false;  // 지속피해 구현을 위한 bool
    public float DamageTurm = 0.5f;  // 지속피해 주기


    public SpriteRenderer sr;
    public Rigidbody2D rb;

    public Color original;

    public Coroutine DamageCoroutine;

    //for pooling
    public int index;


    void Start()
    {
        Mob_Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        original = sr.color; // 몬스터의 기본 색상 저장 

        StartCoroutine(CircleFire());
        StartCoroutine(SpreadBullet());
    }

    void Update()
    {
        if (HP <= 0) return; // 죽었으면 이동하지 않음
        if (index >= 0) //이벤트 몬스터가 아니면
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }


        Dir = target.transform.position - transform.position;
        DirNo = Dir.normalized;

        transform.Translate(DirNo * Speed * Time.deltaTime);


        if (Dir.x > 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);// 오른쪽
            sr.flipX = false;
        }
        else if (Dir.x < 0)
        {
            //transform.localScale = new Vector3(1, 1, 1); // 왼쪽
            sr.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 닿을 경우
    {
        if (collision.CompareTag("Player") && !isAttacking)  // 태그= Player, isAttacking = false
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                isAttacking = true;
                DamageCoroutine = StartCoroutine(ConDam(player));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 떨어지는 경우
    {
        if (collision.CompareTag("Player") && isAttacking) // 태그= Player, isAttacking = true
        {
            isAttacking = false;

            if (DamageCoroutine != null)
            {
                StopCoroutine(DamageCoroutine);
                DamageCoroutine = null;
            }
        }
    }

    IEnumerator CircleFire()
    {
        //공격주기
        float attackRate = 6;
        //발사체 생성갯수
        int count =18;
        //발사체 사이의 각도
        float intervalAngle = 360 / count;
        //가중되는 각도(항상 같은 위치로 발사하지 않도록 설정
        float weightAngle = 0f;

        while (true)
        {
            yield return new WaitForSeconds(attackRate);

            for (int i = 0; i < count; ++i)
            {
                //발사체 생성
                GameObject clone = Instantiate(Missileprefab, transform.position, Quaternion.identity);

                //발사체 이동 방향(각도)
                float angle = weightAngle + intervalAngle * i;
               
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                //발사체 이동 방향 설정
                clone.GetComponent<Enemy_Missile>().SetDirection(new Vector2(x, y));
            }
            //발사체가 생성되는 시작 각도 설정을 위한변수
            weightAngle += 1;

        }

    }

    IEnumerator SpreadBullet()
    {
        float attackRate = 10;
        int count = 30;
        float intervalAngle = 360f / count;
        float fireDelay = 0.1f;
        while (true)
        {
            yield return new WaitForSeconds(attackRate);

            for (int i = 0; i < count; ++i)
            {
                
                GameObject missile = Instantiate(Missileprefab2, transform.position, Quaternion.identity);
               
                float angle = intervalAngle * i;

                // 발사체의 이동 방향 설정 (원형 경로를 따라)
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                // 발사체 이동 방향 설정
                missile.GetComponent<Enemy_Missile>().SetDirection(new Vector2(x, y));

                // fireDelay 간격으로 하나씩 발사
                yield return new WaitForSeconds(fireDelay); 

            }
        }
    }

    IEnumerator ConDam(Player player)
    {
        while (isAttacking && player != null)
        {
            player.TakeDamage(Attack);
            yield return new WaitForSeconds(DamageTurm); // DamageTrum 마다 피해
        }

        isAttacking = false;
        DamageCoroutine = null;
    }

    IEnumerator KnockbackCoroutine(Vector2 AttackDir)
    {
        rb.linearVelocity = AttackDir * KnockBack;
        yield return new WaitForSeconds(0.15f); // n초 간 넉백
        rb.linearVelocity = Vector2.zero; // 멈춤
    }

    void Knockback(Vector2 AttackDir)
    {
        StartCoroutine(KnockbackCoroutine(AttackDir));
    }

    IEnumerator Flashing()
    {
        sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f); // 0.1초 유지  
        sr.color = original; // 색 돌아옴
    }

    public void TakeDamage(float dmg) // 몬스터가 피해를 입는 함수
    {
        if (isDead) return; // 이미 죽었으면 추가 피해를 받지 않음
        HP -= dmg;

        StartCoroutine(Flashing()); // 피격 시 점멸

        Vector2 knockbackDir = (transform.position - target.transform.position).normalized; // 넉백 방향
        Knockback(knockbackDir);

        if (HP <= 0)
        {
            isDead = true; // 몬스터 사망 처리
            Die();
        }
    }

    void Die()
    {
        Mob_Ani.SetTrigger("Die");
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo stateInfo = Mob_Ani.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length; // 현재 애니메이션의 길이 가져오기
        yield return new WaitForSeconds(animationLength);

        GameManager.Instance.pool.ReturnPool(this.gameObject, index);
        InitStat();

    }


    //Add YSW for pooling
    public void Initialize_Index(int index)
    {
        this.index = index;
    }

    //조정이 필요
    public void InitStat()
    {
        HP = 20;
        Speed = 1;
        Attack = 10;
        isDead = false;
    }
}