using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100;
    public float curHp = 100;
    public float speedX = 5;
    public float speedY = 4;
    public float attack = 10;



    void Start()
    {

    }


    void Update()
    {
        PMove();
        
    }

    void PMove()
    {
        float CurSpeedX = Input.GetAxis("Horizontal") * speedX * Time.deltaTime;
        float CurSpeedY = Input.GetAxis("Vertical") * speedY * Time.deltaTime;
        transform.Translate(CurSpeedX, CurSpeedY, 0);
    }

    void TakeDamage(float dmg)
    {
        curHp -= dmg;

        if(curHp<=0)
        {
            Debug.Log("플레이어 사망");

        }
    }
    
    void PlusHp(float amount)
    {
        curHp += amount;
        if(curHp>maxHp)
        {
            curHp = maxHp;
        }
    }
}
