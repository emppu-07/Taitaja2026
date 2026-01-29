using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    bool gameStarted = false;
    public Rigidbody2D lava;
    [SerializeField] private TimerController _timer;

    void Start()
    {     
    }

    void Update()
    {
        if (gameStarted)
        {
            lava.transform.Translate(Vector3.up * 1 * Time.deltaTime);
        }
    }

    public void GameStart(){

        gameStarted = true;
        _timer.StartTimer();
        //lava.transform.Translate(Vector3.up * 20 * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameStart();
            Debug.Log("GAME STARTTTR");
        }
    }
}
