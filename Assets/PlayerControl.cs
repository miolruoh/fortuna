using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Vector3 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    [Range(0.05f, 1f)]
    public float throwForce = 0.3f;
    [Range(1f, 10f)]
    public float v = 5f;

    Vector3 SpherestartPos;

    public Rigidbody ball1;
    public Rigidbody ball2;
    public Rigidbody ball3;
    public Rigidbody ball4;
    public Rigidbody ball5;
    List<Rigidbody> rbList = new List<Rigidbody>();
    private bool isInStartArea;
    private bool isActive = false;
    private int i;

    private Rigidbody rb;

    private void Start()
    {
        rbList.Add(ball1);
        rbList.Add(ball2);
        rbList.Add(ball3);
        rbList.Add(ball4);
        rbList.Add(ball5);
        rb = rbList[i];
        i = 0;
        isActive = true;
        SpherestartPos = rb.transform.position;
    }


    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && isActive)
        {
            rb.velocity = new Vector3(0, 0, v);
            isActive = false;
            i++;
        }
        if ( !isInStartArea && rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            if(i < rbList.Count)
            {
                rb = rbList[i];
                rb.position = SpherestartPos;
                isActive = true;
            }
        }

        /* if (Input.GetMouseButtonDown(0))
         {
             touchTimeStart = Time.time;
             startPos = new Vector3(rb.position.x, rb.position.y, Input.mousePosition.z);
         }
         if (Input.GetMouseButtonUp(0))
         {
             touchTimeFinish = Time.time;
             timeInterval = touchTimeFinish - touchTimeStart;
             endPos = new Vector3(rb.position.x, rb.position.y, Input.mousePosition.z);
             direction = startPos - endPos;
             Vector3 force = direction / timeInterval * throwForce;
             if (force.z > 0)
             {
                 force = new Vector3(0, 0, 0);
             }
             else rb.AddForce(force);
         }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "StartingArea")
        {
            isInStartArea = true;
            Debug.Log("Enter" + i);
            Debug.Log(isInStartArea + " " + i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "StartingArea")
        {
            isInStartArea = false;
            Debug.Log("Stay" + i);
            Debug.Log(isInStartArea + " " + i);
        }
    }


    private void OnButtonPress()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        rb.position = SpherestartPos;
    }
}
