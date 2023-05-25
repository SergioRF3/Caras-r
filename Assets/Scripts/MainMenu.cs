using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Interactable touch;
    private Interactable eye;
    private Interactable voice;
    public void GetSettings()
    {
        touch = GameObject.Find("TouchSelection").GetComponent<Interactable>();
        eye = GameObject.Find("EyeSelection").GetComponent<Interactable>();
        voice = GameObject.Find("VoiceSelection").GetComponent<Interactable>();
    }
    public void StartApplication()
    {
        Game.SetDefaultValues();
        SceneManager.LoadScene("GameScene");
    }
    public void QuitApplication()
    {
        Debug.Log("Application quited");
        Application.Quit();
    }
    public void ToggleMode(string mode)
    {
        switch (mode)
        {
            case "Voice":
                Game.TouchMode = false;
                Game.EyeMode = false;
                Game.VoiceMode = true;
                break;
            case "Touch":
                Game.TouchMode = true;
                Game.EyeMode = false;
                Game.VoiceMode = false;
                break;
            case "Eye":
                Game.TouchMode = false;
                Game.EyeMode = true;
                Game.VoiceMode = false;
                break;

        }
        LoadSettings();
    }
    public void LoadSettings()
    {
        GetSettings();
        touch.IsToggled= Game.TouchMode;
        eye.IsToggled= Game.EyeMode;
        voice.IsToggled = Game.VoiceMode;
    }
}
