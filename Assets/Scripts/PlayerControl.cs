using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    private float factor = 2500f;        // force can be adjusted with this
    private float touchTimeStart, touchTimeFinish; // count holding time
    private float force;
    public Slider powerbar;
    public Image powerbarImage;
    Vector3 SphereStartPos;
    private GameObject player;
    private Array ballsArray;
    private List<GameObject> balls = new List<GameObject>();
    private static bool isInStartArea;         // if true, ball is still in start area and so if it's launched too slow and it comes back and stops, new ball won't come active
    private static bool atZeroPointArea;       // if true, next ball can move to the launch area even if previous ball is still moving
    private static bool isActive;             // if true, ball is ready to launch, otherwise launch is disabled
    private static bool outOfBounds;    // checks if ball is in the game area
    private bool isBouncedOutZeroArea; // if ball bounces off zeropointarea, game doesn't put second ball to game when this ball enters zeropointarea again
    private int i;              // to keep track of the active ball in the list
    private readonly float forceLimit = 3.5f; // if force is higher than limit, it is set to the limit set here
    //private List<Rigidbody> rb = new List<Rigidbody>();
    private List<bool> isStopped = new List<bool>();
    [SerializeField] private AudioClip bonkSFX;
    [SerializeField] private AudioClip bonkNailSFX;
    [SerializeField] private AudioClip rollingBallSFX;
    private readonly float bonkVolume = AudioManager.BonkVolume;
    private readonly float bonkNailVolume = AudioManager.BonkNailVolume;
    [SerializeField] private AudioSource audiosource;
    private float rollingBallVolume;
    private float maxSpeed = 80f;
    public AnimationCurve volumeCurve;
    public AnimationCurve pitchCurve;

    // Check if tutorial is needed
    public delegate void OnTutorialSwitchChanged(bool sw);
    public static event OnTutorialSwitchChanged onTutorialSwitchChanged;

    // Checks when game is ended
    public delegate void OnGameEnded();
    public static event OnGameEnded onGameEnded;


    //Assigned at start
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ballsArray = GameObject.FindGameObjectsWithTag("Balls");
        balls = ballsArray.OfType<GameObject>().ToList();
        i = 0;
        SphereStartPos = balls[i].transform.position;
        powerbar.value = 0;
        powerbar.maxValue = forceLimit;
        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
        isBouncedOutZeroArea = false;
        SwitchBall();
    }
    // Update game every frame to check inputs and outcomes
    void FixedUpdate()
    {
        // Check launch force if ball is in start area
        if(isActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchTimeStart = Time.time;
            } 
            if(Input.GetMouseButtonUp(0))
            {
                if(touchTimeStart > 0)
                {
                    touchTimeFinish = Time.time;
                    LaunchBall();
                }
            }
            // Set powerbar value
            if(touchTimeStart > 0)
            {
                powerbar.value += Time.deltaTime;
                if(powerbar.value >= forceLimit)
                {
                    powerbar.value = forceLimit;
                }
                powerbarImage.color = Color.Lerp(Color.green, Color.red, powerbar.value / forceLimit);
            }
        }
        else
        {
            powerbar.value = 0;
            touchTimeStart = 0;
            touchTimeFinish = 0;
        }
        // Add points to the counter on screen and set up next ball
        if (atZeroPointArea || i <= balls.Count && !isInStartArea && player.GetComponent<Rigidbody>().velocity == Vector3.zero && player.GetComponent<Rigidbody>().angularVelocity == Vector3.zero)
        {
            ScoreManager.SetText();
            // Add own rigidbody to ball so it will continue moving after it is detached from player
            balls[i-1].AddComponent<Rigidbody>();
            balls[i-1].GetComponent<Rigidbody>().mass = 2f;
            balls[i-1].GetComponent<Rigidbody>().angularDrag = 0.5f;
            balls[i-1].GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity;
            balls[i-1].GetComponent<Rigidbody>().angularVelocity = player.GetComponent<Rigidbody>().angularVelocity;
            balls[i-1].transform.parent = player.transform.parent;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            isInStartArea = true;
            atZeroPointArea = false;
            Destroy(audiosource);
            if(i >= balls.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            else
            {
                SwitchBall();
            }
        }
        // Check if ball is out of bounds and set up next ball
        if (outOfBounds)
        {
            balls[i-1].transform.parent = player.transform.parent;
            Destroy(balls[i-1]);
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            Debug.Log("Ball is out of Bounds");
            if(i >= balls.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            SwitchBall();
        }

        //Audio for ball rolling
        var speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        if(!audiosource.isPlaying && speed >= 0.1f && GameMenu.IsPaused == false)
        {
            var scaledVelocity = Remap(Mathf.Clamp(speed, 0, maxSpeed), 0, maxSpeed, 0, 1);
            // set volume based on volume curve
            rollingBallVolume = volumeCurve.Evaluate(scaledVelocity);
            // set pitch based on pitch curve
            float pitch = pitchCurve.Evaluate(scaledVelocity);
            audiosource.volume = rollingBallVolume;
            audiosource.pitch = pitch;
            audiosource.Play();
        }
    }

     // https://discussions.unity.com/t/465623
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
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
        player.GetComponent<Rigidbody>().AddForce(0,0,force*factor);
        touchTimeStart = 0;
        touchTimeFinish = 0;
        powerbar.value = 0;
    }
    // Switch ball or start ending the game
    private void SwitchBall()
    {   
        atZeroPointArea = false;
        isStopped.Add(false);
        balls[i].transform.parent = player.transform;
        player.transform.GetChild(0).transform.position = SphereStartPos;
        audiosource = Instantiate(audiosource, balls[i].transform);
        isActive = true;
        i++;
    }

    // Check that every ball is stopped so all the points are counted
    private IEnumerator CheckEndGame()
    {
        int j = 0;
        foreach(GameObject _go in balls)
        {
            if(player.GetComponent<Rigidbody>().velocity == Vector3.zero && player.GetComponent<Rigidbody>().angularVelocity == Vector3.zero)
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
            if(onGameEnded != null) 
            {
                onGameEnded();
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(CheckEndGame());
        }
        yield return null;
    }
    // Check if ball enters to these triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "StartingArea")
        {
            isInStartArea = true;
            isActive = true;
        }
        // When ball enters (second time) zeropointarea, put trigger on
        if (other.transform.tag == "ZeroPointArea")
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
        if (other.transform.tag == "StartingArea")
        {
            isInStartArea = false;
            isActive = false;
            if(onTutorialSwitchChanged != null) 
            {
                onTutorialSwitchChanged(false);
            }
        }
        if (other.transform.tag == "GameArea")
        {
            outOfBounds = true;
        }
        // // When ball exits zeropointarea, mark it so when ball drops back there second time, it wont trigger extra ball to game
        if(other.transform.tag == "ZeroPointArea")
        {
           isBouncedOutZeroArea = true;
        }
    }
}