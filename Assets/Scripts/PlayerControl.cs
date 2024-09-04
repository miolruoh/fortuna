using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    private float factor = 2200f;        // force can be adjusted with this
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
    private readonly float forceLimit = 3f; // if force is higher than limit, it is set to the limit set here
    private List<bool> isStopped = new List<bool>();
    // For audio
    [SerializeField] private AudioClip bonkSFX;
    [SerializeField] private AudioClip bonkNailSFX;
    private readonly float bonkVolume = AudioManager.BonkVolume;
    private readonly float bonkNailVolume = AudioManager.BonkNailVolume;
    [SerializeField] private AudioSource audiosource;
    private float maxSpeed = 50f;
    private float minPitch = 0.9f;
    private float maxPitch = 2.5f;
    private float maxVolume = 1f;
    private float minVolume = 0.1f;
    private float speed;
    //

    // Check if tutorial is needed
    public delegate void OnTutorialSwitchChanged(bool sw);
    public static event OnTutorialSwitchChanged onTutorialSwitchChanged;

    // Checks when ball is out of bounds and gives text to player
    public delegate void BallOutOfBounds();
    public static event BallOutOfBounds ballOutOfBounds;

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
        SphereStartPos = GameObject.FindGameObjectWithTag("StartPos").transform.position;
        speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        powerbar.value = 0;
        powerbar.maxValue = forceLimit;
        isInStartArea = true;
        atZeroPointArea = false;
        isActive = true;
        outOfBounds = false;
        isBouncedOutZeroArea = false;
        audiosource = Instantiate(audiosource, AudioManager.instance.transform);
        audiosource.enabled = true;
        audiosource.loop = true;
        audiosource.volume = 0;
        audiosource.Play();
        StartCoroutine(SwitchBall());
    }
    // Update game every frame to check inputs and outcomes
    void Update()
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
        {   // Reset powerbar
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
            balls[i-1].tag = "Untagged";
            //
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            isInStartArea = true;
            atZeroPointArea = false;
            isStopped.Add(false);
            // Check if there is balls left or check if game is ready to end
            if(i >= balls.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            else
            {
                StartCoroutine(SwitchBall());
            }
        }
        // Check if ball is out of bounds and set up next ball
        if (outOfBounds)
        {
            balls[i-1].transform.parent = player.transform.parent;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            balls[i-1].tag = "Untagged";
            Destroy(balls[i-1]);
            balls.RemoveAt(i-1);
            i--;
            outOfBounds = false;
            isInStartArea = true;
            atZeroPointArea = false;
            if(i >= balls.Count)
            {
                StartCoroutine(CheckEndGame());
            }
            else
            {
                StartCoroutine(SwitchBall());
            }
        }
        //Audio for ball rolling
        speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        if(speed >= 0.1f)
        {   
            float volumeModifier = maxVolume - minVolume;
            float pitchModifier = maxPitch - minPitch;
            audiosource.pitch = minPitch + (speed/maxSpeed)*pitchModifier;
            audiosource.volume = minVolume + (speed/maxSpeed)*volumeModifier;
        }
        if(speed <= 0.1f || GameMenu.IsPaused == true)
        {
            if(audiosource != null)
            {
                audiosource.volume = 0;
            }
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
        player.GetComponent<Rigidbody>().AddForce(0,0,force*factor);
        touchTimeStart = 0;
        touchTimeFinish = 0;
        powerbar.value = 0;
    }
    // Switch ball or start ending the game
    private IEnumerator SwitchBall()
    {   
        atZeroPointArea = false;
        balls[i].transform.parent = player.transform;
        player.transform.GetChild(0).transform.position = SphereStartPos;
        isActive = true;
        i++;
        yield return new WaitForEndOfFrame();
    }

    // Check that every ball is stopped before ending the game so all the points are counted
    private IEnumerator CheckEndGame()
    {
        audiosource.Stop();
        int j = 0;
        foreach(GameObject ball in balls)
        {
            if(ball.GetComponent<Rigidbody>().velocity == Vector3.zero && ball.GetComponent<Rigidbody>().angularVelocity == Vector3.zero)
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
            if(ballOutOfBounds != null)
            {
                ballOutOfBounds();
            }
            outOfBounds = true;
        }
        // // When ball exits zeropointarea, mark it so when ball drops back there second time, it wont trigger extra ball to game
        if(other.transform.tag == "ZeroPointArea")
        {
           isBouncedOutZeroArea = true;
        }
    }
    // Make sound effect when collision happens
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Nail")
        {
            AudioManager.instance.PlaySFXClip(bonkNailSFX, transform, bonkNailVolume);
        }
        if(collision.gameObject.tag == "Balls")
        {
            AudioManager.instance.PlaySFXClip(bonkSFX, transform, bonkVolume);
        }
    }
}