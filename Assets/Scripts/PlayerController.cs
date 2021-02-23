using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public float yVelocity = 1f;
    public float xVelocity = 1f;
    public float peakHeight = 1f;
    public float xDistance = 1f;
    public float customGravity;
    public float gravityMultiplier = 1.5f;
    public float jumpTimeCounter;
    public float jumpTime;

    public bool _isGrounded;
    private bool _isJumping;
    public bool _canDoubleJump = false;
    public bool _isTouchingFront = false;
    
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


        //Todo Clean up the code and break up into functions
        //Todo Change some variables from public to private(serialize some)
        //Todo Wall Slide
        //Todo Wall Jump
        //Todo Time Reverse and Stop/Pausing, slow down
        //Todo Start Research on pausing
        //todo add coyote/hang time
    }

    void Update()
    {
        ///AAAAHHH 3D equivlant is CheckSphere not OverlapCircle!!!!! WTF....
        _isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);
        //Nice thing though the refactoring actually fit really nicely into my program
        //Seems this is the easiest way to go about platforming...
       
        //respawn if off the map
        if(transform.position.y < -8f)
        {
            transform.position = new Vector3(0, 1, 0);
        }

        //horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(moveInput * xVelocity, rb.velocity.y, 0);

        //rolled my own gravity feel, super customizable though!
        //This gravity is calculated based off position, not time;
        customGravity = (-2 * peakHeight * yVelocity * yVelocity)/(xDistance * xDistance);
        
        //Here is the jump action
        if(_isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
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
}
