using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finish : MonoBehaviour
{

    [SerializeField] private TimerController _timer;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            _timer.StopTimer();
            Debug.Log("GAME STARTTTR");
            SceneManager.LoadScene("MainMenu");
        }
    }

}
