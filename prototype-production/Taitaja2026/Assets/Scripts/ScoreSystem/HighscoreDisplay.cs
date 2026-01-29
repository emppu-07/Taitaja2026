using System;
using UnityEngine;
using TMPro;

public class HighscoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        if (HighscoreManager.Instance != null)
        {
            float bestTime = HighscoreManager.Instance.GetBestTime();

            if (bestTime < float.MaxValue)
                highScoreText.text = "Best Time: " + FormatTime(bestTime);
            else
                highScoreText.text = "Best Time: --:--:--";
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:00}:{1:00}:{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10);
    }
}
