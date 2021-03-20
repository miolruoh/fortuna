using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    /*Vector3 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    [Range(0.05f, 1f)]
    public float throwForce = 0.3f;*/

    [Range(1f, 10f)]
    public float v = 5f;    // how fast ball is launced( just for testing)

    Vector3 SpherestartPos;

    public Rigidbody ball1;
    public Rigidbody ball2;
    public Rigidbody ball3;
    public Rigidbody ball4;
    public Rigidbody ball5;
    List<Rigidbody> rbList = new List<Rigidbody>();
    bool isInStartArea;         // if true, ball is still in start area and so if it's launch too slow and it comes back and stops, new ball won't come active
    bool atZeroPointArea;       // if true, next ball can move to the launch area even though if ball is still moving
    bool isActive;              // if true, ball is ready to launch, otherwise launch is disabled
    private int i = 0;         // to keep track of the active ball in the list

    private Rigidbody rb;

    //Assigned in start
    private void Start()
    {
        rbList.Add(ball1);
        rbList.Add(ball2);
        rbList.Add(ball3);
        rbList.Add(ball4);
        rbList.Add(ball5);
        rb = rbList[i];
        isActive = true;
        //isInStartArea = true;
        //atZeroPointArea = false;
        SpherestartPos = rb.transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && isActive)
        {
            Launch();
        }
        //Not Working correctly
        if (atZeroPointArea || (!isInStartArea && rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero))
        {
            SwitchBall();
        }
    }

    private void Launch()
    {
        rb.velocity = new Vector3(0, 0, v);
        isActive = false;
        i++;
    }

    private void SwitchBall()
    {
        atZeroPointArea = false;
        if (i < rbList.Count)
        {
            rb = rbList[i];
            rb.position = SpherestartPos;
            isActive = true;
        }
    }

    //Not Working correctly
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "StartingArea")
        {
            isInStartArea = true;
        }

        if (other.gameObject.tag == "ZeroPointArea")
        {
            atZeroPointArea = true;
        }
    }

    //Not Working correctly
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "StartingArea")   
        {
            isInStartArea = false;
        }
    }

    // Reset scene (probably going in to own script)
    private void OnButtonPress()
    {
        Debug.Log("Set points to zero");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
