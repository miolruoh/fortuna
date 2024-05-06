using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject scorePanel;
    public GameObject resumeButton;
    //public GameObject HighScorePanel;
    private static GameObject highScoreElement = NewHighScore.HighScoreElement;
    public Text newHighScore;
    public Text endScore;
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
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.SetActive(true);
        scorePanel.SetActive(true);
        resumeButton.SetActive(false);
        endScore.text = ScoreManager.Final_Points;

        Text oldHighScoreTxt = GameObject.Find("HighScore/HighScoreElement/ScoreTxt").GetComponent<Text>();
        // TODO
        try
        {
            int _endScore = Int32.Parse(endScore.text);
            int _oldHighScore = Int32.Parse(oldHighScoreTxt.text);
            if(_endScore > _oldHighScore)
            {
                GameObject newHighScoreElement = Instantiate<GameObject>(highScoreElement);
                Destroy(highScoreElement);
                highScoreElement = newHighScoreElement;
                string newNameTxt = GameObject.Find("PauseCanvas/NewHighScorePanel/EnterPlayerNameField/Text").GetComponent<string>();
                oldHighScoreTxt = endScore;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{endScore.text}'"); // Output: Unable to parse ''
        }

        newHighScore.text = ScoreManager.Final_Points;
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
