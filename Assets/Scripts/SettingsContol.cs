using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsContol : MonoBehaviour
{
    private Text currentTxt;
    private int qualityLevel;


    // Start is called before the first frame update
    void Start()
    {
        currentTxt = GameObject.Find("SettingsCanvas/QualityButton/CurrentQuality").GetComponent<Text>();
        qualityLevel = QualitySettings.GetQualityLevel();
        currentTxt.text = QualitySettings.names[qualityLevel];
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
