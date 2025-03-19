using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100;
    public float curHp = 100;
    public float speedX = 5;
    public float speedY = 4;
    public float attack = 10;

    public float Level = 1;

    public float BasicAttack_Level = 1;

    private Vector3 defaultScale;
    private Animator playerAnim; // 플레이어 애니메이터 가져옴

    public GameObject []attack_Prefab;

    void Start()
    {
        defaultScale = transform.localScale; // 초기 스케일 저장
        playerAnim = GetComponent<Animator>();

        // 2초마다 BasicAttackRoutine 실행
        StartCoroutine(BasicAttackRoutine());
    }

    void Update()
    {
        PMove();

    }

    void PMove()
    {
        float curSpeedX = Input.GetAxis("Horizontal") * speedX * Time.deltaTime;
        float curSpeedY = Input.GetAxis("Vertical") * speedY * Time.deltaTime;

        if (curSpeedX != 0 || curSpeedY != 0)
        {
            // 이동 중이면 Walk 활성화, Idle 비활성화
            playerAnim.SetBool("Walk", true);
            playerAnim.SetBool("Idle", false);

            transform.Translate(curSpeedX, curSpeedY, 0);

            // 이동 방향에 따라 X 스케일 변경 ( 방향 전환 )
            if (curSpeedX > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            }
            else if (curSpeedX < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            }
        }
        else
        {
            // 멈추면 Walk 비활성화, Idle 활성화
            playerAnim.SetBool("Walk", false);
            playerAnim.SetBool("Idle", true);
        }
    }

    IEnumerator BasicAttackRoutine()
    {
        while (true)
        {
            BasicAttack();
            yield return new WaitForSeconds(2f); // 2초 대기 후 반복
        }
    }

    void BasicAttack()
    {
        if (attack_Prefab.Length < 2) return;

        // 왼쪽 공격
        Instantiate(attack_Prefab[0], transform.position + attack_Prefab[0].transform.position, attack_Prefab[0].transform.rotation);

        // 0.1초 후 오른쪽 공격 실행
        StartCoroutine(DelayedAttack());
    }

    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(attack_Prefab[1], transform.position + attack_Prefab[1].transform.position, attack_Prefab[1].transform.rotation);
    }


    void TakeDamage(float dmg)
    {
        curHp -= dmg;

        if(curHp<=0)
        {
            Debug.Log("플레이어 사망");

        }
    }
    
    void PlusHp(float amount)
    {
        curHp += amount;
        if(curHp>maxHp)
        {
            curHp = maxHp;
        }
    }
}
