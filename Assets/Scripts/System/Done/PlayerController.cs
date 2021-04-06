using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script references
    private Rigidbody rb;
    public Transform groundCheck;
    public GameObject playerObj;
    public GameObject blood;

    CharacterController controller;
    Animator anim;

    //For ground collisions
    public float checkRadius;
    public LayerMask whatIsGround;              //Make sure this is set in your layers!
    public LayerMask whatIsClimb;

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
    public bool isPlayerDead = false;


    public bool _isTouchingFront;
    public Transform frontCheck;
    public bool wallSliding;
    public float wallSlidingSpeed;
    public bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        //note I might start start caching some variables, less memory intensive I think?
        //Todo Time Reverse and Stop/Pausing, slow down
        //Todo Start Research on pausing
        //Todo rework our core mechanic of time manipulation, still recording actions, but
        //what if we had unlimited lives and we just had to fail in order to succed, so a previous
        //after you died completing a task you your
        //might be slightly easier than time recording
        //todo create a new scene with reworked game idea.
        //todo 1. create a prefab of player
        //todo 2. create spikes that will kill instance of player and respawn new instance at start
        //todo 3. create button to trigger a drawbridge
        //todo 4. create enemy that kills player in hit
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
        //Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(moveInput * xVelocity, rb.velocity.y, 0);
        //Debug.Log(moveInput);

        if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // flipped
            //Flip();
            anim.SetInteger("condition", 1);
        }
        else if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // normal
                                                          //Flip();
            anim.SetInteger("condition", 1);
        }

        if (moveInput == 0)
        {
            anim.SetInteger("condition", 0);
        }

        if (_isGrounded)
        {
            _hangCounter = hangTime;
            anim.SetInteger("jump", 0);
        }
        else
        {
            _hangCounter -= Time.deltaTime;
        }
        //Jump movement
        Jump();

        _isTouchingFront = Physics.CheckSphere(frontCheck.position, checkRadius, whatIsClimb);

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

        if(Input.GetKeyDown(KeyCode.Space) && wallSliding == true)
        {
            wallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), wallJumpTime);
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


        if (wallJumping == true)
        {
            rb.velocity = new Vector3(xWallForce * -moveInput, yWallForce, 0);
        }
    }

    private void Jump()
    {
        //Jump movement
        if (Input.GetKeyDown(KeyCode.Space) && _hangCounter > 0f)
        {
            Debug.Log("Jump");
            rb.velocity = new Vector3(rb.velocity.x, yVelocity, 0);
            anim.SetInteger("jump", 1);
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


    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    void PlayerRespawn()
    {
        if(isPlayerDead == true)
        {
            Instantiate(playerObj, new Vector3(16f, 2.5f, 0), Quaternion.identity);
            isPlayerDead = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            Instantiate(blood, transform.position, Quaternion.identity);
        }
    }
}
