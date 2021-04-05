using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCommands : MonoBehaviour
{

    public void OnPlayButtonPress()
    {
        // Later to collapse and select board you want to play

       /* int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings)
        {
            i = 1;
        }*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnOptionsButtonPress()
    {
        // open options menu
    }
}
