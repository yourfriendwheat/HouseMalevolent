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
        loseText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false);
        gameText.gameObject.SetActive(true);

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

            killGame();

        }
    }

    // Lose function 
    public void gameOver()
    {
       // audioSource.Stop();
       // audioSource.PlayOneShot(loseSound);
        loseText.gameObject.SetActive(true);
        gameText.gameObject.SetActive(false);
        isPlayerAlive = false;
        Time.timeScale = 0;
    }

    // Win function 
    public void Win()
    {
        //audioSource.Stop();
       // audioSource.PlayOneShot(winSound);
        Time.timeScale = 0;
        gameText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        PlayerWon = true;
    }

    // Function that unpauses the game
    public void Unpausegame()
    {
        Time.timeScale = 1;
        gameText.gameObject.SetActive(true);
        pauseText.gameObject.SetActive(false);
        PauseMenu.SetActive(false);
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


    private void RestartGame(InputAction.CallbackContext ctx)
    {
        if (!isPlayerAlive)
        {
            NewPlayerMovement.OnDisable();
            SceneManager.LoadScene("Experinment_codingInput");
        }
        if (PlayerWon)
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

    private void OnEnable()
    {
        UIsystem.UI.Enable();

        UIsystem.UI.Restart.performed += RestartGame;
        UIsystem.UI.Pause.performed += ctx => TogglePause();
    }

    private void OnDisable()
    {
        UIsystem.UI.Disable();

        UIsystem.UI.Restart.performed -= RestartGame;
        UIsystem.UI.Pause.performed -= ctx => TogglePause();
    }
}
