
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 5f; // 몬스터 생성 주기
    public float difficultyIncreaseInterval = 30f; // 난이도 증가 주기
    private int monsterLevel = 0; // 몬스터 강도
    public int monsterNum = 0; //몬스터 숫자
    private Stopwatch stopwatch;

    public float eventInterval = 30f; // 이벤트 주기
    GameObject[] eventMonster;
    public int eventLevel = 0;

    public int monsterMaxNum = 300;

    private void Start()
    {
        monsterNum = 8;
        
        StartCoroutine(TryRegisterCoroutine());
        
    }

    void Update()
    {
                
    }
    public void GameStart()
    {
        eventMonster = Resources.LoadAll<GameObject>("Event_0/");
        stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(SpawnMonsters());
        StartCoroutine(IncreaseDifficulty());
        StartCoroutine(StartEvent());
    }

    private IEnumerator SpawnMonsters()
    {
        while (GameManager.Instance.isRunning)
        {
            SpawnMonster();
            UnityEngine.Debug.Log(stopwatch.Elapsed.Seconds);
            
            yield return new WaitForSeconds(spawnInterval);            
            
        }
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            monsterLevel++;            
        }
    }

    private void SpawnMonster()
    {
        
        for (int i = 0; i<monsterNum + (stopwatch.Elapsed.Seconds/10); i++)
        {
            GameObject monster = GameManager.Instance.pool.GetPrefab(monsterLevel);
            if (monster != null)
            {
                // 몬스터 위치 설정 
                monster.transform.position = GameManager.Instance.pool.GetSpawnPos(GameManager.Instance.player.transform.position);
            }
        }
        
    }
    private IEnumerator IncreaseMonsterNum()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            monsterNum++;
        }
    }

    IEnumerator TryRegisterCoroutine()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // 한 프레임 대기
        }
        GameManager.Instance.spawner = this;
    }

    private IEnumerator StartEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(eventInterval);

            GameObject eventMobsParent = GameObject.Find("EventMonster") ?? new GameObject("EventMonster");


            int repeatNum = eventMonster[eventLevel].GetComponent<EventMonster>().repeatNum;

            yield return StartCoroutine(SpawnEventMonstersWithDelay(eventMonster[eventLevel], eventMobsParent, repeatNum));

            // eventLevel 증가 (최대값 초과 방지)
            eventLevel = Mathf.Min(eventLevel + 1, eventMonster.Count() - 1);
        }
    }

    private IEnumerator SpawnEventMonstersWithDelay(GameObject monsterPrefab, GameObject parent, int repeatNum)
    {
        
        float spawnDelay = 5f; // 몬스터 간 소환 간격 (5초)

        for (int i = 0; i < repeatNum; i++)
        {
            GameObject player = GameManager.Instance.player.gameObject;
            Vector3 spawnPos = GameManager.Instance.pool.GetSpawnPos(player.transform.position);

            SpawnEventMonster(monsterPrefab, spawnPos, parent);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnEventMonster(GameObject mobPrefab, Vector3 spawnPos, GameObject parent)
    {
        if (mobPrefab.GetComponent<EventMonster>() == null && mobPrefab.GetComponent<Monster>() == null) return;
        float randomRange = 2f;

        int num = mobPrefab.GetComponent<EventMonster>().spawnNum < 0 ? 1 : mobPrefab.GetComponent<EventMonster>().spawnNum;
        for (int i = 0; i < num; i++)
        {
            Vector3 randomOffset = GetRandomOffset(randomRange);
            GameObject mobInstance = Instantiate(mobPrefab, spawnPos + randomOffset, Quaternion.identity);
            mobInstance.transform.parent = parent.transform;
        }
    }

    private Vector3 GetRandomOffset(float range)
    {
        return new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
    }

    
}
