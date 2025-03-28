using System.Collections.Generic;
using UnityEngine;
using VampireSurvival.ItemSystem;

#region EquippedItem<T> 클래스
// 각 아이템(무기 또는 장신구)와 해당 아이템의 현재 레벨을 함께 관리하는 제네릭 클래스
[System.Serializable]
public class EquippedItem<T>
{
    public T itemData;      // 예: WeaponData 또는 AccessoryData
    public int currentLevel; // 초기값은 1, 무기는 최대 8, 장신구는 각 에셋의 maxLevel

    public EquippedItem(T data)
    {
        itemData = data;
        currentLevel = 0;
    }
}
#endregion

public class PlayerInventory : MonoBehaviour
{
    [Header("장착된 무기 (최대 6개)")]
    // 무기는 List 형태로 관리 (최대 6개)
    public List<EquippedItem<WeaponData>> equippedWeapons = new List<EquippedItem<WeaponData>>();

    [Header("장착된 장신구 (최대 6개)")]
    // 장신구도 List 형태로 관리 (최대 6개)
    public List<EquippedItem<AccessoryData>> equippedAccessories = new List<EquippedItem<AccessoryData>>();

    [Header("연동 컴포넌트")]
    // 무기 디스플레이 컨트롤러 (무기 교체 시 외형 업데이트에 사용)
    public WeaponDisplayController weaponDisplayController;

    // 세트 효과 매핑 리스트 (앞서 생성한 SetAwakeningMappingList 에셋)
    public SetAwakeningMappingList setAwakeningMappingList;

    #region 무기 레벨업 및 각성 처리
    // 특정 무기(인덱스 기준)를 레벨업 시키고, 최대 레벨이면 각성 조건을 체크하는 함수
    public void LevelUpWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= equippedWeapons.Count)
        {
            Debug.LogWarning("잘못된 무기 인덱스");
            return;
        }

        EquippedItem<WeaponData> weaponItem = equippedWeapons[weaponIndex];
        if (weaponItem.currentLevel < 8)
        {
            weaponItem.currentLevel++;
            Debug.Log("무기 레벨 업! 현재 무기 레벨: " + weaponItem.currentLevel);
        }
        else
        {
            Debug.Log("무기가 이미 최대 레벨입니다. 각성 조건 확인...");
            CheckAndAwakenWeapon(weaponIndex);
        }
    }

    public int FIndIndex(WeaponData weapon)
    {
        Debug.Log(equippedWeapons.Count);
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            if (weapon.weaponType == equippedWeapons[i].itemData.weaponType)
            {
                return i;
            }
        }
        return 0;
    }

    public bool FIndWeapon(WeaponData weapon)
    {
        for(int i =0; i<equippedWeapons.Count;i++)
        {
            if(weapon.weaponType == equippedWeapons[i].itemData.weaponType)
            {
                return true;
            }
        }
        return false;
    }

    // 특정 무기(인덱스)의 각성 조건을 체크하는 함수
    public void CheckAndAwakenWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= equippedWeapons.Count)
        {
            Debug.LogWarning("잘못된 무기 인덱스");
            return;
        }

        EquippedItem<WeaponData> weaponItem = equippedWeapons[weaponIndex];

        // 무기가 최대 레벨(8)이어야 각성 가능
        if (weaponItem.currentLevel < 8)
        {
            Debug.Log("무기의 레벨이 아직 최대가 아닙니다.");
            return;
        }

        // 세트 효과 매핑 리스트를 순회하며 조건 확인 (예: Whip + HollowHeart → Whip2)
        if (setAwakeningMappingList != null)
        {
            foreach (var mapping in setAwakeningMappingList.mappings)
            {
                if (weaponItem.itemData.weaponType == mapping.originalWeaponType)
                {
                    // 착용된 장신구 중 조건에 맞는 것이 있는지 확인
                    foreach (var acc in equippedAccessories)
                    {
                        if (acc.itemData.accessoryType == mapping.requiredAccessoryType)
                        {
                            if (mapping.awakenedWeapon != null)
                            {
                                // 각성 처리: 무기 데이터를 awakenedWeapon으로 교체
                                weaponItem.itemData = mapping.awakenedWeapon;
                                if (weaponDisplayController != null)
                                {
                                    weaponDisplayController.SetWeaponData(weaponItem.itemData);
                                }
                                Debug.Log("무기가 각성되었습니다!");
                                return;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("각성 조건을 만족하지 않습니다.");
    }
    #endregion

    #region 장신구 레벨업 처리
    // 특정 장신구(인덱스 기준)를 레벨업 시키는 함수
    public void LevelUpAccessory(int accessoryIndex)
    {
        if (accessoryIndex < 0 || accessoryIndex >= equippedAccessories.Count)
        {
            Debug.LogWarning("잘못된 장신구 인덱스");
            return;
        }

        EquippedItem<AccessoryData> accItem = equippedAccessories[accessoryIndex];
        if (accItem.currentLevel < accItem.itemData.maxLevel)
        {
            accItem.currentLevel++;
            Debug.Log("장신구 레벨 업! 현재 장신구 레벨: " + accItem.currentLevel);
        }
        else
        {
            Debug.Log("장신구가 이미 최대 레벨입니다.");
        }
    }
    #endregion

    #region 신규 아이템 획득 관련 (예시)
    // 신규 무기 획득 시 호출: 보물상자 등에서 사용
    public void AddNewWeapon(WeaponData newWeaponData)
    {
        if (equippedWeapons.Count >= 6)
        {
            Debug.Log("무기 최대 보유 개수 도달");
            return;
        }
        equippedWeapons.Add(new EquippedItem<WeaponData>(newWeaponData));
        Debug.Log("신규 무기 획득");
    }

    // 신규 장신구 획득 시 호출
    public void AddNewAccessory(AccessoryData newAccessoryData)
    {
        if (equippedAccessories.Count >= 6)
        {
            Debug.Log("장신구 최대 보유 개수 도달");
            return;
        }
        equippedAccessories.Add(new EquippedItem<AccessoryData>(newAccessoryData));
        Debug.Log("신규 장신구 획득");
    }
    #endregion
}
