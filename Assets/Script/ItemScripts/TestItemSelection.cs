using UnityEngine;

public class TestItemSelection : MonoBehaviour
{
    // ItemManager를 Inspector에서 할당합니다.
    public ItemManager itemManager;

    // 플레이어 위치를 기준으로 아이템 생성 위치 오프셋 (원하는 대로 조정 가능)
    public Vector3 spawnPositionOffset = new Vector3(2f, 0, 0);

    // 테스트용 키 (여기서는 T키를 사용)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 플레이어 오브젝트를 찾고, 그 위치에 오프셋을 더한 곳에 아이템 생성
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && itemManager != null)
            {
                Vector3 spawnPos = player.transform.position + spawnPositionOffset;
                itemManager.SpawnRandomWeaponOrAccessory(spawnPos);
                Debug.Log("랜덤 아이템 3개 생성");
            }
            else
            {
                Debug.LogWarning("Player 또는 ItemManager가 씬에 없습니다.");
            }
        }
    }
}