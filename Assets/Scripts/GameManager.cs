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

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerWon = false;
        isPlayerAlive = true; 
        winText.gameObject.SetActive(false); 
        loseText.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
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
            Time.timeScale = 1;
            SceneManager.LoadScene("Experinment_codingInput");
        }

        if(Input.GetKeyDown(KeyCode.Space) && PlayerWon == true)
        {
            SceneManager.LoadScene("MainMenu_Testing");
            Time.timeScale = 1;
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
