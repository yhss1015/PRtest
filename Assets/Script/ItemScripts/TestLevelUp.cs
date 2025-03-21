using UnityEngine;

public class ItemLevelUpOnCollision : MonoBehaviour
{
    [Header("레벨 관련 설정")]
    public int currentLevel = 1;   // 시작 레벨
    public int maxLevel = 1000;      // 최대 레벨 (원하는 값으로 조정)

    // Optional: 레벨업 시 아이템 외형에 변화(예: 크기 변경)를 주고 싶다면 사용
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 플레이어가 접촉하면 레벨 업
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentLevel < maxLevel)
            {
                currentLevel++;
                Debug.Log(gameObject.name + " leveled up to " + currentLevel);

                // 예시: 레벨이 올라갈수록 아이템의 크기를 증가시켜 시각적 변화를 줌
                transform.localScale = Vector3.one * (1f + (currentLevel - 1) * 0.2f);
            }
            else
            {
                Debug.Log(gameObject.name + " is at max level!");
            }
        }
    }
}
