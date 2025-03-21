using UnityEngine;

public class Exp : MonoBehaviour
{
    public float EXP = 1;
  
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌 감지");
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.PlusExp(EXP);
                Debug.Log("경험치 증가");
            }
            Destroy(gameObject);
        }
    }
}
