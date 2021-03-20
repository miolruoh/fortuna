using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreas : MonoBehaviour
{
    private static int areaAddTriggered = 0;
    private static int areaSubtractTriggered = 0;

    public int totalTriggered = 0;
    public int oldTotal = 0;

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
            areaAddTriggered++;
            Debug.Log("AreaAddTriggered " + areaAddTriggered);
            CalculateTriggered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            areaSubtractTriggered++;
            Debug.Log("AreaSubtractTriggered " + areaSubtractTriggered);
            CalculateTriggered();
        }
    }

    private void CalculateTriggered()
    {
        if (areaSubtractTriggered <= totalTriggered + areaAddTriggered)
        {
            oldTotal = totalTriggered;
            totalTriggered = areaAddTriggered - areaSubtractTriggered;
        }
        else totalTriggered = 0;     // if working correct, this will never happen

        if (totalTriggered > 1 && totalTriggered % 2 == 0)
        {
            PointSystem();
        }
    }
    // TODO: if goes through area, it will subtract points and it shouldn't. modify ifs' to not accept that
    private void PointSystem()
    {
        if (oldTotal < totalTriggered)
        {
            AddPoints();
        }
        else SubtractPoints();
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
