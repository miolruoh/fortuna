using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip newHighscoreSFX;
    private readonly float clickVolume = AudioManager.ClickVolume;
    private readonly float newHighscoreVolume = AudioManager.NewHighScoreSFXVolume;
    private static bool isPaused;
    public static bool IsPaused
    {
        get {return isPaused;}
    }
    public GameObject pauseMenuUI;
    public GameObject scorePanel;
    public GameObject resumeButton;
    public GameObject highscoreButton;
    public GameObject startGameButton;
    public GameObject restartButton;    
    public GameObject newHighScorePanel;
    public GameObject highScorePanel;
    public GameObject tutorial;
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;
    List<GameObject> uiHighscoreElements = new List<GameObject>();
    [SerializeField] HighScoreHandler highscoreHandler;
    public Text newHighScore;
    public Text endScore;
    [SerializeField] int finalScore;
    public Text playerName;
    public Button gameSoundButton;
    public Button musicButton;
    private float gamespeed = 3.0f;
    public static bool tutorialSwitch;
    


    public delegate void CheckIfMusicButtonExists(Button button);
    public static event CheckIfMusicButtonExists checkIfMusicButtonExists;
    public delegate void CheckIfSFXButtonExists(Button button);
    public static event CheckIfSFXButtonExists checkIfSFXButtonExists;

    // Set everything ready for game to start
    void Awake()
    {   // check if the scene has music/sfx toggle buttons and their state(on/off)
        if(checkIfMusicButtonExists != null && checkIfSFXButtonExists != null)
        {
            checkIfMusicButtonExists(musicButton);
            checkIfSFXButtonExists(gameSoundButton);
        }
        tutorialSwitch = false;
        highscoreHandler.LoadHighScores();
        highScorePanel.SetActive(false);
        isPaused = true;
        StartMenu();
    }
    // Update to check if game is paused/continued
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
    // Check if game is ready to end
    private void End()
    {
        EndMenu();
    }

    // Set elements for when game is paused
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
    // Set elements for the startgame menu
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
    // Start game button, check if tutorial is needed and closes menu
    public void StartGame()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        if(PlayerPrefs.GetInt("HasLaunched", 0) == 0) // Game hasn't launched before. 0 is the default value if the player pref doesn't exist yet.
        {
            tutorialSwitch = true;
            TutorialActivation(tutorialSwitch);
            
        }
        else
        {
            tutorialSwitch = false;
            TutorialActivation(tutorialSwitch);
        }
        PlayerPrefs.SetInt("HasLaunched", 1); // Set to 1, so we know the user has been here before
        pauseMenuUI.SetActive(false);
        Time.timeScale = gamespeed;
        isPaused = false;
    }
    // Handle tutorial animations
    private void TutorialActivation(bool activated)
    {
        tutorial.SetActive(activated);
    }
    // Panel at the end of the game and possible new highscorepanel
    public void EndMenu()
    {
        Time.timeScale = 0f;
        isPaused = true;
        startGameButton.SetActive(false);
        restartButton.SetActive(true);
        scorePanel.SetActive(true);
        resumeButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        highscoreButton.SetActive(true);
        endScore.text = ScoreManager.Final_Points;
        // Check if there is enough points for highscore list
        if(highscoreHandler.GetHighScoreCount() < highscoreHandler.MaxCount || ScoreManager.Points > highscoreHandler.GetLastHighScore())
        {
            AudioManager.instance.PlaySFXClip(newHighscoreSFX, transform, newHighscoreVolume);
            newHighScorePanel.SetActive(true);
            newHighScore.text = ScoreManager.Final_Points;
        }
    }

    //Resume Game after paused
    public void Resume()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        pauseMenuUI.SetActive(false);
        Time.timeScale = gamespeed;
        isPaused = false;
    }

    // Restart game
    public void Restart()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        SceneChanger.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Back To menu button
    public void BackToMenu()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        Time.timeScale = 1f;
        SceneChanger.LoadScene(0);
    }
    // Back button in highscore panel
    public void BackButton()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        highScorePanel.SetActive(false);
    }
    // Add new highscore and close new highscore window
    public void OkButton()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        finalScore = Int32.Parse(ScoreManager.Final_Points); // try-catch?
        highscoreHandler.AddHighScoreIfPossible(new HighScoreElement(playerName.text, finalScore));
        newHighScorePanel.SetActive(false);
    }
    // open highscore panel
    public void HighscoresButton()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        Time.timeScale = 0f;
        highScorePanel.SetActive(true);
    }

    // Update highscore list
    private void UpdateHighScoreUI (List<HighScoreElement> list) 
    {
        for(int i = 0; i < list.Count; i++) 
        {
            HighScoreElement el = list[i];

            if(el != null && el.score > 0) 
            {
                if(i >= uiHighscoreElements.Count) 
                {
                    // instantiate new entry
                    var inst = Instantiate(highscoreUIElementPrefab, elementWrapper);
                    uiHighscoreElements.Add(inst);
                }

                // write or overwrite name & points
                var texts = uiHighscoreElements[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = el.playerName;
                texts[1].text = el.score.ToString();
            }
        }
    }
    // start event
    private void OnEnable() 
    {
        HighScoreHandler.onHighScoreListChanged += UpdateHighScoreUI;
        PlayerControl.onTutorialSwitchChanged += TutorialActivation;
        PlayerControl.onGameEnded += End;
    }
    // end event
    private void OnDisable()
    {
        HighScoreHandler.onHighScoreListChanged -= UpdateHighScoreUI;
        PlayerControl.onTutorialSwitchChanged -= TutorialActivation;
        PlayerControl.onGameEnded -= End;
    }
}
