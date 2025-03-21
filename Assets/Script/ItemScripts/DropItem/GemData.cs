using UnityEngine;

namespace VampireSurvival.ItemSystem
{
    public enum GemType { Gem1, Gem2, GemRed }

    [CreateAssetMenu(menuName = "VampireSurvival/Gem Data", fileName = "GemData")]
    public class GemData : ScriptableObject
    {
        [Header("보석 종류 및 외형")]
        public GemType gemType;
        public Sprite itemSprite;  // PNG 할당

        [Header("경험치")]
        [Tooltip("보석 획득 시 제공되는 경험치)")]
        public int xpAmount;
    }
}
