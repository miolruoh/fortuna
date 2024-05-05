using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int points;
    private TextMeshProUGUI output_Points;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        output_Points = GameObject.Find("ScoreCanvas/Score").GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    // Sets Score to the screen
    public void SetText()
    {
        string score = points.ToString();
        output_Points.text = score;
    }

    // returns the amount of points given from the spot
    public int CalculatePoints(string tag)
    {
        switch (tag)
        {
            case "10Points":
                return 10;
            case "20Points":
                return 20;
            case "25Points":
                return 25;
            case "50Points":
                return 50;
            case "75Points":
                return 75;
            case "100Points":
                return 100;
            case "125Points":
                return 125;
            case "150Points":
                return 150;
            default:
                Debug.Log("Something went wrong");
                return 0;
        }
    }
}
