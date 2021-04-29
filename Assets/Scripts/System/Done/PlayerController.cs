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
    public GameObject fire;

    public GameObject playerModel;
    public GameManager gM;

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

    //Spawn Point
    public Vector3 spawnPoint;
    //Other checks
    [SerializeField]
    private bool _isGrounded = true;
    private bool _isJumping;

    private bool facingRight = false;
    public bool isPlayerDead = false;
    public bool isOnFire = false;


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
        spawnPoint = new Vector3(-21.31f, 0.27f, -35);
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
        Debug.Log(_isGrounded);
        //Debug.Log(spawnPoint);
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

        if (isOnFire == true)
        {
            //StartCoroutine(DelayFifteen());
        }

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
        if (other.gameObject.CompareTag("Spikes") || other.gameObject.CompareTag("spikechange"))
        {
            Instantiate(blood, transform.position, Quaternion.identity);
        }

        if (other.gameObject.CompareTag("Fire") && !gameObject.CompareTag("Dead"))
        {
            
            gameObject.tag = "Dead";
            GameObject thing = Instantiate(fire, transform.position + new Vector3(0f, 1.134f, -0.141f), Quaternion.identity);
            GameObject thing2 = Instantiate(fire, transform.position + new Vector3(0f, 0.401f, 0.131f), Quaternion.identity);
            thing.transform.parent = transform;     
            thing2.transform.parent = transform;
            StartCoroutine(DelayFifteen());
            //gameObject.GetComponent<PlayerController>().enabled = false;a
            //gameObject.GetComponent<Rigidbody>().useGravity = true;
            //gameObject.GetComponent<Collider>().material.dynamicFriction = 10f;
            //gameObject.GetComponent<Collider>().material.staticFriction = 10f;
            //gameObject.GetComponent<Animator>().enabled = false;
        }

        if (other.gameObject.CompareTag("firechange") && !gameObject.CompareTag("Dead"))
        {
            gameObject.tag = "Dead";
            GameObject thing = Instantiate(fire, transform.position, Quaternion.identity);
            thing.transform.parent = transform; 
            spawnPoint = new Vector3(-20.33f, 5.8f, -35f);
            StartCoroutine(DelayFifteen());
        }
        if (other.gameObject.CompareTag("Button"))
        {
            spawnPoint = new Vector3(-8.29f, 0.27f, -35f);
        }

        if (other.gameObject.CompareTag("spikechange"))
        {
            anim.SetInteger("crushed", 1);
            spawnPoint = new Vector3(0, 0.38f, -35f);
        }
    }

    public void SetFireState()
    {
        isOnFire = true;
    }

    public IEnumerator DelayFifteen()
    {
        yield return new WaitForSeconds(5f);

        isOnFire = false;
        Debug.Log("Burnt to a crisp, GONE");
        anim.SetInteger("crushed", 1);
        //play death animation
        gameObject.GetComponent<PlayerController>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
        //GameObject playerInstance = Instantiate(playerObj, new Vector3(-17f, -0.28f, -35), Quaternion.identity);
        GameObject playerInstance = Instantiate(playerModel, spawnPoint, Quaternion.identity);

        Destroy(playerInstance.transform.Find("vfx_firev2(Clone)").gameObject);

        //Destroy(playerInstance.transform.GetChild(3).gameObject);
        playerInstance.tag = "Player";
        gameObject.GetComponent<PlayerController>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Animator>().enabled = false;
        //Destroy(this.gameObject);
    } 
}
