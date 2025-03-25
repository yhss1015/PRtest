using UnityEngine;
using VampireSurvival.ItemSystem;

public class Weapon_All : MonoBehaviour
{
    public WeaponData awakenedVersion;

    [Header("무기 종류 및 외형")]
    public WeaponType weaponType;
    public Sprite itemSprite;  // PNG 할당

    [Header("무기 스탯")]
    public int AttackPower;
    public int ProjectileCount;
    public int projectileLimit;   // 채찍: 30, 지팡이: 60 등
    public float CoolTime;   // 채찍: 1.35, 지팡이: 1.2 등
    public int Penetration;     // -1: 무제한, 아니면 지정 값
    public float Critical;      // 치명타 확률(%)
    public float Knockback;      // 넉백

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
