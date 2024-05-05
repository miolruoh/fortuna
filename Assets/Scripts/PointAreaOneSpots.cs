using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreaOneSpots : MonoBehaviour
{
    private ScoreManager pointValue;

    // Start is called before the first frame update
    void Start()
    {
        pointValue = GameObject.Find("CalculatePoints").GetComponent<ScoreManager>();
    }

    // Ball goes to the spot // Add points
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ScoreManager.Points = pointValue.PointValue(gameObject.tag);
        }
    }
    // Ball exits from the spot // Subtract points
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ScoreManager.Points = -(pointValue.PointValue(gameObject.tag));
        }
    }
}
