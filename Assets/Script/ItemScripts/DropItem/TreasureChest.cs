using UnityEngine;
using VampireSurvival.ItemSystem;  // WeaponData, AccessoryData 등이 정의된 네임스페이스

public class TreasureChest : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
            return;

        if (collision.CompareTag("Player"))
        {
            // PlayerInventory 컴포넌트 가져오기
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                // 무기(0) 또는 장신구(1) 카테고리 중 랜덤 선택
                int choice = Random.Range(0, 2);

                // 선택된 카테고리의 아이템이 없으면 다른 카테고리로 전환
                if (choice == 0 && inventory.equippedWeapons.Count == 0)
                {
                    if (inventory.equippedAccessories.Count > 0)
                        choice = 1;
                    else
                    {
                        Debug.Log("장착된 아이템이 없습니다.");
                        isCollected = true;
                        Destroy(gameObject);
                        return;
                    }
                }
                else if (choice == 1 && inventory.equippedAccessories.Count == 0)
                {
                    if (inventory.equippedWeapons.Count > 0)
                        choice = 0;
                    else
                    {
                        Debug.Log("장착된 아이템이 없습니다.");
                        isCollected = true;
                        Destroy(gameObject);
                        return;
                    }
                }

                if (choice == 0) // 무기 카테고리 선택
                {
                    int index = Random.Range(0, inventory.equippedWeapons.Count);
                    var weaponItem = inventory.equippedWeapons[index];
                    // 무기 레벨이 최대(8) 미만이면 레벨업
                    if (weaponItem.currentLevel < 8)
                    {
                        inventory.LevelUpWeapon(index);
                    }
                    else
                    {
                        // 무기가 최대 레벨이면 세트 효과(각성) 조건 체크
                        inventory.CheckAndAwakenWeapon(index);
                    }
                }
                else // 장신구 카테고리 선택
                {
                    int index = Random.Range(0, inventory.equippedAccessories.Count);
                    var accessoryItem = inventory.equippedAccessories[index];
                    // 장신구는 최대 레벨까지 레벨업(최대 레벨이면 아무 작업도 하지 않음)
                    if (accessoryItem.currentLevel < accessoryItem.itemData.maxLevel)
                    {
                        inventory.LevelUpAccessory(index);
                    }
                    else
                    {
                        Debug.Log("장신구가 이미 최대 레벨입니다.");
                    }
                }
            }

            isCollected = true;
            Destroy(gameObject);
        }
    }
}
