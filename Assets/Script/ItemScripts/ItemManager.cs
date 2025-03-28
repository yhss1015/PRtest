using System.Collections.Generic;
using UnityEngine;
using VampireSurvival.ItemSystem; // WeaponData, AccessoryData 등이 정의된 네임스페이스

public class ItemManager : MonoBehaviour
{
    [Header("아이템 데이터 에셋")]
    public WeaponData[] weaponDataList;
    public AccessoryData[] accessoryDataList;

    [Header("아이템 Prefab")]
    public GameObject weaponPrefab;
    public GameObject accessoryPrefab;

    // 플레이어의 인벤토리를 참조하기 위한 필드 (다중 장착을 관리하는 PlayerInventory로 가정)
    public PlayerInventory playerInventory;

    public void LevelUpPlayerWeapon(int weaponIndex)
    {
        PlayerInventory inventory = playerInventory;  // ItemManager 내에서 미리 할당된 PlayerInventory 참조

        // playerInventory는 이미 ItemManager.cs에서 플레이어 인벤토리를 참조하는 public 필드라고 가정
        if (playerInventory != null)
        {
            // PlayerInventory에서 해당 무기를 레벨업 (내부에서 각성 체크 포함)
            playerInventory.LevelUpWeapon(weaponIndex);

            // 최신 무기 데이터를 가져옴
            WeaponData updatedWeapon = playerInventory.equippedWeapons[weaponIndex].itemData;

            // Player.cs에 있는 무기 정보 업데이트 함수 호출
            // (예: 무기 프리팹에 적용할 스탯 갱신)
            if (GameManager.Instance.player != null)
            {
                GameManager.Instance.player.UpdateWeaponInfo(updatedWeapon);
            }
        }
    }

    // 랜덤 선택 결과를 담을 데이터 클래스
    public class RandomItemData
    {
        // WeaponData 또는 AccessoryData (두 타입 모두 ScriptableObject를 상속받음)
        public ScriptableObject itemData;
        // 해당 아이템이 플레이어 인벤토리에 이미 있는지 여부
        public bool isEquipped;
    }

    // 3개의 랜덤 아이템(무기/장신구)을 중복 없이 생성하는 함수
    public List<RandomItemData> GetRandomItemData(PlayerInventory inventory)
    {
        List<RandomItemData> allItems = new List<RandomItemData>();

        // 무기 데이터 처리 (무기 최대 레벨 8)
        foreach (WeaponData weapon in weaponDataList)
        {
            bool isEquipped = inventory.equippedWeapons.Exists(e => e.itemData == weapon);
            if (isEquipped)
            {
                EquippedItem<WeaponData> equippedWeapon = inventory.equippedWeapons.Find(e => e.itemData == weapon);
                // 이미 장착되어 있고 최대 레벨이면 건너뒤기
                if (equippedWeapon.currentLevel >= 8)
                    continue;
            }
            RandomItemData rid = new RandomItemData();
            rid.itemData = weapon;
            rid.isEquipped = isEquipped;
            allItems.Add(rid);
        }

        // 장신구 데이터 처리
        foreach (AccessoryData accessory in accessoryDataList)
        {
            bool isEquipped = inventory.equippedAccessories.Exists(e => e.itemData == accessory);
            if (isEquipped)
            {
                EquippedItem<AccessoryData> equippedAccessory = inventory.equippedAccessories.Find(e => e.itemData == accessory);
                // 이미 장착되어 있고 최대 레벨이면 건너뛰기
                if (equippedAccessory.currentLevel >= accessory.maxLevel)
                    continue;
            }
            RandomItemData rid = new RandomItemData();
            rid.itemData = accessory;
            rid.isEquipped = isEquipped;
            allItems.Add(rid);
        }

        // 중복 없이 3개를 랜덤 선택
        List<RandomItemData> selectedItems = new List<RandomItemData>();
        List<RandomItemData> tempList = new List<RandomItemData>(allItems);
        for (int i = 0; i < 3 && tempList.Count > 0; i++)
        {
            int index = Random.Range(0, tempList.Count);
            selectedItems.Add(tempList[index]);
            tempList.RemoveAt(index);
        }

        return selectedItems;
    }
}
