using System.Collections;
using UnityEngine;

public class Reposition : MonoBehaviour
{

    private Coroutine respawnCoroutine; // 재소환 코루틴 저장용

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = MapManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;
        /*
                float diffX = Mathf.Abs(playerPos.x - myPos.x);
                float diffY = Mathf.Abs(playerPos.y - myPos.y);

                Vector3 playerDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
                float dirX = playerDir.x < 0 ? -1 : 1;
                float dirY = playerDir.y < 0 ? -1 : 1;
        */
        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);
        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                /*if (!IsEnemyOnScreen())
                {
                    transform.position = GameManager.Instance.pool.GetSpawnPos(GameManager.Instance.player.transform.position).position;
                }*/

                break;
        }

    }
    public Vector3 GetDir()
    {
        Vector3 playerPos = MapManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        //플레이어랑 몬스터가 조금이라도 축이 다르면 대각선으로 나옴
        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);
        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        return new Vector3(dirX, dirY, 0);

    }
    private void OnBecameInvisible()
    {
        if (transform.tag != "Enemy" || !this.gameObject.activeInHierarchy) return;
        // 화면 밖으로 나가면 재소환 타이머 시작
        if (respawnCoroutine == null)
        {
            respawnCoroutine = StartCoroutine(RespawnAfterDelay(2f));
        }
    }

    private void OnBecameVisible()
    {
        if (transform.tag != "Enemy" || !this.gameObject.activeInHierarchy) return;
        // 다시 화면 안으로 들어오면 타이머 취소
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (!IsEnemyOnScreen())
            {
                /*transform.position = GameManager.Instance.pool.GetSpawnPos(GameManager.Instance.player.transform.position);*/
                Vector3 playerDir = GetDir(); // 플레이어 방향 가져오기
                float spawnDistance = 40f; // Ground와 같은 거리

                // 플레이어 방향으로 이동
                transform.Translate(new Vector3(playerDir.x * spawnDistance, playerDir.y * spawnDistance, 0));

                // 스폰 포인트도 반영 (보정 필요할 수도 있음)
                transform.position = GameManager.Instance.pool.GetSpawnPos(MapManager.Instance.player.transform.position);
            }
            else
            {
                break;
            }
        }
        

        respawnCoroutine = null; // 코루틴 리셋
    }

    bool IsEnemyOnScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z > 0;
    }
}
