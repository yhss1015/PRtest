using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VampireSurvival.ItemSystem;

public class Slash : Weapon_All
{
    [Header("Slash 스탯")]
    public float enabled_delay =0.1f;
    public float destroy_delay;
    public ItemManager itemManager;

    [SerializeField]
    private bool firstSound = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("몬스터 맞음");
            if (firstSound == true) // 몬스터 피격 소리가 겹치면 매우 크므로 검기 하나당 1번만 실행
            {
                SoundManager.Instance.slashAttackSound();
                firstSound = false;
            }

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

                //Monster monster = collision.GetComponent<Monster>() ? collision.GetComponent<Monster>() : collision.GetComponent<EventMonster>();
                //monster.TakeDamage(AttackPower);

            }
        }
    }


    // 히트 되는 타이밍을 조절하기위해 collider를 일정 시간 뒤에 활성화 함.
    private void Start()
    {/*
        itemManager = FindAnyObjectByType<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager를 찾을 수 없습니다!");
            return;
        }

        // weaponDataList에서 Whip 무기 찾기
        WeaponData whipWeapon = null;
        foreach (var weapon in itemManager.weaponDataList)
        {
            if (weapon.weaponType == WeaponType.Whip)
            {
                whipWeapon = weapon;
                break;  // 첫 번째 Whip 무기를 찾으면 반복 종료
            }
        }

        if (whipWeapon != null)
        {
            AttackPower = whipWeapon.baseAttackPower;
            //delay = whipWeapon.baseCoolTime;
            Debug.Log($"Whip 무기 적용: 공격력({AttackPower}))");
        }
        else
        {
            Debug.LogError("weaponDataList에서 Whip 타입 무기를 찾을 수 없습니다!");
        }
        */
        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
        StartCoroutine(OnCollider(polygon, enabled_delay, true));
        if (destroy_delay != 0)
        {
            StartCoroutine(OnCollider(polygon, destroy_delay, false));
        }
    }

    IEnumerator OnCollider(PolygonCollider2D polygon, float delay, bool isenable)
    {
        yield return new WaitForSeconds(delay);
        polygon.enabled = isenable;
    }



}