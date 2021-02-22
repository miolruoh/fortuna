using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;
    Vector3 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    float posx;
    float posy;
    float posz;

    [Range(0.05f, 1f)]
    public float throwForce = 0.3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rb.velocity = new Vector3(0, 0, 5);
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
    public void OnButtonPress()
    {
        rb.AddForce(0, 0, 0);
        posx = 0;
        posy = 1;
        posz = 0;
        rb.position = new Vector3(posx, posy, posz);
    }
}
