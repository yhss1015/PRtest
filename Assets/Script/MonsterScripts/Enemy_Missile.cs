using UnityEngine;

public class Enemy_Missile : MonoBehaviour
{
    public float Speed = 3f;
    public float Damage = 10f;
    public float lifetime = 10f;
    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction; // 화살 방향을 올바르게 설정
    }

    void Update()
    {
        transform.position += (Vector3)direction * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
