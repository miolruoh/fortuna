using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCommands : MonoBehaviour
{

    public void OnPlayBoard1ButtonPress()
    {
        // Later to collapse and select board you want to play

       /* int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings)
        {
            i = 1;
        }*/
        SceneManager.LoadScene(3);
    }

    public void OnPlayBoard2ButtonPress()
    {
        // Later to collapse and select board you want to play

        /* int i = SceneManager.GetActiveScene().buildIndex + 1;
         if (i >= SceneManager.sceneCountInBuildSettings)
         {
             i = 1;
         }*/
        SceneManager.LoadScene(4);
    }

    public void OnSettingsButtonPress()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBackButtonPress()
    {
        SceneManager.LoadScene(0);
    }

    public void OnHighscoreButtonPress()
    {
        SceneManager.LoadScene(2);
    }
}
