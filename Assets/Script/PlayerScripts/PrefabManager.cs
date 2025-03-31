using System.Collections;
using UnityEngine;
using VampireSurvival.ItemSystem;

public class PrefabManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public WeaponData wd;
    public WeaponData wd2;
    public WeaponData wd3;
    public WeaponData wd4;
    public PlayerInventory playerInventory;
    

    WeaponType wt; // 디버깅용
    Weapon_All wa;
    int count;
    Player player;


    // Weapondata변경 사항을 프리팹에 적용하는 함수
    public void UpdateWeaponPrefab(WeaponData weaponData,int level)
    {
        if (weaponData.weaponType == WeaponType.Whip)
        {
            count = 2;
        }
        else
        {
            count = 1;
        }
        foreach (var prefab in prefabs)
        {
            wt = prefab.GetComponent<Weapon_All>().weaponType;

            if (wt == weaponData.weaponType)
            {
                count--;
                wa = prefab.GetComponent<Weapon_All>();
                if(level==1)
                {
                    wa.AttackPower = weaponData.baseAttackPower;
                    wa.ProjectileCount = weaponData.baseProjectileCount;
                    wa.projectileLimit = weaponData.projectileLimit;
                    wa.CoolTime = weaponData.baseCoolTime;
                    wa.Penetration = weaponData.basePenetration;
                    wa.Critical = weaponData.baseCritical;
                    wa.Knockback = weaponData.baseKnockback;

                    Debug.Log("기본 세팅 초기화");
                }
                else
                {
                    wa.AttackPower += weaponData.levelData[level-1].additionalAttackPower;
                    wa.ProjectileCount += weaponData.levelData[level-1].additionalProjectiles;
                    wa.projectileLimit = weaponData.projectileLimit;
                    wa.CoolTime += weaponData.levelData[level - 1].cooldownChange;
                    wa.Penetration += weaponData.levelData[level - 1].additionalPenetration;
                    wa.Critical = weaponData.baseCritical;
                    wa.Knockback = weaponData.baseKnockback;

                    Debug.Log("기존 정보에서 강화");
                    
                }
                    

                player.UpdateWeaponInfo(weaponData,level - 1);
                

                Debug.Log($"{weaponData.weaponType}을 업데이트");
                Debug.Log($"{weaponData.weaponType}의 레벨 {level}");
                if (count == 0)
                {
                    Debug.Log(wa.AttackPower);
                    Debug.Log(wa.ProjectileCount);
                    break;
                }
                
            }
        }
        
    }

    private void Start()
    {

        //InitializeWeapon();
        StartCoroutine(TryRegisterCoroutine());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // 처음에는 whip 무기만 존재
        UpdateWeaponPrefab(wd, 1);
        firstUpdate(wd2);
        firstUpdate(wd3);
        firstUpdate(wd4);
        //player.UpdateWeaponInfo(player.FindWeaponInfo(WeaponType.Whip),0);
        

    }

    public void firstUpdate(WeaponData weaponData)
    {
        foreach (var prefab in prefabs)
        {
            wt = prefab.GetComponent<Weapon_All>().weaponType;

            if (wt == weaponData.weaponType)
            {
                count--;
                wa = prefab.GetComponent<Weapon_All>();                
                    wa.AttackPower = weaponData.baseAttackPower;
                    wa.ProjectileCount = weaponData.baseProjectileCount;
                    wa.projectileLimit = weaponData.projectileLimit;
                    wa.CoolTime = weaponData.baseCoolTime;
                    wa.Penetration = weaponData.basePenetration;
                    wa.Critical = weaponData.baseCritical;
                    wa.Knockback = weaponData.baseKnockback;




            }
        }
    }

    public void InitializeWeapon()
    {
        // 모든 무기의 기본값 초기화
        foreach (var prefab in prefabs)
        {
            Weapon_All wa = prefab.GetComponent<Weapon_All>();
            if (wa != null)
            {
                wa.AttackPower = 0;
                wa.ProjectileCount = 0;
                wa.projectileLimit = 0;
                wa.CoolTime = 0;
                wa.Penetration = 0;
                wa.Critical = 0;
                wa.Knockback = 0;
            }
        }
    }

    IEnumerator TryRegisterCoroutine()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // 한 프레임 대기
        }
        GameManager.Instance.prefabManager = this;
    }


}
