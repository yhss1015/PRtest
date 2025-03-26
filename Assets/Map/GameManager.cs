using System.Collections;
using UnityEngine;
using VampireSurvival.ItemSystem;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PoolManager pool;
    public Player player;
    public ItemManager itemManager;
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
            itemManager = GameObject.Find("itemManager").GetComponent<ItemManager>();
        }
        catch
        {

        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetisRunning();
            spawner.GameStart();
        }
    }

    void levelUpEvent()
    {
        Vector3 playerpos = GameManager.Instance.player.transform.position;
        Random.InitState((int)System.DateTime.Now.Ticks);
        Time.timeScale = 0;
        WeaponData[] selectItem = new WeaponData[3];
        GameManager.Instance.itemManager.SpawnRandomWeaponOrAccessory(playerpos);
        UIManager.instance.LevelUpUI(selectItem, (selectedItem) =>
        {
            GameManager.Instance.player.UpdateWeaponInfo(selectedItem);
        });
        Time.timeScale = 1;
    }


    public void SetisRunning()
    {
        isRunning = !isRunning;
    }

    
}
