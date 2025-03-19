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

    }

    public GameObject GetPrefab(int index)
    {
        GameObject select = null;

        /*
        ������ Ǯ�� ��Ȱ��ȭ ��(������� �ƴ�)���� ������Ʈ�� ����,
        �߰��ϸ� select ������ �Ҵ�
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
        ã�� ���ϸ� ���Ӱ� �����ϰ� select ������ �Ҵ�
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
            yield return null; // �� ������ ���
        }
        GameManager.Instance.pool = this;
    }
}
