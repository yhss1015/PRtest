using UnityEngine;
using VampireSurvival.ItemSystem; // GemData가 있는 네임스페이스

public class GemDisplayController : MonoBehaviour
{
    public Sprite itemSprite; // 보석 외형을 저장할 필드

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemSprite != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }

    // GemData 에셋을 전달받아 스프라이트를 적용하는 함수
    public void SetGemData(GemData gemData)
    {
        if (gemData == null) return;
        itemSprite = gemData.itemSprite;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }
}
