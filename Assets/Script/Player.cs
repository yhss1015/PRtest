using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100;
    public float curHp = 100;
    public float speed = 5;
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
        float speedX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float speedY = Input.GetAxis("Vertical") * (speed / 2) * Time.deltaTime;
        transform.Translate(speedX, speedY, 0);
    }
}
