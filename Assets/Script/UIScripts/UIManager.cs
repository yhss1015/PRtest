using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvival.ItemSystem;
using static ItemManager;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Player Player;
    public WeaponData[] weapons= new WeaponData[3];
    public Button[] buttons = new Button[3];
    public Image[] buttonImage = new Image[3];
    public Text[] buttonDescription = new Text[3];
    public Text[] texts = new Text[10];
    public Slider ExpSlider;
    public GameObject LevelUpUi;
    public GameObject[] menuButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameManager.Instance.player = Player;
        LevelUpUi.SetActive(false);
    }

    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager 삭제
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        texts[0].text = Player.attack.ToString("F1");
        texts[1].text = Player.maxHp.ToString("F1");
        texts[2].text = Player.curHp.ToString("F1");
        texts[3].text = Player.speed.ToString("F1");
        ExpSlider.value = Player.curExp/Player.maxExp;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale==1)
            {
                Time.timeScale = 0;
                menuButtons[0].SetActive(true);
                menuButtons[1].SetActive(true);
            }
            
        }
    }
    public void TimeScaleChange(float scale)
    {
        Time.timeScale = scale;
    }

    public void LevelUpUI(List<ItemManager.RandomItemData> items, Action<ScriptableObject> onSelected)
    {
        LevelUpUi.SetActive(true);
        for (int i = 0; i < items.Count && i < 3; i++)
        {
            int index = i;
            ScriptableObject item = items[i].itemData;

            if (item is WeaponData weapon)
            {
                int currentLevel = 0;
                var equipped = GameManager.Instance.playerInventory.equippedWeapons.Find(e => e.itemData == weapon);
                if (equipped != null)
                { currentLevel = equipped.currentLevel; }
                // Whip의 경우 currentLevel이 0이면 보정해서 1로 처리
                if (weapon.weaponType == WeaponType.Whip && currentLevel == 0)
                { currentLevel = 1; }
                int nextLevel = currentLevel + 1;
                int descriptionIndex = (weapon.weaponType == WeaponType.Whip) ? currentLevel : nextLevel - 1;
                string desc = "";
                if (weapon.levelDescriptions != null && weapon.levelDescriptions.Length > descriptionIndex)
                { desc = weapon.levelDescriptions[descriptionIndex]; }

                buttonImage[i].sprite = weapon.itemSprite;
                buttonDescription[i].text = weapon.name + " (" + nextLevel + ")\n" + desc;
            }
            else if (item is AccessoryData accessory)
            {
                int currentLevel = 0;
                var equipped = GameManager.Instance.playerInventory.equippedAccessories.Find(e => e.itemData == accessory);
                if (equipped != null)
                    currentLevel = equipped.currentLevel;
                int nextLevel = currentLevel + 1;
                buttonImage[i].sprite = accessory.itemSprite;
                buttonDescription[i].text = accessory.name + " (" + nextLevel + ")\n" + accessory.levelDescription;
            }

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() =>
            {
                LevelUpUi.SetActive(false);
                onSelected?.Invoke(item);
            });
        }
    }



}
