using System.Collections;
using UnityEngine;
using VampireSurvival.ItemSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ItemManager ItemManager;
    public PoolManager pool;
    public Player player;
    public Spawner spawner;
    

    public bool isRunning = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        try
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        catch
        {

        }
    }

    /*void levelUpEvent()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        WeaponData[] selectItem = new WeaponData[3];
        WeaponData selectedItem;
        ItemManager.SpawnRandomWeaponOrAccessory(selectItem);
        UIManager.instance.LevelUpUI(selectItem, (selectedItem) =>
        {
            GameManager.Instance.player.AddWeapon(selectedItem); // 예시: 무기 적용
        });
    }*/

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetisRunning();
            spawner.GameStart();
        }
    }

    public void SetisRunning()
    {
        isRunning = !isRunning;
    }

    
}
