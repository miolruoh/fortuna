using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreas : MonoBehaviour
{
    private int areaTriggered = 0;

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
            areaTriggered++;
            Debug.Log("AreaTriggered " + areaTriggered);
            if (areaTriggered != 1 && areaTriggered % 2 == 0)
            {
                AddPoints();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (areaTriggered != 1 && areaTriggered % 2 == 0 && areaTriggered > 0)
            {
                SubtractPoints();
            }
            areaTriggered--;
            Debug.Log("AreaTriggered " + areaTriggered);
        }
    }

    private void SubtractPoints()
    {
        Debug.Log("Subtract Area Points");
    }

    private void AddPoints()
    {
        Debug.Log("Add Area Points");
    }
}
