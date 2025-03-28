using UnityEngine;

public class AroundPlayer : Weapon_All
{




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("회전 공격 맞음");

            if (collision.CompareTag("Enemy"))
            {
                //Instantiate(hitEffect, transform.position, Quaternion.identity);
                if (collision.GetComponent<Monster>())
                {
                    Monster monster = collision.GetComponent<Monster>();
                    monster.TakeDamage(AttackPower);
                }
                else if (collision.GetComponent<EventMonster>())
                {
                    EventMonster monster = collision.GetComponent<EventMonster>();
                    monster.TakeDamage(AttackPower);
                }
                else if (collision.GetComponent<Ranger_Monster>())
                {
                    Ranger_Monster monster = collision.GetComponent<Ranger_Monster>();
                    monster.TakeDamage(AttackPower);
                }
                else if (collision.GetComponent<Boss>())
                {
                    Boss monster = collision.GetComponent<Boss>();
                    monster.TakeDamage(AttackPower);
                }

            }
        }

        void Start()
        {


        }

    }
}

    

