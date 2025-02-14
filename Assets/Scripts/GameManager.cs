using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public bool isPlayerAlive;
    public EnemyMovement EnemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerAlive = true;  
    }

    // Update is called once per frame
    void Update()
    {
        restartGame();
        killGame();
    }

    public void gameOver()
    {
        //Debug.Log("You Lose! Press R to restart");
        isPlayerAlive = false;
        Time.timeScale = 0;
    }

    void restartGame() //Work in progress
    {
        if (Input.GetKeyDown(KeyCode.R) && isPlayerAlive == false)
        {
            Time.timeScale = 1;
        }
    }

    void killGame() //Work in progress
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Application has closed in this build");
        }
    }
}
