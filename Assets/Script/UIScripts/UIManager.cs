using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvival.ItemSystem;

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

    public void LevelUpUI(WeaponData[] item, Action<WeaponData> onSelected)
    {
        LevelUpUi.SetActive(true);
        WeaponData[] weapons = { item[0], item[1], item[2] };

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            buttonImage[i].GetComponent<Image>().sprite = weapons[i].itemSprite;
            buttonDescription[i].text = weapons[i].name + "\n\n" + weapons[i].name; //나중에 itemData에 description추가될시 뒤에 부분 변경 필요.

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() =>
            {
                LevelUpUi.SetActive(false);
                onSelected?.Invoke(weapons[index]);
            });
        }
    }




}
