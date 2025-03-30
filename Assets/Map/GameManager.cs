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
    public PlayerInventory playerInventory;
    public PrefabManager prefabManager;
    public GameObject DmgPrefab;

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

    public void levelUpEvent()
    {
        Vector3 playerpos = GameManager.Instance.player.transform.position;
        Random.InitState((int)System.DateTime.Now.Ticks);
        Time.timeScale = 0;
        List<RandomItemData> randomItems = GameManager.Instance.itemManager.GetRandomItemData(GameManager.Instance.itemManager.playerInventory);

        UIManager.instance.LevelUpUI(randomItems, (selectedItem) =>
        {
            if (selectedItem is WeaponData weapon)
            {                

                Time.timeScale = 1;
                int index = 0;
                index = playerInventory.FIndIndex(weapon);
                if (!playerInventory.FIndWeapon(weapon))
                {
                    playerInventory.AddNewWeapon(weapon);
                    GameManager.Instance.prefabManager.UpdateWeaponPrefab(weapon, playerInventory.equippedWeapons[index].currentLevel);
                    GameManager.Instance.player.StartWeapon(weapon);
                }
                else
                {
                    playerInventory.LevelUpWeapon(index);
                    GameManager.Instance.prefabManager.UpdateWeaponPrefab(weapon, playerInventory.equippedWeapons[index].currentLevel);
                }                               
                
                //GameManager.Instance.player.UpdateWeaponInfo(weapon, playerInventory.equippedWeapons[index].currentLevel);
            }
            else if (selectedItem is AccessoryData accessory)
            {
                Time.timeScale = 1;
                GameManager.Instance.itemManager.playerInventory.AddNewAccessory(accessory);
            }
        });
        
        
    }



    public void SetisRunning()
    {
        isRunning = !isRunning;
    }

    
}
