using System;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float currentTime;
    private bool timerActive;

    private void Start()
    {
        currentTime = 0f;
        //timerActive = true;
    }

    private void Update()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
            timerText.text = FormatTime(currentTime);
        }
    }


    public void StartTimer()
    {
        timerActive = true;
    }
    
    
    public void StopTimer()
    {
        timerActive = false;

        if (HighscoreManager.Instance != null)
        {
            bool isNewRecord = HighscoreManager.Instance.TrySetNewBestTime(currentTime);
            if (isNewRecord)
                Debug.Log("New Record! " + FormatTime(currentTime));
        }

        
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:00}:{1:00}:{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10);
    }
}
