using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject scorePanel;
    public GameObject resumeButton;
    private bool end = PlayerControl.endGame;

    void Start()
    {
        isPaused = false;
        if(end)
        {
            end = false;
        }
        
        StartCoroutine(End());
    }

    void Update()
    {
        end = PlayerControl.endGame;
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
    }

    private IEnumerator End()
    {
        if(end)
        {
            StopCoroutine(End());
            yield return null;
            EndMenu();
        }
        else
        {
            yield return null;
            StartCoroutine(End());
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

    private void EndMenu()
    {
        pauseMenuUI.SetActive(true);
        scorePanel.SetActive(true);
        resumeButton.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    //Resume Game
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Restart scene
    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Back To menu
    private void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
