using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject scorePanel;
    public GameObject resumeButton;
    public GameObject highscoreButton;
    public GameObject startGameButton;
    public GameObject restartButton;
    public GameObject highScorePanel;
    public GameObject elements;
    private HighScoreHandler highscoreHandler;
    public Text newHighScore;
    public Text endScore;
    public Text playerName;
    private bool end = PlayerControl.endGame;

    void Start()
    {
        isPaused = true;
        if(end)
        {
            end = false;
        }
        StartMenu();
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
    public void Pause()
    {
        scorePanel.SetActive(false);
        restartButton.SetActive(true);
        startGameButton.SetActive(false);
        resumeButton.SetActive(true);
        highscoreButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void StartMenu()
    {
        Time.timeScale = 0f;
        isPaused = true;
        scorePanel.SetActive(false);
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
        highscoreButton.SetActive(true);
        startGameButton.SetActive(true);
        pauseMenuUI.SetActive(true);
    }

    public void StartGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void EndMenu()
    {
        Time.timeScale = 0f;
        isPaused = true;
        startGameButton.SetActive(false);
        restartButton.SetActive(true);
        scorePanel.SetActive(true);
        resumeButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        highScorePanel.SetActive(true);
        endScore.text = ScoreManager.Final_Points;
        newHighScore.text = ScoreManager.Final_Points;
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
        SceneChanger.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Back To menu
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneChanger.LoadScene(0);
    }

    public void OkButton()
    {
        int finalScore = Int32.Parse(ScoreManager.Final_Points); // try-catch?
        highscoreHandler.AddHighScoreIfPossible(new HighScoreElement(playerName.text, finalScore));
        highScorePanel.SetActive(false);
    }

    public void HighscoresButton()
    {
        Time.timeScale = 0f;
        SceneChanger.LoadScene(2);
    }
}
