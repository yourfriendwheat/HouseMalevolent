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

    public float Timer;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        Timer = 90.0f;
        Time.timeScale = 1;
        PlayerWon = false;
        isPlayerAlive = true; 
        winText.gameObject.SetActive(false); 
        loseText.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Time:" + (int)Timer;
        Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {
            gameOver();
        }

        restartGame();
        killGame();
    }

    public void gameOver()
    {
        loseText.gameObject.SetActive(true);
        isPlayerAlive = false;
        Time.timeScale = 0;
    }

    public void Win()
    {
        Time.timeScale = 0;
        winText.gameObject.SetActive(true);
        PlayerWon = true;   
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
