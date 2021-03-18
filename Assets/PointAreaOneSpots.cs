using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreaOneSpots : MonoBehaviour
{
    private int triggered = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggered++;
            Debug.Log("Triggered " + triggered);
            if (triggered != 1 && triggered % 2 == 0)
            {
                AddPoints();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (triggered != 1 && triggered % 2 == 0 && triggered > 0)
            {
                SubtractPoints();
            }
            triggered--;
            Debug.Log("Triggered " + triggered);
        }
    }

    private void SubtractPoints()
    {
        Debug.Log("Subtract Points");
    }

    private void AddPoints()
    {
        Debug.Log("Add Points");
    }
}
