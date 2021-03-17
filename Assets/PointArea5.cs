using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointArea5 : MonoBehaviour
{
    ushort triggered = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            triggered++;
            Debug.Log(triggered);
            Debug.Log("Inside PointArea5: Add Points / 2");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggered--;
            Debug.Log(triggered);
            Debug.Log("Inside PointArea5: Subtract Points / 2");
        }
    }

    // Update is called once per frame
    void Update()
    {
       /* if (triggered == 2)
        {
            //inside cylinder collider
            
        }
        else
        {
            //outside cylinder collider
        }*/
    }
}
