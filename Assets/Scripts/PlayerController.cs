using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script references
    private Rigidbody rb;
    public Transform groundCheck;

    //For ground collisions
    public float checkRadius;
    public LayerMask whatIsGround;              //Make sure this is set in your layers!

    //Adjust these values for specific gravity
    public float yVelocity = 1f;                //Vertical move speed, jump force
    public float xVelocity = 1f;                //Horizontal move speed
    public float peakHeight = 1f;               //Peak height of jump
    public float xDistance = 1f;                //Total distance for complete arc
    public float customGravity;                 //Gravity based on above values
    public float gravityMultiplier = 1.5f;      //Used to fine tune gravity after peak

    //Timer for hold for higher jump              
    public float jumpTime;
    public float jumpTimeCounter;

    //Hang time
    public float hangTime = 0.2f;
    private float _hangCounter;

    //Other checks
    [SerializeField]
    private bool _isGrounded;
    private bool _isJumping;

    private bool facingRight = false;

    public bool _isTouchingFront;
    public Transform frontCheck;
    public bool wallSliding;
    public float wallSlidingSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //note I might start start caching some variables, less memory intensive I think?
        //Todo Wall Jump
        //Todo Time Reverse and Stop/Pausing, slow down
        //Todo Start Research on pausing
    }

    void Update()
    {
        //Check if player is grounded, if they are able to jump, refer to MovementControls()
        _isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        //rolled my own gravity feel, super customizable though!
        //This gravity is calculated based off position, not time;
        customGravity = (-2 * peakHeight * yVelocity * yVelocity) / (xDistance * xDistance);

        //respawn if off the map (DEBUG ONLY)
        if (transform.position.y < -8f)
        {
            transform.position = new Vector3(0, 1, 0);
        }

        MovementControls();


    }

    private void MovementControls()
    {
        //Horiztonal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(moveInput * xVelocity, rb.velocity.y, 0);
        Debug.Log(moveInput);

        if (moveInput < 0 && facingRight == false)
        {
            Flip();
        }
        else if (moveInput > 0 && facingRight == true)
        {
            Flip();
        }

        if (_isGrounded)
        {
            _hangCounter = hangTime;
        }
        else
        {
            _hangCounter -= Time.deltaTime;
        }
        //Jump movement
        Jump();

        _isTouchingFront = Physics.CheckSphere(frontCheck.position, checkRadius, whatIsGround);

        if (_isTouchingFront == true && _isGrounded == false && moveInput != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }


        //Once player reaches peak of jump
        if (rb.velocity.y <= 0)
        {
            //heavier gravity for juice
            float fallGravity = customGravity * gravityMultiplier;
            rb.velocity += Vector3.up * fallGravity * Time.deltaTime;
        }
        //If player releases anytime before peak
        if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * customGravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        //Jump movement
        if (Input.GetKeyDown(KeyCode.Space) && _hangCounter > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, yVelocity, 0);
            _isGrounded = false;
            _isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKey(KeyCode.Space) && _isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, yVelocity, 0);
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
