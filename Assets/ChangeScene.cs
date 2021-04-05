using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void OnButtonPress()
    {
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings)
        {
            i = 0;
        }
        SceneManager.LoadScene(i);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
