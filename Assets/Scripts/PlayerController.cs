using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float yVelocity = 1f;
    public float xVelocity = 1f;
    public float peakHeight = 1f;
    public float xDistance = 1f;
    public float customGravity;

    public bool _isJump = false;
    public bool _isGrounded = false;

    public float gravityMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //g or acc = (-2 * h * vx^2)/(xh)^2
        //v0 = (2 * h * vx)/xh
        
        //pos += (vel * dt) + (.5acc * dt * dt)
        //vel += acc * dt

        //v0 = initial velocity in y direction
        //h = peak height
        //vx = velocity in x direction
        //xh = x distance to get to peak h
        
    }

    void Update()
    {
        if(transform.position.y < -8f)
        {
            transform.position = new Vector3(0, 1, 0);
        }

        customGravity = (-2 * peakHeight * yVelocity * yVelocity)/(xDistance * xDistance);
        if(_isGrounded == true)
        {
            //actions here... like jumping
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector3.up * yVelocity;
                _isGrounded = false;
            }

        }

        if (rb.velocity.y <= 0)
        {
            rb.velocity += Vector3.up * customGravity * gravityMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * customGravity * Time.deltaTime;
        }



        //horizontal movement
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * xVelocity, rb.velocity.y, 0);
    }

    void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.name == "Platform" || other.gameObject.name == "Platform (1)")
        {
            _isGrounded = true;
        }
    }

    void OnCollsionExit(Collision other)
    {
        if(other.gameObject.name == "Platform" || other.gameObject.name == "Platform (1)")
        {
            _isGrounded = false;
        }
    }
    // Update is called once per frame
    /*void Update()
    {
        if (_isJump == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJump = true;
                rb.velocity = Vector3.up * yVelocity;

            }
                   
        }


//if not jumping
//then press space bar and set jumping to true;



        //move horizontally
        
    }
*/
}
