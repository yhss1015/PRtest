using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using VampireSurvival.ItemSystem;

public class Player : MonoBehaviour
{
    public float maxHp = 100;       // 최대 체력
    public float curHp = 100;       // 현재 체력
    public float speed = 4;         // 스피드
    public float attack = 10;       // 공격력
    public float recoveryHp = 1f;   // 1초마다 재생 체력

    public float curExp = 0;
    public float maxExp = 100;

    public float Level = 1; //캐릭터 레벨

    public float BasicAttack_Level = 1; // 기본 공격 레벨
    public float BasicCool = 2f;    // 기본 공격 주기를 결정
    public WeaponData whipWeapon;
    public float whipCount = 0;
    public bool isLeft = false;

    public float MissileAttack_Level = 1;   // 투사체 공격 레벨
    public float MissileCool = 3f;  // 투사체 공격 주기


    

    private Vector3 defaultScale;
    private Animator playerAnim; // 플레이어 애니메이터 가져옴

    public GameObject []attack_Prefab;
    public GameObject missile_prefab;

    public ItemManager itemManager;

    void Start()
    {
        defaultScale = transform.localScale; // 초기 스케일 저장
        playerAnim = GetComponent<Animator>();

        FindWeaponInfo(WeaponType.Whip, ref whipWeapon);
        BasicCool = whipWeapon.baseCoolTime;
        whipCount = whipWeapon.baseProjectileCount;

        // 2초마다 BasicAttackRoutine 실행
        StartCoroutine(BasicAttackRoutine());
        StartCoroutine(MissileAttackRoutine());
    }

    void Update()
    {
        PMove();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Slash sl = attack_Prefab[0].GetComponent<Slash>();
            sl.power += 1;
            Debug.Log("능력치업");
        }
    }

    void PMove()
    {
        float curSpeedX = Input.GetAxis("Horizontal") * speed *1.25f * Time.deltaTime;
        float curSpeedY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if (curSpeedX != 0 || curSpeedY != 0)
        {
            // 이동 중이면 Walk 활성화, Idle 비활성화
            playerAnim.SetBool("Walk", true);
            playerAnim.SetBool("Idle", false);

            transform.Translate(curSpeedX, curSpeedY, 0);

            // 이동 방향에 따라 X 스케일 변경 ( 방향 전환 )
            if (curSpeedX > 0)
            {
                isLeft = false;
                transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            }
            else if (curSpeedX < 0)
            {
                isLeft = true;
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
            yield return new WaitForSeconds(BasicCool); // 2초 대기 후 반복
        }
    }

    IEnumerator MissileAttackRoutine()
    { 
        while (true)
        {
            Instantiate(missile_prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(MissileCool);
        }
    }

    IEnumerator RecoveryRoutine()
    {
        while (true)
        {
            PlusHp(recoveryHp);
            yield return new WaitForSeconds(1f);
        }
    }


    void BasicAttack()
    {
        if (attack_Prefab.Length < 1) return;


        // 왼쪽 공격
        if (whipCount != 1 )
        {
            Instantiate(attack_Prefab[0], transform.position + attack_Prefab[0].transform.position, attack_Prefab[0].transform.rotation);
            SoundManager.Instance.slashSound();
            // 0.1초 후 오른쪽 공격 실행
            StartCoroutine(DelayedAttack(0.1f));
        }
        else if((whipCount == 1 && isLeft == true)) // 채찍 횟수가 1이면서 왼쪽을 바라볼떄
        {
            Instantiate(attack_Prefab[0], transform.position + attack_Prefab[0].transform.position, attack_Prefab[0].transform.rotation);
            SoundManager.Instance.slashSound();
            
        }
        else // 오른쪽을 바라볼때 (채찍횟수 1이면서)
        {
            StartCoroutine(DelayedAttack(0.0f));
        }
            
        
        
    }

    IEnumerator DelayedAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.slashSound();
        Instantiate(attack_Prefab[1], transform.position + attack_Prefab[1].transform.position, attack_Prefab[1].transform.rotation);
    }


    public void TakeDamage(float dmg)
    {
        curHp -= dmg;

        if(curHp<=0)
        {
            Debug.Log("플레이어 사망");

        }
    }
    
    public void PlusHp(float amount)
    {
        curHp += amount;
        if(curHp>maxHp)
        {
            curHp = maxHp;
        }
    }

    public void PlusExp(float amount)
    {
        // 경험치 증가
        curExp += amount;
        if(curExp>=maxExp) // 레벨경험치 꽉찰시 현재경험치 차감,최대 경험치 상승, 레벨 상승
        {
            curExp -= maxExp;
            maxExp *= 1.1f; // 최대 경험치 증가
            Level++;
            // 레벨업 ui 함수 추가
        }
        
    }


    // 단순 스텟 증가 참조 변수 활용
    public void Something(float amount, ref float stat)   // ex. ( 증가할 값 , player.speed )
    {
        stat += amount;
    }

    public void PlusSpeed(float amount)
    {
        speed += amount;
    }

    public void FindWeaponInfo(WeaponType weaponType,ref WeaponData curWeapon)
    {
        if(itemManager==null)
        {
            Debug.LogWarning("씬에 ItemManager 객체를 생성하고 스크립트를 넣어주세요.");
        }
        else
        {
            foreach (var weapon in itemManager.weaponDataList)
            {
                if (weapon.weaponType == weaponType)
                {
                    curWeapon = weapon;
                    break;  // 첫 번째 Whip 무기를 찾으면 반복 종료
                }
            }
        }
            
    }
}
