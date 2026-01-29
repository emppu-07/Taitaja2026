using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void StartGame() {
        Debug.Log("Level1");
        SceneManager.LoadScene("something");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
