using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class SettingsControl : MonoBehaviour
{
    private Text currentTxt;
    private int qualityLevel;
    public Sprite volumeOnIcon;
    public Sprite volumeOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;
    public Button volumeButton;
    public Button musicButton;
    public Slider musicSlider;
    public Slider volumeSlider;
    public GameObject musicPlayer;
    private bool toggleVolume;
    private bool toggleMusic;
    private float previousVolumeValue;
    private float previousMusicValue;

    // Start is called before the first frame update
    void Start()
    {
        currentTxt = GameObject.Find("SettingsCanvas/QualityButton/CurrentQuality").GetComponent<Text>();
        qualityLevel = QualitySettings.GetQualityLevel();
        currentTxt.text = QualitySettings.names[qualityLevel];
        toggleVolume = true;
        toggleMusic = true;
    }

    public void OnClickVolume()
    {
        if(toggleVolume)
        {
            previousVolumeValue = volumeSlider.value;
            volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
            volumeSlider.value = 0;
            toggleVolume = false;
        }
        else
        {
            volumeButton.GetComponent<Image>().sprite = volumeOnIcon;
            volumeSlider.value = previousVolumeValue;
            toggleVolume = true;
        }
    }

    
    public void OnClickMusic()
    {
        if(toggleMusic)
        {
            previousMusicValue = musicSlider.value;
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            musicSlider.value = 0;
            toggleMusic = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            musicSlider.value = previousMusicValue;
            toggleMusic = true;
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
