using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class SceneChanger : MonoBehaviour
{
    private static List<int> sceneHistory = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void LoadScene(int newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
    }
 
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
