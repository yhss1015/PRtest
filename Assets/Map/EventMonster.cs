using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EventMonster : Monster
{
    public int spawnNum;
    public int repeatNum = 1;
    public bool isNormalMonster = false;

    public void Start()
    {
        index = -999;
        SettingTarget();

        Mob_Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        original = sr.color; // 몬스터의 기본 색상 저장 
    }

    public void SettingTarget()
    {
        if (isNormalMonster)
        {
            GameObject player = GameManager.Instance.player.gameObject;
            Speed = 6f;
            float targetDistance = 40f;
            Vector3 targetPos = player.transform.position + (player.transform.position - transform.position).normalized * targetDistance;
            GameObject targetPoint = new GameObject("EventTarget") { transform = { position = targetPos } };
            
            target = targetPoint;
            Destroy(gameObject, 5f);
            Destroy(targetPoint, 6f);
        }
        else { 
        }
    }
    private IEnumerator TmpCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject player = GameManager.Instance.player.gameObject;

            float targetDistance = 20f;
            Vector3 targetPos = player.transform.position + (player.transform.position - transform.position).normalized * targetDistance;
            GameObject targetPoint = new GameObject("EventTarget") { transform = { position = targetPos } };
            Destroy(targetPoint, 10f);
        }
    }
    


}
