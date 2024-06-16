using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Range(10f, 1000f)]
    private float factor = 1000f;        // force can be adjusted with this


    private float touchTimeStart, touchTimeFinish; // count dragging time
    private float force;
    public Slider powerbar;
    private bool isPressed = false;       // calculates how much force is added to ball when launching
    Vector3 SphereStartPos;
    public List<GameObject> balls = new List<GameObject>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private int i = 0;              // to keep track of the active ball in the list
    private readonly float forceLimit = 7f; // if force is higher than limit, it is set to the limit set here
    private Rigidbody rb;
    public static bool endGame; 

    //Assigned at start
    private void Start()
    {
        rb = balls[i].GetComponent<Rigidbody>();
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
                isPressed = true;
                PowerBar();
            } 
            if(Input.GetMouseButtonUp(0))
            {
                touchTimeFinish = Time.time;
                isPressed = false;
                LaunchBall();
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
            Destroy(rb);
            balls[i].SetActive(false);
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            Debug.Log("Ball is out of Bounds");
            StartCoroutine(SwitchBall());
        }
    }

    private void PowerBar()
    {
        while(isPressed)
        {
            powerbar.value += forceLimit * 0.01f;
            if(powerbar.value  == 1)
            {
                break;
            }
        }
    }

    private void LaunchBall()
    {
        force = touchTimeFinish - touchTimeStart;
        if(force <= 0)
        {
            force = 0;
        }
        else if(force > forceLimit)
        {
            force = forceLimit;
        }

        rb.AddForce(0,0,force*factor);
    }

    private IEnumerator SwitchBall()
    {   
        i++;
        atZeroPointArea = false;
        if (i < balls.Count)
        {
            rb = balls[i].GetComponent<Rigidbody>();
            rb.transform.position = SphereStartPos;
            isActive = true;
        } 
        else 
        {

            yield return new WaitForSeconds(0.5f);
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