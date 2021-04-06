using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    public float extendSpeed = 0.1f;
    public float retractSpeed; 
    public float extendedTime; //used to determine how long it is extended for before retracting
    public float timeLeft; //will be used to determine countdown

    public bool pistonExtend = false;  //piston extend set to true when you want to extend
    public bool isExtended = false; //extended

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeLeft = extendedTime;
    }

    void FixedUpdate()
    {
        Debug.Log(transform.localPosition.y);
        if(pistonExtend == true) //start extention
        {
            isExtended = true;
            Debug.Log("Extending");

            rb.MovePosition(transform.position + Vector3.down * Time.deltaTime * extendSpeed);
            
            timeLeft = timeLeft-Time.deltaTime;
            
            if(timeLeft <= 0)
            {
                //pistonExtend = false; //when time runs out, set the retract flag
                pistonExtend = false;
                isExtended = false;
                timeLeft = extendedTime;
                //rb.MovePosition(transform.position + Vector3.up * Time.deltaTime * retractSpeed);
            }
        }

        if (transform.localPosition.y <= 1.7f && pistonExtend == false)  //retract back to start
        {
            if(transform.localPosition.y >= 2.7f)
            {
                rb.MovePosition(transform.position + Vector3.zero * Time.deltaTime);
            }
            Debug.Log("Retracting");
            rb.MovePosition(transform.position + Vector3.up * Time.deltaTime * retractSpeed);
            timeLeft = extendedTime;
            isExtended = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isExtended == false)
        {
            pistonExtend = true;
            Debug.Log("Squish");    
        }
    }    
}
