using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PoolManager pool;
    public Player player;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetisRunning();
        }
    }

    public void SetisRunning()
    {
        isRunning = !isRunning;
    }
}
