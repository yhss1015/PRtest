using UnityEngine;
using VampireSurvival.ItemSystem;

public class PrefabManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public WeaponData wd;
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
                if(level==0)
                {
                    wa.AttackPower = weaponData.baseAttackPower;
                    wa.ProjectileCount = weaponData.baseProjectileCount;
                    wa.projectileLimit = weaponData.projectileLimit;
                    wa.CoolTime = weaponData.baseCoolTime;
                    wa.Penetration = weaponData.basePenetration;
                    wa.Critical = weaponData.baseCritical;
                    wa.Knockback = weaponData.baseKnockback;
                }
                else
                {
                    wa.AttackPower += weaponData.levelData[level].additionalAttackPower;
                    wa.ProjectileCount += weaponData.levelData[level].additionalProjectiles;
                    wa.projectileLimit = weaponData.projectileLimit;
                    wa.CoolTime += weaponData.levelData[level].cooldownChange;
                    wa.Penetration += weaponData.levelData[level].additionalPenetration;
                    wa.Critical = weaponData.baseCritical;
                    wa.Knockback = weaponData.baseKnockback;
                }
                    

                player.UpdateWeaponInfo(weaponData,level);

                Debug.Log($"{weaponData.weaponType}을 업데이트");
                if (count == 0)
                {
                    break;
                }
            }
        }
    }

    private void Start()
    {

        InitializeWeapon();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // 처음에는 whip 무기만 존재
        player.UpdateWeaponInfo(player.FindWeaponInfo(WeaponType.Whip),0);

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

    

}
