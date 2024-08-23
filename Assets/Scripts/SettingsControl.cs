using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    private Text currentTxt;
    private int qualityLevel;
    public Button gameSoundButton;
    public Button musicButton;
    [SerializeField] private AudioClip clickSFX;
    private readonly float clickVolume = AudioManager.ClickVolume;


    public delegate void CheckIfMusicButtonExists(Button button);
    public static event CheckIfMusicButtonExists checkIfMusicButtonExists;
    public delegate void CheckIfSFXButtonExists(Button button);
    public static event CheckIfSFXButtonExists checkIfSFXButtonExists;


    // Start is called before the first frame update
    void Start()
    {
        if(checkIfMusicButtonExists != null && checkIfSFXButtonExists != null)
        {
            checkIfMusicButtonExists(musicButton);
            checkIfSFXButtonExists(gameSoundButton);
        }
        currentTxt = GameObject.Find("SettingsCanvas/QualityButton/CurrentQuality").GetComponent<Text>();
        qualityLevel = QualitySettings.GetQualityLevel();
        currentTxt.text = QualitySettings.names[qualityLevel];

    }

    // Change game quality
    public void ChangeQuality()
    {
        AudioManager.instance.PlaySFXClip(clickSFX, transform, clickVolume);
        string[] qualities = QualitySettings.names;
        int qualityLevel = QualitySettings.GetQualityLevel();
        if(qualityLevel + 1 < qualities.Length)
        {
            QualitySettings.SetQualityLevel(qualityLevel + 1, true);
            currentTxt.text = QualitySettings.names[qualityLevel+1];
        }
        else
        {
            qualityLevel = 0;
            QualitySettings.SetQualityLevel(qualityLevel, true);
            currentTxt.text = QualitySettings.names[qualityLevel];
        }
    }

}
