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
        if (other.gameObject.tag == "Player")
        {
            ScoreManager.Points = pointValue.PointValue(gameObject.tag);
        }
    }
}
