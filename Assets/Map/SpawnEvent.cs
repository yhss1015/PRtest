using System.Linq;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string[] GetEventFolderNames()
    {
        return Resources.LoadAll<TextAsset>("Event")
            .Select(file => System.IO.Path.GetFileNameWithoutExtension(file.name))
            .ToArray();
    }
}
