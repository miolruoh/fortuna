using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreas : MonoBehaviour
{
    private CheckPoints checkPoints;

    // Start is called before the first frame update
    void Start()
    {
        checkPoints = GameObject.Find("CalculatePoints").GetComponent<CheckPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            int value =  checkPoints.CalculatePoints(gameObject.tag);
            AddPoints(value);
        }
    }

    private void AddPoints(int value)
    {
        checkPoints.points += value;
        checkPoints.SetText();
    }
}
