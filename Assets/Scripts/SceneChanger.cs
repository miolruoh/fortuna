using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class SceneChanger : MonoBehaviour
{
    private static List<int> sceneHistory = new List<int>();

    // Create DontDestroyOnLoad to keep scenechanger alive
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Game opens in menu so add it straight to the history
    void Start()
    {
        sceneHistory.Add(0);
    }
    // Add previous scene to history and change scene
    public static void LoadScene(int newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
    }
    // Go back to previous scene, keep the scenehistory count at 2 and if this fails, go back to menu
    public static void PreviousScene()
    {
        if(sceneHistory.Count >= 2)
        {
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count -1]);
        }
        else
        {
            LoadScene(0);
        }
    }
}
