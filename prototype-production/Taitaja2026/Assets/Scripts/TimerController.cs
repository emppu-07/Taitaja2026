using System;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private bool timerActive;
    private float currentTime;

    private void Start()
    {
        currentTime = 0f;
        timerActive = true;
    }

    private void Update()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}",
            time.Minutes,
            time.Seconds,
            time.Milliseconds / 10);
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}
