using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VampireSurvival.ItemSystem;
using static ItemManager;
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
        List<RandomItemData> randomItems = GameManager.Instance.itemManager.GetRandomItemData(GameManager.Instance.itemManager.playerInventory);

        UIManager.instance.LevelUpUI(randomItems, (selectedItem) =>
        {
            if (selectedItem is WeaponData weapon)
            {
                GameManager.Instance.player.UpdateWeaponInfo(weapon);
            }
            else if (selectedItem is AccessoryData accessory)
            {
                GameManager.Instance.itemManager.playerInventory.AddNewAccessory(accessory);
            }
        });
        Time.timeScale = 1;
    }



    public void SetisRunning()
    {
        isRunning = !isRunning;
    }

    
}
