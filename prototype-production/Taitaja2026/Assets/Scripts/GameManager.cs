using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    bool gameStarted = false;
    public GameObject lava;

    void Start()
    {
        GameStart();        
    }

    void Update()
    {
        
    }

    public void GameStart(){

        //TimerController.StartTimer();
        lava.transform.Translate(Vector3.up * 20 * Time.deltaTime);

    }
}
