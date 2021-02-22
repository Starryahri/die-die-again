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
    public float gravityMultiplier = 1.5f;
    public float jumpTimeCounter;
    public float jumpTime;

    private bool _isJumping;
    public bool _canDoubleJump = false;
    public bool _isGrounded = false;

    
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
        //horizontal movement
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * xVelocity, rb.velocity.y, 0);

        customGravity = (-2 * peakHeight * yVelocity * yVelocity)/(xDistance * xDistance);
        
        if(_isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            //actions here... like jumping
            rb.velocity = new Vector3(rb.velocity.x, yVelocity, 0);
            _isGrounded = false;
            _isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        //once player reaches peak of jump
        if (rb.velocity.y <= 0)
        {
            //heavier gravity
            rb.velocity += Vector3.up * customGravity * gravityMultiplier * Time.deltaTime;
        }
        if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * customGravity * Time.deltaTime;
        }

        //hold for higher jump
        if(Input.GetKey(KeyCode.Space) && _isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, yVelocity, 0);
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                _isJumping = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }
        
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
}
