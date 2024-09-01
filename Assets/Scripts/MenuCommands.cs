using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
    [SerializeField] private static string _fileName;
    [SerializeField] private AudioClip clickSFX;
    private readonly float clickVolume = AudioManager.ClickVolume;
    public static string FileName
    {
        get {return _fileName;}
    }

    // Open Credits for the project
    public void OnClickCredits()
    {
        // Add credits
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
    }

    // Play red board
    public void OnPlayBoard1ButtonPress()
    {
        _fileName = "highscores1.json";
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        SceneChanger.LoadScene(2);
    }
    // Play green board
    public void OnPlayBoard2ButtonPress()
    {
        _fileName = "highscores2.json";
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        SceneChanger.LoadScene(3);
    }
    // Go to settings
    public void OnSettingsButtonPress()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        SceneChanger.LoadScene(1);
    }
    // Go back to previous scene from settings
    public void OnBackButtonPress()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        SceneChanger.PreviousScene();
    }
    // Exit game on windows(in editor) / on android put game to background
    public void ExitGame()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Application.Quit();
        #elif UNITY_ANDROID
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        #endif
    }
}
