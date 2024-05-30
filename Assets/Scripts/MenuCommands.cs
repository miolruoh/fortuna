using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
    // Play first board
    public void OnPlayBoard1ButtonPress()
    {
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
    // Go back to previous scene
    public void OnBackButtonPress()
    {
        SceneChanger.PreviousScene();
    }
    // Exit game
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
