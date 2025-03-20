using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float power;
    public float delay;

    [SerializeField]
    private bool firstSound = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Debug.Log("몬스터 맞음");
            if(firstSound==true) // 몬스터 피격 소리가 겹치면 매우 크므로 검기 하나당 1번만 실행
            {
                SoundManager.Instance.slashAttackSound();
                firstSound = false;
            }
            
        }
    }


    // 히트 되는 타이밍을 조절하기위해 collider를 일정 시간 뒤에 활성화 함.
    private void Start()
    {
        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
        StartCoroutine(OnCollider(polygon,delay));
    }

    IEnumerator OnCollider(PolygonCollider2D polygon,float delay)
    {
        yield return new WaitForSeconds(delay);
        polygon.enabled = true;
    }

}
