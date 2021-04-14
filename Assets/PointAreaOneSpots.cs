using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreaOneSpots : MonoBehaviour
{
    private ScoreManager checkPoints;

    // Start is called before the first frame update
    void Start()
    {
        checkPoints = GameObject.Find("CalculatePoints").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Ball goes to the spot
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name + " Add");
            int value = checkPoints.CalculatePoints(gameObject.tag);
            AddPoints(value);
        }
    }
    // Ball exits from the spot
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name + " Subtract");
            int value = checkPoints.CalculatePoints(gameObject.tag);
            SubtractPoints(value);
        }
    }

    private void SubtractPoints(int value)
    {
        checkPoints.points -= value;
        checkPoints.SetText();
    }

    private void AddPoints(int value)
    {
        checkPoints.points += value;
        checkPoints.SetText();
    }
}
