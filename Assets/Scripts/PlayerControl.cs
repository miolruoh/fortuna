using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private float factor = 1400f;        // force can be adjusted with this

    private float touchTimeStart, touchTimeFinish; // count holding time
    private float force;
    public Slider powerbar;
    public Image powerbarImage;
    private bool isPressed = false;       // if true, start powerbar
    Vector3 SphereStartPos;
    public List<GameObject> balls = new List<GameObject>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private bool isBouncedOutZeroArea; // if ball bounces off zeropointarea, game doesn't put second ball to game when this ball enters zeropointarea again
    private int i;              // to keep track of the active ball in the list
    private readonly float forceLimit = 3f; // if force is higher than limit, it is set to the limit set here
    private Rigidbody rb;
    public static bool endGame; 

    //Assigned at start
    private void Start()
    {
        i = 0;
        SphereStartPos = balls[i].transform.position;
        StartCoroutine(SwitchBall());
        powerbar.value = 0;
        powerbar.maxValue = forceLimit;
        endGame = false;
        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
        isBouncedOutZeroArea = false;
    }
    // Update game every frame to check inputs and outcomes
    void Update()
    {
        // Check launch force if ball is in start area
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
        if(!isInStartArea)
        {
            isPressed = false;
            powerbar.value = 0;
        }
        // Set powerbar value
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
        // Add points to the counter on screen and set up next ball
        if ((atZeroPointArea || !isInStartArea && rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero))
        {
            ScoreManager.SetText();
            isInStartArea = true;
            atZeroPointArea = false;
            StartCoroutine(SwitchBall());
        }
        // Check if ball is out of bounds and set up next ball
        if (outOfBounds)
        {
            Destroy(rb);
            balls[i-1].SetActive(false);
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            Debug.Log("Ball is out of Bounds");
            StartCoroutine(SwitchBall());
        }
    }
    // Add force to launch the ball
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
    // Switch ball or start ending the game
    private IEnumerator SwitchBall()
    {   
        atZeroPointArea = false;
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
            i--;
        }
        yield return null;
    }
    // TODO not working, might also give error
    // Check if game is ready to end (every ball is stopped so all the points are counted)
    private void CheckEndGame()
    {
        while(!endGame)
        {
            List<bool> boolCount = new List<bool>();
            bool stopped;
            for(int j = 0; j < balls.Count; j++)
            {
                Rigidbody rb_ = balls[j].GetComponent<Rigidbody>();
                if ((rb_.velocity == Vector3.zero) || (rb_.angularVelocity == Vector3.zero))
                {
                    stopped = true;
                    boolCount.Add(stopped);
                }
                else
                {
                    stopped = false;
                    boolCount.Add(stopped);
                }
            }
            foreach (bool b in boolCount)
            {
                if(!b)
                {
                    endGame = false;
                    break;
                }
            }
            endGame = true;
        }
    }
    // Check if ball enters to these triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StartingArea")
        {
            isInStartArea = true;
            isActive = true;
        }
        // When ball enters (second time) zeropointarea, put trigger on
        if (other.gameObject.tag == "ZeroPointArea")
        {
            if(!isBouncedOutZeroArea)
            {
                atZeroPointArea = true;
            }
            isBouncedOutZeroArea = false;
        }
    }
    // Check if ball exits from these triggers
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
        // // When ball exits zeropointarea, mark it so when ball drops back there second time, it wont trigger extra ball to game
        if(other.gameObject.tag == "ZeroPointArea")
        {
           isBouncedOutZeroArea = true;
        }
    }
}