using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI output_Points;
    private static int points;
    private static int old_points;
    private static string final_points;

    public static string Final_Points
    {
        get {return final_points;}
    }
    public static int Points
    {
        get {return points;}
        set 
        {
            old_points = points;
            points += value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        old_points = 0;
        output_Points = GameObject.Find("ScoreCanvas/Score").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(old_points != points)
        {
            SetText();
        }
    }

    // Sets Score to the screen
    public void SetText()
    {
        output_Points.text = points.ToString();
        if(PlayerControl.endGame)
        {
            final_points = output_Points.text;
        }
    }

    // returns the amount of points given from the spot
    public int PointValue(string tag)
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
