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
    Vector3 SphereStartPos;
    public List<GameObject> balls = new List<GameObject>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private bool isBouncedOutZeroArea; // if ball bounces off zeropointarea, game doesn't put second ball to game when this ball enters zeropointarea again
    private int i;              // to keep track of the active ball in the list
    private readonly float forceLimit = 3f; // if force is higher than limit, it is set to the limit set here
    private List<Rigidbody> rb = new List<Rigidbody>();
    private List<bool> isStopped = new List<bool>();
    public static bool endGame;

    // Check if tutorial is needed
    public delegate void OnTutorialSwitchChanged(bool sw);
    public static event OnTutorialSwitchChanged onTutorialSwitchChanged;

    //Assigned at start
    private void Start()
    {
        i = 0;
        SphereStartPos = balls[i].transform.position;
        powerbar.value = 0;
        powerbar.maxValue = forceLimit;
        endGame = false;
        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
        isBouncedOutZeroArea = false;
        for(int j = 0; j < balls.Count; j++)
        {
            rb.Add(balls[j].GetComponent<Rigidbody>());
            isStopped.Add(false);
        }
        SwitchBall();
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
            } 
            if(Input.GetMouseButtonUp(0))
            {
                if(touchTimeStart != 0)
                {
                    touchTimeFinish = Time.time;
                    powerbar.value = 0;
                    LaunchBall();
                }
            }
        }
        // Set launch values to zero when leaving startarea
        if(!isInStartArea)
        {
            powerbar.value = 0;
            touchTimeStart = 0;
            touchTimeFinish = 0;
            if(onTutorialSwitchChanged != null) 
            {
                onTutorialSwitchChanged(false);
            }
        }
        // Set powerbar value
        if(touchTimeStart != 0)
        {
            powerbar.value += Time.deltaTime;
            if(powerbar.value >= forceLimit)
            {
                powerbar.value = forceLimit;
            }
            powerbarImage.color = Color.Lerp(Color.green, Color.red, powerbar.value / forceLimit);
        }
        // Add points to the counter on screen and set up next ball
        if (atZeroPointArea || !isInStartArea && rb[i-1].velocity == Vector3.zero && rb[i-1].angularVelocity == Vector3.zero)
        {
            ScoreManager.SetText();
            isInStartArea = true;
            atZeroPointArea = false;
            if(i >= rb.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            SwitchBall();
        }
        // Check if ball is out of bounds and set up next ball
        if (outOfBounds)
        {
            Destroy(rb[i-1]);
            balls[i-1].SetActive(false);
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            Debug.Log("Ball is out of Bounds");
            if(i >= rb.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            SwitchBall();
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

        rb[i-1].AddForce(0,0,force*factor);
        touchTimeStart = 0;
        touchTimeFinish = 0;
    }
    // Switch ball or start ending the game
    private void SwitchBall()
    {   
        atZeroPointArea = false;
        if (i < rb.Count)
        {
            rb[i].transform.position = SphereStartPos;
            isActive = true;
        }
        i++;
    }

    // Check that every ball is stopped so all the points are counted
    private IEnumerator CheckEndGame()
    {
        int j = 0;
        foreach(Rigidbody _rb in rb)
        {
            if(_rb.velocity == Vector3.zero && _rb.angularVelocity == Vector3.zero)
            {
                isStopped[j] = true;
            }
            else
            {
                isStopped[j] = false;
            }
            j++;
        }
        if(!isStopped.Contains(false))
        {
            endGame = true;
        }
        else
        {
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(CheckEndGame());
        }
        yield return null;
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