using UnityEngine;
using VampireSurvival.ItemSystem;

[System.Serializable]
public class SetAwakeningMapping // 보물상자에서 무기 각성 시 필요한 정보
{
    public WeaponType originalWeaponType;            // 기본 무기 타입 (예: Whip)
    public AccessoryType requiredAccessoryType;      // 각성에 필요한 장신구 타입 (예: HollowHeart)
    public WeaponData awakenedWeapon;                // 각성 후 무기 데이터 (예: Whip2Data)
}
