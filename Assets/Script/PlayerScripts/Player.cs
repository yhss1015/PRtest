using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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
    public bool isLeft = false;

    public PrefabManager prefabmanager;
    public SpriteRenderer spriteRenderer;
    [Header("채찍 공격 정보")]
    public GameObject[] attack_Prefab;
    public float whipAttack_Level = 1; // 기본 공격 레벨
    public float whipCool = 2f;    // 기본 공격 주기를 결정
    //public WeaponData whipWeapon;
    public float whipCount = 0;


    [Header("원거리 공격 정보")]
    public GameObject missile_prefab;
    public float MissileAttack_Level = 1;   // 투사체 공격 레벨
    public float MissileCool = 3f;  // 투사체 공격 주기


    [Header("써클 공격 정보")]
    public CircleAttackManager circleAttackManager;
    public GameObject circle_prefab;   // 원형 공격 프리팹
    // 채찍,원거리 공격도 따로 Manager를 만들어서 관리하도록 변경할 필요 있음. Player에 코드가 너무많아진거 같음.

    [Header("표창 공격 정보")]
    public GameObject ninjaStarPrefab;  // 표창 프리팹
    public float ninjaCount = 1;        // 한 번에 발사할 표창 개수
    public float ninjaCoolTime = 2f;    // 표창 공격 주기



    private Vector3 defaultScale;
    private Animator playerAnim; // 플레이어 애니메이터 가져옴
    [Header("아이템 매니저")]
    public ItemManager itemManager;

    void Start()
    {
        defaultScale = transform.localScale; // 초기 스케일 저장
        playerAnim = GetComponent<Animator>();
        prefabmanager = FindAnyObjectByType<PrefabManager>();
        circleAttackManager = FindAnyObjectByType<CircleAttackManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //무기 초기화는 PrefabManager에서 실행       

        // 2초마다 BasicAttackRoutine 실행
        StartCoroutine(BasicAttackRoutine());
        //StartCoroutine(MissileAttackRoutine());
        //AcquireCircleAttack();
        // 표창 공격 코루틴 실행
        //StartCoroutine(NinjaStarAttackRoutine());


    }

    void Update()
    {
        PMove();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Slash sl = attack_Prefab[0].GetComponent<Slash>();
            sl.AttackPower += 1;
            Debug.Log("능력치업");

            IncreaseCircleCount();
        }
    }

    void PMove()
    {
        float curSpeedX = Input.GetAxis("Horizontal") * speed * 1.25f * Time.deltaTime;
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

    // 채찍 공격 코루틴
    IEnumerator BasicAttackRoutine()
    {
        while (true)
        {
            BasicAttack();
            yield return new WaitForSeconds(whipCool); // 2초 대기 후 반복
        }
    }

    // 원거리 공격 코루틴
    IEnumerator MissileAttackRoutine()
    {
        while (true)
        {
            Instantiate(missile_prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(MissileCool);
        }
    }

    // 써클 공격 시작 함수
    void AcquireCircleAttack()
    {
        if (circleAttackManager == null)
        {
            circleAttackManager = gameObject.AddComponent<CircleAttackManager>();
            circleAttackManager.circlePrefab = circle_prefab; //플레이어에서 프리팹 할당할시 사용
            circleAttackManager.StartCircleAttack();
        }
    }

    // 써클 공격 갯수 증가 함수
    void IncreaseCircleCount()
    {
        if (circleAttackManager != null)
        {
            int newCount = circleAttackManager.maxCircles + 1;
            if (newCount > 6) newCount = 6; // 최대 6개 제한
            circleAttackManager.UpdateCircleCount(newCount);
        }
    }


    // 표창 공격 생성 코루틴
    IEnumerator NinjaStarAttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < ninjaCount; i++)
            {
                // 표창 생성
                Instantiate(ninjaStarPrefab, transform.position, Quaternion.identity);
                SoundManager.Instance.slashSound(); // 표창 발사 사운드

                // 0.1초 간격으로 표창을 발사
                yield return new WaitForSeconds(0.1f);
            }

            // 표창 발사 주기만큼 대기
            yield return new WaitForSeconds(ninjaCoolTime);
        }
    }


    // 체젠 코루틴
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
        if (whipCount != 1)
        {
            Instantiate(attack_Prefab[0], transform.position + attack_Prefab[0].transform.position, attack_Prefab[0].transform.rotation);
            SoundManager.Instance.slashSound();
            // 0.1초 후 오른쪽 공격 실행
            StartCoroutine(DelayedAttack(0.1f));
        }
        else if ((whipCount == 1 && isLeft == true)) // 채찍 횟수가 1이면서 왼쪽을 바라볼떄
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

        // 색상 변경 효과 적용
        StartCoroutine(DamageEffect());

        if (curHp <= 0)
        {
            Debug.Log("플레이어 사망");

        }
    }

    private IEnumerator DamageEffect()
    {
        // 색상을 빨간색으로 변경
        spriteRenderer.color = Color.red;

        // 일정 시간 대기
        yield return new WaitForSeconds(0.2f);

        // 원래 색상으로 복구
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public void PlusHp(float amount)
    {
        curHp += amount;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
    }

    public void PlusExp(float amount)
    {
        // 경험치 증가
        curExp += amount;
        if (curExp >= maxExp) // 레벨경험치 꽉찰시 현재경험치 차감,최대 경험치 상승, 레벨 상승
        {
            curExp -= maxExp; // 경험치 초기화 및 초과분 반환
            maxExp *= 1.1f; // 최대 경험치 증가
            Level++;
            SoundManager.Instance.PlaySound(SoundManager.Instance.levelUp); // 레벨업 사운드
            // 레벨업 ui 함수 추가


            GameManager.Instance.levelUpEvent();

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


    // 특정 무기정보를 가져온다.(참조 변수 활용)
    public WeaponData FindWeaponInfo(WeaponType weaponType)
    {

        if (itemManager == null)
        {
            Debug.LogWarning("씬에 ItemManager 객체를 생성하고 스크립트를 넣어주세요.");
            return null;
        }
        else
        {
            WeaponData curWeapon = null;
            foreach (var weapon in itemManager.weaponDataList)
            {
                if (weapon.weaponType == weaponType)
                {
                    curWeapon = weapon;
                    //UpdateWeaponInfo(curWeapon.weaponType,weapon);
                    break;  // 첫 번째 Whip 무기를 찾으면 반복 종료
                }
            }
            return curWeapon;
        }

    }

    // 특정 무기의 정보를 가지고 플레이어의 무기를 업데이트한다.(특정무기만)
    // ex. 채찍무기를 레벨업 하였다-> UpdateWeaponInfo(WeaponData @@@) 알아서 구분하여 업데이트
    public void UpdateWeaponInfo(WeaponData weapondata,int level)
    {
        WeaponType wt;
        switch (weapondata.weaponType)
        {
            case WeaponType.Whip:
                if(level==0)
                {
                    whipCool = weapondata.baseCoolTime;
                    whipCount = weapondata.baseProjectileCount;
                }
                else
                {
                    whipCool += weapondata.levelData[level].cooldownChange;
                    whipCount += weapondata.levelData[level].additionalProjectiles;
                }

                    break;
            case WeaponType.MagicWand:
                MissileCool = weapondata.baseCoolTime;
                break;
            case WeaponType.KingVible:
                Debug.Log("킹 바이블");
                foreach (var prefab in prefabmanager.prefabs)
                {
                    wt = prefab.GetComponent<Weapon_All>().weaponType;
                    if (wt == WeaponType.KingVible)
                    {
                        circleAttackManager.maxCircles = prefab.GetComponent<Weapon_All>().ProjectileCount;
                        circleAttackManager.UpdateCircleCount(prefab.GetComponent<Weapon_All>().ProjectileCount);
                        break;
                    }
                }
                break;
            case WeaponType.NinjaStar:
                if(level==0)
                {
                    ninjaCount = weapondata.baseProjectileCount;
                    ninjaCoolTime = weapondata.baseCoolTime; 
                }
                ninjaCount += weapondata.levelData[level].additionalProjectiles;
                ninjaCoolTime += weapondata.levelData[level].additionalProjectiles;
                break;
            default:
                break;
        }

        /*
        // 웨폰 데이터의 레벨이 1이라면 공격을 시작하는 함수를 실행한다
        if (weapondata.레벨)
        {
            StartCoroutine(BasicAttackRoutine());
            StartCoroutine(MissileAttackRoutine());
            AcquireCircleAttack();
            // 표창 공격 코루틴 실행
            StartCoroutine(NinjaStarAttackRoutine());
            
            switch문으로 구분


        }
        */

    }
    public void StartWeapon(WeaponData weapon)
    {
        switch(weapon.weaponType)
        {
            case WeaponType.MagicWand:
                StartCoroutine(MissileAttackRoutine());
                break;
            case WeaponType.KingVible:
                AcquireCircleAttack();
                break;
            case WeaponType.NinjaStar:
                StartCoroutine(NinjaStarAttackRoutine());
                break;
            default:
                Debug.LogWarning("없는 무기 타입");
                break;
        }
    }
}
