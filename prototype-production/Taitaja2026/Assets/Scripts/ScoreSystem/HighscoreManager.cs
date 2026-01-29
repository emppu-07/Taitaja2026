using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private const string BEST_TIME_KEY = "BestTime";

    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);
    }

    public bool TrySetNewBestTime(float time)
    {
        float bestTime = GetBestTime();
        if (time < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, time);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}
