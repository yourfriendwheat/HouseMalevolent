﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

  //  public AudioClip winSound;
   // public AudioClip loseSound;
   // AudioSource audioSource;

    private NewPlayerMovement NewPlayerMovement;

    public InputSystem UIsystem;
    public GameObject PauseMenu;
    public GameObject LoseMenu;
    public GameObject WinMenu;
    public GameObject Stamina;

    private bool isPaused = false; // Track whether the game is paused

    private void Awake()
    {
        UIsystem = new InputSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
       // audioSource = GetComponent<AudioSource>();

        Timer = 90.0f;
        Time.timeScale = 1;
        PlayerWon = false;
        isPlayerAlive = true;

        winText.gameObject.SetActive(false);
        LoseMenu.SetActive(false);
        pauseText.gameObject.SetActive(false);
        gameText.gameObject.SetActive(true);
        Stamina.SetActive(true);

        NewPlayerMovement = GameObject.Find("Player").GetComponent<NewPlayerMovement>();

        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            // Displays the timer
            timerText.text = "Time:" + (int)Timer;
            // Updates the timer
            Timer -= Time.deltaTime;

            if (Timer <= 0.0f)
            {
                gameOver();
            }

        }
    }

    // Lose function 
    public void gameOver()
    {
       // audioSource.Stop();
       // audioSource.PlayOneShot(loseSound);
        LoseMenu.SetActive(true);
        gameText.gameObject.SetActive(false);
        Stamina.SetActive(false);
        isPlayerAlive = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Win function 
    public void Win()
    {
        //audioSource.Stop();
       // audioSource.PlayOneShot(winSound);
        Time.timeScale = 0;
        gameText.gameObject.SetActive(false);
        WinMenu.SetActive(true);
        winText.gameObject.SetActive(true);
        Stamina.SetActive(false);
        PlayerWon = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function that unpauses the game
    public void Unpausegame()
    {
        Time.timeScale = 1;
        gameText.gameObject.SetActive(true);
        pauseText.gameObject.SetActive(false);
        PauseMenu.SetActive(false);
        Stamina.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        NewPlayerMovement.OnEnable();

    }

    // Function that pauses the game
    private void PauseGame()
    {
        Time.timeScale = 0;
        gameText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(true);
        PauseMenu.SetActive(true);
        Stamina.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        NewPlayerMovement.OnDisable();
    }

    // Toggle pause state
    private void TogglePause()
    {
        if (isPaused)
        {
            Unpausegame();
        }
        else
        {
            PauseGame();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu_Testing");
    }


    public void RestartGame()
    {
        if (!isPlayerAlive)
        {
            NewPlayerMovement.OnDisable();
            SceneManager.LoadScene("PrototypeEnvironment");
        }
        if (PlayerWon)
        {
            SceneManager.LoadScene("MainMenu_Testing");
        }
    }

    private void killGame(InputAction.CallbackContext ctx)
    {
        Application.Quit();
        Debug.Log("Application has closed in this build");
    }


    private void OnEnable()
    {
        UIsystem.UI.Enable();

        UIsystem.UI.KillGame.performed += killGame;
        UIsystem.UI.Restart.performed += ctx => RestartGame();
        UIsystem.UI.Pause.performed += ctx => TogglePause();
    }

    private void OnDisable()
    {
        UIsystem.UI.Disable();

        UIsystem.UI.KillGame.performed -= killGame;
        UIsystem.UI.Restart.performed -= ctx => RestartGame();
        UIsystem.UI.Pause.performed -= ctx => TogglePause();
    }
}
