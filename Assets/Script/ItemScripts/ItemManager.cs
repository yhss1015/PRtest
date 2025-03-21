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

    // 3개의 랜덤 아이템(무기/장신구)을 중복 없이 생성하는 함수
    public void SpawnRandomWeaponOrAccessory(Vector3 position)
    {
        float spacing = 1.0f;  // 원하는 간격

        // 각 카테고리별로 이미 선택된 인덱스를 저장할 리스트
        List<int> chosenWeaponIndices = new List<int>();
        List<int> chosenAccessoryIndices = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            // 가운데 기준 좌우 배치: i=0 -> -spacing, i=1 -> 0, i=2 -> +spacing
            Vector3 spawnPos = position + new Vector3((i - 1) * spacing, 0, 0);

            // 무기(0) 또는 장신구(1) 카테고리 랜덤 선택
            int category = Random.Range(0, 2);
            if (category == 0) // 무기
            {
                if (weaponDataList.Length == 0 || weaponPrefab == null)
                {
                    Debug.LogWarning("Weapon data 또는 Prefab이 할당되지 않았습니다.");
                    continue;
                }
                // 아직 선택되지 않은 무기 인덱스 목록 생성
                List<int> availableWeaponIndices = new List<int>();
                for (int idx = 0; idx < weaponDataList.Length; idx++)
                {
                    if (!chosenWeaponIndices.Contains(idx))
                        availableWeaponIndices.Add(idx);
                }
                if (availableWeaponIndices.Count == 0)
                {
                    Debug.LogWarning("더 이상 중복되지 않은 무기가 없습니다.");
                    continue;
                }
                int randomIndex = availableWeaponIndices[Random.Range(0, availableWeaponIndices.Count)];
                chosenWeaponIndices.Add(randomIndex);
                WeaponData selectedWeapon = weaponDataList[randomIndex];

                GameObject itemInstance = Instantiate(weaponPrefab, spawnPos, Quaternion.identity);
                WeaponDisplayController displayController = itemInstance.GetComponent<WeaponDisplayController>();
                if (displayController != null)
                {
                    displayController.SetWeaponData(selectedWeapon);
                }
            }
            else // 장신구
            {
                if (accessoryDataList.Length == 0 || accessoryPrefab == null)
                {
                    Debug.LogWarning("Accessory data 또는 Prefab이 할당되지 않았습니다.");
                    continue;
                }
                List<int> availableAccessoryIndices = new List<int>();
                for (int idx = 0; idx < accessoryDataList.Length; idx++)
                {
                    if (!chosenAccessoryIndices.Contains(idx))
                        availableAccessoryIndices.Add(idx);
                }
                if (availableAccessoryIndices.Count == 0)
                {
                    Debug.LogWarning("더 이상 중복되지 않은 장신구가 없습니다.");
                    continue;
                }
                int randomIndex = availableAccessoryIndices[Random.Range(0, availableAccessoryIndices.Count)];
                chosenAccessoryIndices.Add(randomIndex);
                AccessoryData selectedAccessory = accessoryDataList[randomIndex];

                GameObject itemInstance = Instantiate(accessoryPrefab, spawnPos, Quaternion.identity);
                AccessoryDisplayController displayController = itemInstance.GetComponent<AccessoryDisplayController>();
                if (displayController != null)
                {
                    displayController.SetAccessoryData(selectedAccessory);
                }
            }
        }
    }
}
