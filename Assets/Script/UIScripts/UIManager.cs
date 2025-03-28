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
    }

    public void LevelUpUI(List<RandomItemData> items, Action<ScriptableObject> onSelected)
    {
        LevelUpUi.SetActive(true);
        // items 리스트가 3개 미만일 수도 있으니, 최소 3개가 되도록 처리하거나 UI를 유연하게 구성해야 합니다.
        for (int i = 0; i < items.Count && i < 3; i++)
        {
            int index = i;
            ScriptableObject item = items[i].itemData;

            // 아이템이 무기이면 WeaponData, 장신구이면 AccessoryData로 처리 (여기서는 두 타입 모두 itemSprite와 name을 가지고 있다고 가정)
            if (item is WeaponData weapon)
            {
                buttonImage[i].sprite = weapon.itemSprite;
                buttonDescription[i].text = weapon.name + "\n\n" + weapon.name;
            }
            else if (item is AccessoryData accessory)
            {
                buttonImage[i].sprite = accessory.itemSprite;
                buttonDescription[i].text = accessory.name + "\n\n" + accessory.name;
            }
            else
            {
                buttonImage[i].sprite = null;
                buttonDescription[i].text = "Unknown";
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
