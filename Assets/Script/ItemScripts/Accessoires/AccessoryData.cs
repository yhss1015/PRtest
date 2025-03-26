using UnityEngine;

namespace VampireSurvival.ItemSystem
{
    public enum AccessoryType { HollowHeart, EmptyTome, Wing, Duplicator, Candelabrador, Spellbind }

    [CreateAssetMenu(menuName = "VampireSurvival/Accessory Data", fileName = "AccessoryData")]
    public class AccessoryData : ScriptableObject
    {
        [Header("장신구 종류 및 외형")]
        public AccessoryType accessoryType;
        public Sprite itemSprite;  // PNG 할당

        [Header("기본 설정")]
        [Tooltip("장신구 최대 레벨 (예: 5)")]
        public int maxLevel = 5;
        [Tooltip("매 레벨당 효과 값 (HollowHeart: 최대 체력 20% 증가, EmptyTome: 쿨타임 8% 감소 등)")]
        public float perLevelEffectValue;
    }
}
