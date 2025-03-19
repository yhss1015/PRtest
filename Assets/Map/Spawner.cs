using UnityEngine;

public class Spawner : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.pool.GetPrefab(0);
        }
        
    }
}
