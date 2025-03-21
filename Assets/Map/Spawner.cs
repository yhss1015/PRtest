using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 5f; // 몬스터 생성 주기
    public float difficultyIncreaseInterval = 30f; // 난이도 증가 주기
    private int monsterLevel = 0; // 몬스터 강도
    public int monsterNum = 0; //몬스터 숫자
    private Stopwatch stopwatch;

    private void Start()
    {
        monsterNum = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStart();
        }
        
    }
    private void GameStart()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(SpawnMonsters());
        StartCoroutine(IncreaseDifficulty());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnMonster();
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
        for(int i = 0; i<monsterNum; i++)
        {
            GameObject monster = GameManager.Instance.pool.GetPrefab(monsterLevel);
            if (monster != null)
            {
                // 몬스터 위치 설정 (예제)
                monster.transform.position = GameManager.Instance.pool.GetSpawnPos(GameManager.Instance.player.transform.position);
            }
        }
        
    }

}
