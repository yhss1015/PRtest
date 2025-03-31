using UnityEngine;

namespace VampireSurvival.ItemSystem
{
    public enum WeaponType { Whip, MagicWand, KingVible, NinjaStar }

    [System.Serializable]
    public class WeaponLevelData
    {
        [Tooltip("해당 레벨에서 추가되는 투사체 수")]
        public int additionalProjectiles;
        [Tooltip("해당 레벨에서 추가되는 공격력")]
        public int additionalAttackPower;
        [Tooltip("해당 레벨에서 추가되는 공격 범위(%)")]
        public float additionalRangePercent;
        [Tooltip("해당 레벨에서 변경되는 쿨타임 (음수면 단축)")]
        public float cooldownChange;
        [Tooltip("해당 레벨에서 추가되는 관통 횟수")]
        public int additionalPenetration;
    }

    [CreateAssetMenu(menuName = "VampireSurvival/Weapon Data", fileName = "WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public WeaponData awakenedVersion;

        [Header("무기 종류 및 외형")]
        public WeaponType weaponType;
        public Sprite itemSprite;  // PNG 할당

        [Header("기본 스탯")]
        public int baseAttackPower = 0;
        public int baseProjectileCount = 0;
        public int projectileLimit = 0;   // 채찍: 30, 지팡이: 60 등
        public float baseCoolTime = 0;   // 채찍: 1.35, 지팡이: 1.2 등
        public int basePenetration = 0;     // -1: 무제한, 아니면 지정 값
        public float baseCritical = 0;      // 치명타 확률(%)
        public float baseKnockback = 0;      // 넉백

        [Header("레벨 데이터 (레벨 1 ~ 8)")]
        [Tooltip("인덱스 0: 레벨1 (초기 상태), 인덱스 1: 레벨2, ... 인덱스 7: 레벨8")]
        public WeaponLevelData[] levelData;
        public string[] levelDescriptions;
    }
    
}
