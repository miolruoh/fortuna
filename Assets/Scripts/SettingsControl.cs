using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private static Slider musicSlider;
    public static float MusicValue
    {
        get {return musicSlider.value;}
        set 
        {
            musicSlider.value = value;
        }
    }
    private static Slider volumeSlider;
    public static float VolumeValue
    {
        get {return volumeSlider.value;}
        set 
        {
            volumeSlider.value = value;
        }
    }
    public GameObject musicPlayer;
    private static bool volumeOn;
    public static bool VolumeOn
    {
        get {return volumeOn;}
        set 
        {
            volumeOn = value;
        }
    }
    private static bool musicOn;
    public static bool MusicOn
    {
        get {return musicOn;}
        set 
        {
            musicOn = value;
        }
    }
    private float previousVolumeValue;
    private float previousMusicValue;

    // Start is called before the first frame update
    void Start()
    {
        currentTxt = GameObject.Find("SettingsCanvas/QualityButton/CurrentQuality").GetComponent<Text>();
        List<Slider> sliders = new List<Slider>(GameObject.FindObjectsOfType<Slider>());
        volumeSlider = sliders[1];
        musicSlider = sliders[0];
        qualityLevel = QualitySettings.GetQualityLevel();
        currentTxt.text = QualitySettings.names[qualityLevel];
        CheckVolume();
        CheckMusic();
    }
    public void VolumeValueChanged()
    {
        PlayerPrefs.SetFloat("VolumeValueChanged", volumeSlider.value);
        if(volumeSlider.value == 0)
        {
            volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
            VolumeOn = false;
        }
        else
        {
            volumeButton.GetComponent<Image>().sprite = volumeOnIcon;
            VolumeOn = true;
        }
    }

    public void MusicValueChanged()
    {
        PlayerPrefs.SetFloat("MusicValueChanged", musicSlider.value);
        if(musicSlider.value == 0)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            MusicOn = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            MusicOn = true;
        }
    }
    private void CheckVolume()
    {   
        volumeSlider.value = PlayerPrefs.GetFloat("VolumeValueChanged", volumeSlider.value);
        if(volumeSlider.value != 0)
        {
            VolumeOn = true;
            volumeButton.GetComponent<Image>().sprite = volumeOnIcon;
        }
        else
        {
            VolumeOn = false;
            volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
        }
    }
    private void CheckMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicValueChanged", musicSlider.value);
        if(musicSlider.value != 0)
        {
            MusicOn = true;
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
        }
        else
        {
            MusicOn = false;
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
        }
    }
    public void OnClickVolume()
    {
        if(VolumeOn)
        {
            previousVolumeValue = volumeSlider.value;
            volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
            volumeSlider.value = 0;
            VolumeOn = false;
        }
        else
        {
            volumeSlider.value = previousVolumeValue;
            if(volumeSlider.value != 0)
            {
                VolumeOn = true;
                volumeButton.GetComponent<Image>().sprite = volumeOnIcon;
            }
            else
            {
                VolumeOn = false;
                volumeButton.GetComponent<Image>().sprite = volumeOffIcon;
            }
        }
    }

    
    public void OnClickMusic()
    {
        if(MusicOn)
        {
            previousMusicValue = musicSlider.value;
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            musicSlider.value = 0;
            MusicOn = false;
        }
        else
        {
            musicSlider.value = previousMusicValue;
            if(musicSlider.value != 0)
            {
                MusicOn = true;
                musicButton.GetComponent<Image>().sprite = musicOnIcon;
            }
            else
            {
                MusicOn = false;
                musicButton.GetComponent<Image>().sprite = musicOffIcon;
            }
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
