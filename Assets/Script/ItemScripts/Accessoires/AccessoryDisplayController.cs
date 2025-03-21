using UnityEngine;
using VampireSurvival.ItemSystem; // AccessoryData가 있는 네임스페이스

public class AccessoryDisplayController : MonoBehaviour
{
    public Sprite itemSprite; // 장신구 외형을 저장할 필드

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemSprite != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }

    // AccessoryData 에셋을 전달받아 스프라이트를 적용하는 함수
    public void SetAccessoryData(AccessoryData accessoryData)
    {
        if (accessoryData == null) return;
        itemSprite = accessoryData.itemSprite;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }
}
