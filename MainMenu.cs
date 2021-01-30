using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;


//enum Scene [];
public class MainMenu : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject settingMenu;
    [SerializeField] GameObject victoryMenu;
    bool activeMenu = true;

    [Header("Buttons")]
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject settingTopBtn;
    [SerializeField] GameObject mainMenuButton;

    [Header("Audio Settings")]
    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        int victoryInt = PlayerPrefs.GetInt("Victory");
        if(victoryInt == 1) // if the player has had a victory load the victory Menu and reset the victory int to 0.
        {
            VictoryMenu();
            PlayerPrefs.SetInt("Victory", 0);
        }


        Cursor.lockState = CursorLockMode.None; // disable the cursor lock.
        Cursor.visible = true; // make the cursor visible.
    }

    public void StartGame() // method for the start button. loads first scene and sets player prefs varibles to default.
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("numOfSpells", 0);
    }

    public void ControlsMenu() // method for contolls button. hides mainmenu and shows the control menu. also sets the selected object for controler support.
    {
        mainMenu.SetActive(!activeMenu);
        EventSystem.current.SetSelectedGameObject(null);
        controlsMenu.SetActive(activeMenu);
        EventSystem.current.SetSelectedGameObject(controlsButton);
    }

    public void CreditsMenu()
    {
        mainMenu.SetActive(!activeMenu);
        EventSystem.current.SetSelectedGameObject(null);
        creditsMenu.SetActive(activeMenu);
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }

    public void SettingMenu()
    {
        mainMenu.SetActive(!activeMenu);
        EventSystem.current.SetSelectedGameObject(null);
        settingMenu.SetActive(activeMenu);
        EventSystem.current.SetSelectedGameObject(settingTopBtn);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back() // hides other menus and shows the main menu and sets the selected object.
    {
        EventSystem.current.SetSelectedGameObject(null);
        mainMenu.SetActive(activeMenu);
        EventSystem.current.SetSelectedGameObject(startButton);
        controlsMenu.SetActive(!activeMenu);
        creditsMenu.SetActive(!activeMenu);
        settingMenu.SetActive(!activeMenu);
        victoryMenu.SetActive(!activeMenu);
    }

    public void VictoryMenu()
    {
        mainMenu.SetActive(!activeMenu);
        EventSystem.current.SetSelectedGameObject(null);
        victoryMenu.SetActive(activeMenu);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    public void SetMainVolume(float value) // method for the main volume slider in the setting menu.
    {
        audioMixer.SetFloat("MainVolume", value); // sets the audiomixers mainvolume varible to the value from the UI slider.
    }

    public void SetAmbientMusicVolume(float value)
    {
        audioMixer.SetFloat("AmbientMusicVolume", value);
    }

    public void SetAmbientSoundsVolume(float value)
    {
        audioMixer.SetFloat("AmbientSoundsVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", value);
    }
}
