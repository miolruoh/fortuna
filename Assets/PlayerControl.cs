using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Range(0.05f, 1f)]
    public float factor = 0.1f;        // force can be adjusted with this

    private Vector2 startPos;         // place where dragging starts
    private Vector2 endPos;           // place where dragging ends
    private float touchTimeStart, touchTimeFinish; // count dragging time
    private float force;       // calculates how mmuch force is added to ball when launching

    Vector3 SphereStartPos;

    //public Rigidbody ball1;
    //public Rigidbody ball2;
    //public Rigidbody ball3;
    //public Rigidbody ball4;
    //public Rigidbody ball5;
    public List<Rigidbody> rbList = new List<Rigidbody>();

    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launch too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even though if ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private int i = 0;               // to keep track of the active ball in the list
    private readonly int forceLimit = 7000;

    private Rigidbody rb;

    //Assigned in start
    private void Start()
    {
        //rbList.Add(ball1);
        //rbList.Add(ball2);
        //rbList.Add(ball3);
        //rbList.Add(ball4);
        //rbList.Add(ball5);
        rb = rbList[i];
        SphereStartPos = rb.transform.position;

        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
    }

    void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchTimeStart = Time.time;
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touchTimeFinish = Time.time;
                endPos = Input.mousePosition;
                force = (endPos.y - startPos.y) / (touchTimeFinish - touchTimeStart);

                if ((touchTimeFinish - touchTimeStart) == 0 || force < 0)
                {
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

    // Reset scene (probably going in to own script)
    private void OnButtonPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}