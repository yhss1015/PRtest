using System.Collections;
using UnityEngine;

public class Ranger_Monster : MonoBehaviour
{
    public float HP = 20;
    public float Speed = 3;
    public float Attack = 5;
    //public float Dying =1.5f; // 몬스터 사망 모션 시간조절
    public float KnockBack = 5f; // 피격시 넉백 정도
    public float Interval = 5f;
    public float detectionrange = 10f;
    private float fireTimer;


    public GameObject target;  //플레이어
    public GameObject EXP;
    public GameObject Missileprefab;
    public Transform Shooter;
    private Transform Player;

    Vector2 Dir;
    Vector2 DirNo;

    Animator Mob_Ani;

    private bool isDead = false; // 몬스터가 죽었는지 확인하는 변수

    private bool isAttacking = false;  // 지속피해 구현을 위한 bool
    private float DamageTurm = 0.5f;  // 지속피해 주기


    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private Color original;

    private Coroutine DamageCoroutine;

    //for pooling
    public int index;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Mob_Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        fireTimer = Interval;

        original = sr.color; // 몬스터의 기본 색상 저장 
    }

    void Update()
    {
        if (HP <= 0) return; // 죽었으면 이동하지 않음

        target = GameObject.FindGameObjectWithTag("Player");

        Dir = target.transform.position - transform.position;
        DirNo = Dir.normalized;

        transform.Translate(DirNo * Speed * Time.deltaTime);

        if (Dir.x > 0)
            transform.localScale = new Vector3(-1, 1, 1); // 오른쪽
        else if (Dir.x < 0)
            transform.localScale = new Vector3(1, 1, 1); // 왼쪽

        float DistanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (DistanceToPlayer <= detectionrange)
        {
            //플레이어 방향으로 스프라이트 회전
            sr.flipX = (Player.position.x < transform.position.x);

            //미사일 발사 로직
            fireTimer -= Time.deltaTime;  // 타이머 감소
            if (fireTimer < 0)
            {
                shoot();  //미사일 발사
                fireTimer = Interval; //타이머 리셋;
            }
        }

    }

    void shoot()
    {
        //미사일 생성
        GameObject missile = Instantiate(Missileprefab, Shooter.position, Quaternion.identity);

        //플레이어 방향으로 발사 방향 설정
        Vector2 Direction = (Player.position - Shooter.position).normalized;
        missile.GetComponent<Enemy_Missile>().SetDirection(Direction);
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
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f); // 0.1초 유지  
        sr.color = original; // 색 돌아옴
        Debug.Log("코루틴발생");
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
            ExpDrop();
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

    public void ExpDrop()
    {
        Instantiate(EXP, transform.position, Quaternion.identity);
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
