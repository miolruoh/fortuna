using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Range(0.05f, 1f)]
    private float factor = 0.075f;        // force can be adjusted with this

    private Vector2 startPos;         // place where dragging starts
    private Vector2 endPos;           // place where dragging ends
    private float touchTimeStart, touchTimeFinish; // count dragging time
    private float force;       // calculates how much force is added to ball when launching
    Vector3 SphereStartPos;
    public List<Rigidbody> rbList = new List<Rigidbody>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private int i = 0;              // to keep track of the active ball in the list
    private readonly int forceLimit = 10000; // if force is higher than limit, it is set to the limit set here
    private Rigidbody rb;
    public static bool endGame; 

    //Assigned at start
    private void Start()
    {
        rb = rbList[i];
        SphereStartPos = rb.transform.position;

        endGame = false;
        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
    }

    void Update()
    {
        if (isActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchTimeStart = Time.time;
                startPos = Input.mousePosition;
            } 
            else if(Input.GetMouseButtonUp(0))
            {
                touchTimeFinish = Time.time;
                endPos = Input.mousePosition;
                if(touchTimeFinish - touchTimeStart != 0)
                {
                    force = (endPos.y - startPos.y) / (touchTimeFinish - touchTimeStart);
                }
                else
                {
                    force = 0;
                }
                if(force <= 0)
                {
                    force = 0;
                    isActive = true;
                }
                else if (force > forceLimit)
                {
                    force = forceLimit;
                    rb.AddForce(0, 0, force * factor);
                }
                else
                {
                    rb.AddForce(0, 0, force * factor);
                }
            }
        }

        if (atZeroPointArea || (!isInStartArea && rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero))
        {
            ScoreManager.SetText();
            isInStartArea = true;
            atZeroPointArea = false;
            StartCoroutine(SwitchBall());
        }

        if (outOfBounds)
        {
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            Debug.Log("Ball is out of Bounds");
            StartCoroutine(SwitchBall());
        }
    }

    private IEnumerator SwitchBall()
    {   
        i++;
        atZeroPointArea = false;
        if (i < rbList.Count)
        {
            rb = rbList[i];
            rb.position = SphereStartPos;
            isActive = true;
        } 
        else 
        {
            yield return new WaitForSeconds(1.0f);
            endGame = true;
        }
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StartingArea")
        {
            isInStartArea = true;
            isActive = true;
        }

        if (other.gameObject.tag == "ZeroPointArea")
        {
            atZeroPointArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "StartingArea")
        {
            isInStartArea = false;
            isActive = false;
        }

        if (other.gameObject.tag == "GameArea")
        {
            outOfBounds = true;
        }
    }
}