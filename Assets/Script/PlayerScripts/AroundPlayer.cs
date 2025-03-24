using UnityEngine;

public class AroundPlayer : MonoBehaviour
{

    public float power = 5f; // 원의 공격력


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("회전 공격 맞음");
            

            Monster monster = collision.GetComponent<Monster>();
            monster.TakeDamage(power);

        }
    }

    void Start()
    {
        
       
    }

   

    
}
