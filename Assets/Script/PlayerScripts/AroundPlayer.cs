using UnityEngine;

public class AroundPlayer : MonoBehaviour
{
    private Transform player;  // 플레이어 Transform
    public float radius = 1f; // 원의 반지름
    public float speed = 2f;  // 회전 속도

    private float angle = 0f; // 현재 각도

    void Start()
    {
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        Orbit();
    }

    void Orbit()
    {
        if (player == null) return;

        angle += speed * Time.deltaTime; // 각도 증가 (속도 조절)

        // 새로운 위치 계산 (원의 방정식: x = r*cos(θ), y = r*sin(θ))
        float x = player.position.x + Mathf.Cos(angle) * radius;
        float y = player.position.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
