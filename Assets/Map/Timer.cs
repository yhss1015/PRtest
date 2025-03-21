using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float startTime;
    public Stopwatch stopwatch;
   

    public void StartTimer()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    public void StopTimer()
    {
        if(stopwatch.IsRunning)
        {
            stopwatch.Stop();
        }
    }
}
