using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Unity.Mathematics;
using UnityEngine; 

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 5f; // 몬스터 생성 주기
    public float difficultyIncreaseInterval = 30f; // 난이도 증가 주기
    private int monsterLevel = 0; // 몬스터 강도
    public int monsterNum = 0; //몬스터 숫자
    private Stopwatch stopwatch;

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
        stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(SpawnMonsters());
        StartCoroutine(IncreaseDifficulty());
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
            if (CheckPoolActive() >= monsterMaxNum) continue;

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

    public int CheckPoolActive()
    {
        return GameManager.Instance.pool.pools.Sum(pool => pool.Count(obj => obj.activeSelf));
    }

    public void RandomEvent()
    {
        string[] eventFolder = GetEventFolderNames();

        int rand = UnityEngine.Random.Range(0, eventFolder.);

        switch (rand)
        {
            case 0: 
                break;
        }
    }

    string[] GetEventFolderNames()
    {
        // "Event"라는 이름이 포함된 폴더의 경로를 얻기 위해 Resources 폴더를 탐색
        string[] allFolders = System.IO.Directory.GetDirectories("Resources/"); // Resources 폴더 내 모든 디렉터리 경로 가져오기
        // Event로 시작하는 폴더만 선택
        var eventFolders = allFolders.Where(folder => folder.Contains("Event")).ToArray();

        // 폴더 이름만 추출
        string[] eventFolderNames = eventFolders.Select(folder => System.IO.Path.GetFileName(folder)).ToArray();

        return eventFolderNames;
    }

}
