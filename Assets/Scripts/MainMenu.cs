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
   // AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SettingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Experinment_codingInput");
    }

    public void OpenSetting()
    {
        settingMenu = true;
        SettingMenu.SetActive(true);

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CloseSetting() 
    {
        settingMenu = false; 
        SettingMenu.SetActive(false);
    }

}
