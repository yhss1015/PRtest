using UnityEngine;
using VampireSurvival.ItemSystem; // WeaponData가 있는 네임스페이스

public class WeaponDisplayController : MonoBehaviour
{
    public Sprite itemSprite; // 무기 외형을 저장할 필드

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemSprite != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }

    // WeaponData 에셋을 전달받아 스프라이트를 적용하는 함수
    public void SetWeaponData(WeaponData weaponData)
    {
        if (weaponData == null) return;
        itemSprite = weaponData.itemSprite;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }
}
