using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject scorePanel;
    public GameObject resumeButton;
    private bool end;

    private void Start()
    {
        end = PlayerControl.EndGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(end)
        {
            EndMenu();
        }
    }

    // Game is paused
    private void Pause()
    {
        scorePanel.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void EndMenu()
    {
        pauseMenuUI.SetActive(true);
        scorePanel.SetActive(true);
        resumeButton.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    //Resume Game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Restart scene
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Back To menu
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
