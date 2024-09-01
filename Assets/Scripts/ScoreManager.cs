using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private static TextMeshProUGUI output_Points; // points in textmeshprougui ( points that is seen in the top corner)
    private static int points;   // points in int

    public static string Final_Points      // points that is seen in the endmenu
    {
        get {return points.ToString();}
    }
    public static int Points
    {
        get {return points;}
        set 
        {
            points += value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        output_Points = GameObject.Find("ScoreCanvas/Score").GetComponent<TextMeshProUGUI>();
    }

    // Sets Score to the screen
    public static void SetText()
    {
        output_Points.text = points.ToString();
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
