using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreas : MonoBehaviour
{
    private ScoreManager pointValue;

    // Start is called before the first frame update
    void Start()
    {
        pointValue = GameObject.Find("CalculatePoints").GetComponent<ScoreManager>();
    }
    // Add points when ball enters area
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Balls")
        {
            ScoreManager.Points = pointValue.PointValue(gameObject.tag);
        }
    }
    // Subtract points when ball exits from the area 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Balls")
        {
            ScoreManager.Points = -pointValue.PointValue(gameObject.tag);
        }
    }
}
