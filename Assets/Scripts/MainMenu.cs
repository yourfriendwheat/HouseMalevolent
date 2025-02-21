using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool settingMenu = false;
    public GameObject buttons;
    public GameObject SettingMenu;
    public GameObject AudioMenu;
    public GameObject KeybaordMenu;
    public GameObject ControllerMenu;


   // AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SettingMenu.SetActive(false);
        AudioMenu.SetActive(false);
        KeybaordMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Experinment_CodingInput");
    }

    public void OpenSetting()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);
        AudioMenu.SetActive(true);
        KeybaordMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

    public void OpenKeyboard()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);
        AudioMenu.SetActive(false);
        KeybaordMenu.SetActive(true);
        ControllerMenu.SetActive(false);
    }

    public void OpenController()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);
        AudioMenu.SetActive(false);
        KeybaordMenu.SetActive(false);
        ControllerMenu.SetActive(true);
    }

    public void OpenAudio()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);
        AudioMenu.SetActive(true);
        KeybaordMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CloseSetting() 
    {
        settingMenu = false; 
        SettingMenu.SetActive(false);
        AudioMenu.SetActive(false);
        KeybaordMenu.SetActive(false);
        ControllerMenu.SetActive(false);
    }

}
