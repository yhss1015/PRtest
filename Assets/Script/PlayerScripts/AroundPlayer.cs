using UnityEngine;

public class AroundPlayer : Weapon_All
{

   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("회전 공격 맞음");
            

            Monster monster = collision.GetComponent<Monster>();
            monster.TakeDamage(AttackPower);

        }
    }

    void Start()
    {
        
       
    }

   

    
}
