using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    private Text currentTxt;
    private int qualityLevel;
    public Sprite gameSoundOnIcon;
    public Sprite gameSoundOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;
    public Button gameSoundButton;
    public Button musicButton;

    // Start is called before the first frame update
    void Start()
    {
        currentTxt = GameObject.Find("SettingsCanvas/QualityButton/CurrentQuality").GetComponent<Text>();
        qualityLevel = QualitySettings.GetQualityLevel();
        currentTxt.text = QualitySettings.names[qualityLevel];

        /*if(PlayerPrefs.GetInt("IsMusicMuted", 0) == 0)
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
        }*/
    }

    public void OnClickGameSound()
    {
        if(Music.GameSoundOn)
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOffIcon;
            Music.GameSoundOn = false;
        }
        else
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOnIcon;
            Music.GameSoundOn = true;
        }
    }
    public void OnClickMusic()
    {
        if(Music.MusicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            Music.MuteMusic();
            Music.MusicOn = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            Music.MuteMusic();
            Music.MusicOn = true;
        }
    }
    

    // Change game quality
    public void ChangeQuality()
    {
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
