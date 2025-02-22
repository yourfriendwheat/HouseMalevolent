using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering.VirtualTexturing;

public class MainMenu : MonoBehaviour
{
    bool settingMenu = false;
    public GameObject buttons;
    public GameObject SettingMenu;
    public GameObject AudioMenu;
    public GameObject KeyboardMenu;
    public GameObject ControllerMenu;
    public Button[] settingButtons;
    private int selectedIndex = 0;
    InputSystem inputSystem;

    void Awake()
    {
        inputSystem = new InputSystem();

    }


    void Start()
    {
        SettingMenu.SetActive(false);
        AudioMenu.SetActive(false);
        KeyboardMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PrototypeEnvironment");
    }

    public void OpenSetting()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);
        SetActivePanel(0); // Default to AudioMenu
    }

    public void OpenKeyboard()
    {
        SetActivePanel(1);
    }

    public void OpenController()
    {
        SetActivePanel(2);
    }

    public void OpenAudio()
    {
        SetActivePanel(0);
    }

    private void SetActivePanel(int index)
    {
        AudioMenu.SetActive(index == 0);
        KeyboardMenu.SetActive(index == 1);
        ControllerMenu.SetActive(index == 2);
        selectedIndex = index;
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Application has closed in this build");
    }

    public void CloseSetting()
    {
        settingMenu = false;
        SettingMenu.SetActive(false);
        AudioMenu.SetActive(false);
        KeyboardMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

    void Update()
    {

    }

    private void GoRightButton(InputAction.CallbackContext ctx)
    {
        if (settingMenu)
        {
            selectedIndex = (selectedIndex + 1) % settingButtons.Length;
            SetActivePanel(selectedIndex);
        }

    }
    private void GoLeftButton(InputAction.CallbackContext ctx)
    {
        if (settingMenu)
        {
            selectedIndex = (selectedIndex - 1 + settingButtons.Length) % settingButtons.Length;
            SetActivePanel(selectedIndex);
        }
    }

    public void OnDisable()
    {
        inputSystem.MainMenu.Disable();
        inputSystem.MainMenu.RightButton.performed -= GoRightButton;
        inputSystem.MainMenu.LeftButton.performed -= GoLeftButton;
    }

    public void OnEnable()
    {
        inputSystem.MainMenu.Enable();
        inputSystem.MainMenu.RightButton.performed += GoRightButton;
        inputSystem.MainMenu.LeftButton.performed += GoLeftButton;
    }
}
