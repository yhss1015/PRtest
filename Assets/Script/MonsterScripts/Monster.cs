using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float HP = 20;
    public float Speed = 3;
    public float Attack = 5;
    public float Dying =1.5f; // 몬스터 사망 모션 시간조절
    public float KnockBack = 10f; // 피격시 넉백 정도


    public GameObject target;  //플레이어
    public GameObject EXP;
    Vector2 Dir;
    Vector2 DirNo;
   
    Animator Mob_Ani;

    private bool isDead = false; // 몬스터가 죽었는지 확인하는 변수


    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private Color original;

    //for pooling
    public int index;


    void Start()
    {
        Mob_Ani= GetComponent<Animator>();  
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        original = sr.color; // 몬스터의 기본 색상 저장
        Debug.Log("원래 색상: " + original);

    }

    void Update()
    {
        if (HP <= 0) return; // 죽었으면 이동하지 않음

        target = GameObject.FindGameObjectWithTag("Player"); 

        Dir = target.transform.position - transform.position;
        DirNo = Dir.normalized;

        transform.Translate(DirNo * Speed *Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.TakeDamage(Attack); //플레이어 스크립트의 함수 호출
                Debug.Log("플레이어 데미지");
            }
        }
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
        
        sr.color = Color.white; // 흰색 변경
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

        if(HP <= 0)
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
