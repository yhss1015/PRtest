using UnityEngine;

public class Enemy_Missile : MonoBehaviour
{
    public GameObject target; 
    public float Speed = 3f;
    public float Damage = 10f;
    public float lifetime = 10;
    Vector2 direction;
    //Vector2 dirNo;

    void Start()
    {
      Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * Speed * Time.deltaTime);
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
