using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class SettingsContol : MonoBehaviour
{
    private Text currentTxt;
    private int qualityLevel;
    public Sprite volumeOnIcon;
    public Sprite volumeOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;
    public Button volumeButton;
    public Button musicButton;
    private bool toggleVolume;
    private bool toggleMusic;


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
            volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
            toggleVolume = false;
        }
        else
        {
            volumeButton.GetComponent<Image>().sprite = volumeOnIcon;
            toggleVolume = true;
        }
    }

    
    public void OnClickMusic()
    {
        if(toggleMusic)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            toggleMusic = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
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
