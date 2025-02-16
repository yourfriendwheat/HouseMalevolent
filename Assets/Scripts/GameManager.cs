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
    public bool PlayerWon;

    public EnemyMovement EnemyMovement;

    private float Timer;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI gameText;

    // Start is called before the first frame update
    void Start()
    {
        Timer = 90.0f;
        Time.timeScale = 1;
        PlayerWon = false;
        isPlayerAlive = true; 

        winText.gameObject.SetActive(false); 
        loseText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false); 
        gameText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Displays the timer
        timerText.text = "Time:" + (int)Timer;

        //Updates the timer
        Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {
            gameOver();
        }


        //If statement that allows the player to pause/unpause the game while playing it
        if(isPlayerAlive == true && PlayerWon == false)
        {
            PauseGame();
            Unpausegame();
        }

        restartGame();
        killGame();
    }

    //Lose function 
    public void gameOver()
    {
        loseText.gameObject.SetActive(true);
        gameText.gameObject.SetActive(false);
        isPlayerAlive = false;
        Time.timeScale = 0;
    }

    //Win function 
    public void Win()
    {
        Time.timeScale = 0;
        gameText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        PlayerWon = true;   
    }

    //Function that pauses the game
    void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            gameText.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(true);
        }
    }

    //Function that unpauses the game
    void Unpausegame()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Time.timeScale = 1;
            gameText.gameObject.SetActive(true);
            pauseText.gameObject.SetActive(false);
        }
    }

    void restartGame() //Work in progress
    {
        if (Input.GetKeyDown(KeyCode.R) && isPlayerAlive == false)
        {
            SceneManager.LoadScene("Experinment_codingInput");
        }

        if(Input.GetKeyDown(KeyCode.Space) && PlayerWon == true)
        {
            SceneManager.LoadScene("MainMenu_Testing");
        }
    }

    void killGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Application has closed in this build");
        }
    }
}
