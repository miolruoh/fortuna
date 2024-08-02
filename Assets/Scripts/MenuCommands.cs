using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
    [SerializeField] private static string _fileName;
    public static string FileName
    {
        get {return _fileName;}
    }

    private void Start()
    {
        
    }

    // Play first board
    public void OnPlayBoard1ButtonPress()
    {
        _fileName = "highscores1.json";
        // Later to collapse and select board you want to play

       /* int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings)
        {
            i = 1;
        }*/
        SceneChanger.LoadScene(2);
    }
    // Play second board
    public void OnPlayBoard2ButtonPress()
    {
         _fileName = "highscores2.json";
        // Later to collapse and select board you want to play

        /* int i = SceneManager.GetActiveScene().buildIndex + 1;
         if (i >= SceneManager.sceneCountInBuildSettings)
         {
             i = 1;
         }*/
        SceneChanger.LoadScene(3);
    }
    // Go to settings
    public void OnSettingsButtonPress()
    {
        SceneChanger.LoadScene(1);
    }
    // Go back to previous scene from settings
    public void OnBackButtonPress()
    {
        SceneChanger.PreviousScene();
    }
    // Exit game on windows / on android put game to background
    public void ExitGame()
    {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Application.Quit();
        #elif UNITY_ANDROID
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        #endif
    }
}
