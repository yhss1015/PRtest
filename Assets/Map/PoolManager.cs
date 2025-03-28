using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    [SerializeField]
    public List<GameObject>[] pools;

    private void Awake()
    {
        
    }
    private void Start()
    {
        prefabs = Resources.LoadAll<GameObject>("Monster/");
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
        StartCoroutine(TryRegisterCoroutine()); 
    }

    public GameObject GetPrefab(int index)
    {
        GameObject select = null;

        /*
        선택한 풀의 비활성화 된(사용중이 아닌)게임 오브젝트에 접근,
        발견하면 select 변수에 할당
        */
        if(index >= pools.Length)
        {
            index = pools.Length-1;
        }
        //out of range 나올 수 있음 index <- 몬스터 레벨 증가(종류 변경)
        foreach (GameObject obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        /*
        찾지 못하면 새롭게 생성하고 select 변수에 할당
        */
        if (!select)
        {
            select = Instantiate(prefabs[index], GetSpawnPos(GameManager.Instance.player.transform.position), Quaternion.identity);
            pools[index].Add(select);

            Monster monster = select.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Initialize_Index(index);
            }
        }


        return select;
    }

    public void ReturnPool(GameObject obj, int index)
    {
        if (index >= pools.Length || index < 0) return;
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj);
        }
        obj.SetActive(false);
    }

    IEnumerator TryRegisterCoroutine()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // 한 프레임 대기
        }
        GameManager.Instance.pool = this;
    }

    public Vector3 GetSpawnPos(Vector3 playerPos)
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) return Vector3.zero;

        Vector3 spawnPos = playerPos;

        float camHeight = 2f * mainCam.orthographicSize;
        float camWidth = camHeight * mainCam.aspect;

        float spawnMargin = 2f;
        float spawnDistance = 5f;

        int randomSide = Random.Range(0, 4); //방향 랜덤설정

        switch (randomSide)
        {
            case 0: // 위쪽
                spawnPos += new Vector3(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 + spawnMargin, 0); break;
            case 1: // 아래쪽
                spawnPos += new Vector3(Random.Range(-camWidth / 2, camWidth / 2), -camHeight / 2 - spawnMargin, 0); break;
            case 2: // 왼쪽
                spawnPos += new Vector3(-camWidth / 2 - spawnMargin, Random.Range(-camHeight / 2, camHeight / 2), 0); break;
            case 3: // 오른쪽
                spawnPos += new Vector3(camWidth / 2 + spawnMargin, Random.Range(-camHeight / 2, camHeight / 2), 0); break;
        }

        // 플레이어와 일정 거리 유지
        if (Vector3.Distance(spawnPos, playerPos) < spawnDistance)
        {
            spawnPos += (spawnPos - playerPos).normalized * spawnDistance;
        }
/*
        // 빈 GameObject를 생성하여 위치 설정
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.position = spawnPos;
*/

        return spawnPos;
    }
}
