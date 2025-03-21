using UnityEngine;
using VampireSurvival.ItemSystem;  // WeaponData, AccessoryData, AccessoryType 등이 정의된 네임스페이스

public class PlayerInventory : MonoBehaviour
{
    // 현재 장착된 무기와 장신구 (Inspector에서 연결)
    public WeaponData equippedWeapon;
    public AccessoryData equippedAccessory;

    // 무기의 현재 레벨 (실제 게임 로직에 따라 업데이트)
    public int currentWeaponLevel = 1;

    // 무기의 외형을 업데이트할 때 사용할 무기 디스플레이 컨트롤러 (플레이어의 무기 오브젝트에 붙어있음)
    public WeaponDisplayController weaponDisplayController;

    // 보물상자 획득 등의 이벤트 시 호출되는 함수: 각성 조건 확인 및 무기 업그레이드
    public void CheckAndAwakenWeapon()
    {
        if (equippedWeapon == null || equippedAccessory == null)
        {
            Debug.LogWarning("장착된 무기나 장신구가 없습니다.");
            return;
        }

        // 예시 조건: 무기의 현재 레벨이 8 이상이고, 장신구가 세트 효과에 해당하는 타입일 때 각성 가능
        if (currentWeaponLevel >= 8 && equippedAccessory.accessoryType == AccessoryType.HollowHeart)
        {
            // 각성 무기 정보가 WeaponData의 awakenedVersion 필드에 할당되어 있어야 함
            if (equippedWeapon.awakenedVersion != null)
            {
                // 각성 무기로 업그레이드: 무기 데이터를 교체하고, 무기 디스플레이를 업데이트합니다.
                equippedWeapon = equippedWeapon.awakenedVersion;
                if (weaponDisplayController != null)
                {
                    weaponDisplayController.SetWeaponData(equippedWeapon);
                }
                Debug.Log("무기가 각성되었습니다!");
            }
            else
            {
                Debug.LogWarning("각성 무기 정보가 설정되지 않았습니다.");
            }
        }
        else
        {
            Debug.Log("각성 조건을 만족하지 않습니다.");
        }
    }
}
