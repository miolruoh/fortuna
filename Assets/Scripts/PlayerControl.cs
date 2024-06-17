using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Range(10f, 10000f)]
    private float factor = 1300f;        // force can be adjusted with this

    private float touchTimeStart, touchTimeFinish; // count dragging time
    private float force;
    public Slider powerbar;
    public Image powerbarImage;
    private bool isPressed = false;       // calculates how much force is added to ball when launching
    Vector3 SphereStartPos;
    public List<GameObject> balls = new List<GameObject>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private int i = 0;              // to keep track of the active ball in the list
    private readonly float forceLimit = 4f; // if force is higher than limit, it is set to the limit set here
    private Rigidbody rb;
    public static bool endGame; 

    //Assigned at start
    private void Start()
    {
        SphereStartPos = balls[0].transform.position;
        StartCoroutine(SwitchBall());

        powerbar.maxValue = forceLimit;
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
            } 
            if(Input.GetMouseButtonUp(0))
            {
                touchTimeFinish = Time.time;
                isPressed = false;
                powerbar.value = 0;
                LaunchBall();
            }
        }
        if(isPressed)
        {
            if(powerbar.value >= forceLimit)
            {
                powerbar.value = forceLimit;
            }
            else
            {
                powerbar.value += Time.deltaTime;
            }
            powerbarImage.color = Color.Lerp(Color.green, Color.red, powerbar.value / forceLimit);
        }

        if ( (atZeroPointArea || !isInStartArea && rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero))
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
        if (i < balls.Count)
        {
            rb = balls[i].GetComponent<Rigidbody>();
            rb.transform.position = SphereStartPos;
            isActive = true;
            i++;
        } 
        else 
        {
            CheckEndGame();
        }
        atZeroPointArea = false;
        yield return null;
    }

    private void CheckEndGame()
    {
            for(int j = 0; j < balls.Count; j++)
            {
                Rigidbody rb_ = balls[j].GetComponent<Rigidbody>();
                if (!(rb_.velocity == Vector3.zero) || !(rb_.angularVelocity == Vector3.zero))
                {
                    endGame = false;
                    StartCoroutine(SwitchBall());
                }
            }
            endGame = true;
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
        if (other.gameObject.tag == "ZeroPointArea")
        {
            Debug.Log(balls[i] + " collider poistetaan");
            balls[i].GetComponent<Collider>().enabled = false;
        }
    }
}