using System.Collections;
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

    private EnemyMovement EnemyMovement;

    private float Timer;
    private int x = 1;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameText;
    public TextMeshProUGUI KeyReminder;
    public TextMeshProUGUI GetBack;
    public TextMeshProUGUI BeginningText;


    public AudioClip winSound;
    public AudioClip loseSound;
    AudioSource audioSource;

    private NewPlayerMovement NewPlayerMovement;

    public InputSystem UIsystem;
    public GameObject PauseMenu;
    public GameObject LoseMenu;
    public GameObject WinMenu;
    public GameObject Stamina;
    public GameObject Flashlight;
    private bool isPaused = false; // Track whether the game is paused

    private void Awake()
    {
        UIsystem = new InputSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Timer = 90.0f;
        Time.timeScale = 1;
        PlayerWon = false;
        isPlayerAlive = true;

        LoseMenu.SetActive(false);
        gameText.gameObject.SetActive(true);
        Stamina.SetActive(true);
        Flashlight.SetActive(true);
        NewPlayerMovement = GameObject.Find("Player").GetComponent<NewPlayerMovement>();
        EnemyMovement = GameObject.FindWithTag("Enemy").GetComponent<EnemyMovement>();

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
                if(x == 1)
                {
                    x = 2;
                    gameOver();
                }
            }

        }
    }

    // Lose function 
    public void gameOver()
    {
        audioSource.Stop();
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position); 
        LoseMenu.SetActive(true);
        gameText.gameObject.SetActive(false);
        Stamina.SetActive(false);
        Flashlight.SetActive(false);
        KeyReminder.gameObject.SetActive(false);
        GetBack.gameObject.SetActive(false);
        BeginningText.gameObject.SetActive(false);


        isPlayerAlive = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Win function 
    public void Win()
    {
        EnemyMovement.enemySound.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Time.timeScale = 0;
        gameText.gameObject.SetActive(false);
        WinMenu.SetActive(true);
        Stamina.SetActive(false);
        Flashlight.SetActive(false);
        PlayerWon = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function that unpauses the game
    public void Unpausegame()
    {
        Time.timeScale = 1;
        gameText.gameObject.SetActive(true);
        PauseMenu.SetActive(false);
        Stamina.SetActive(true);
        Flashlight.SetActive(true);


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
        PauseMenu.SetActive(true);
        Stamina.SetActive(false);
        Flashlight.SetActive(false);

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
