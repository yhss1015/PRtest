using UnityEngine;

public class Monster : MonoBehaviour
{
    public float HP = 20;
    public float Speed = 3;
    public float Attack = 5;

    public GameObject target;  //플레이어
    Vector2 Dir;
    Vector2 DirNo;

    Animator Mob_Ani;

    void Start()
    {
        Mob_Ani= GetComponent<Animator>();  
    }

    void Update()
    {
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

    public void TakeDamage(float dmg) // 몬스터가 피해를 입는 함수
    {
        HP -= dmg;

        if(HP <= 0)
        {
            Die();
            //ExpDrop();
        }
    }

    void Die()
    {
        Mob_Ani.SetTrigger("Die");
        Destroy(gameObject, 1.5f);
    }

    //public void ExpDrop()
    //{

    //}
}
