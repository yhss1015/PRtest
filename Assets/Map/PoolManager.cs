using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i<pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Start()
    {
        StartCoroutine(TryRegisterCoroutine());
        prefabs = Resources.LoadAll<GameObject>("Monster/");        

    }

    public GameObject GetPrefab(int index)
    {
        GameObject select = null;

        /*
        선택한 풀의 비활성화 된(사용중이 아닌)게임 오브젝트에 접근,
        발견하면 select 변수에 할당
        */
        foreach(GameObject obj in pools[index])
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
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }


        return select;
    }
    IEnumerator TryRegisterCoroutine()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // 한 프레임 대기
        }
        GameManager.Instance.pool = this;
    }
}
