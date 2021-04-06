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
    private BoxCollider m_coll;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_coll = GetComponent<BoxCollider>();
        timeLeft = extendedTime;
    }

    void FixedUpdate()
    {
        if(pistonExtend == true) //start extention
        {
            isExtended = true;
            //Debug.Log("Extending");
            rb.MovePosition(transform.position + Vector3.down * Time.deltaTime * extendSpeed);
            timeLeft = timeLeft-Time.deltaTime;
            if(timeLeft <= 0)
            {
                pistonExtend = false; //when time runs out, set the retract flag
                timeLeft = extendedTime;
            }
        }

        if (transform.localPosition.y <= 2.7f && pistonExtend == false)  //retract back to start
        {
            //Debug.Log("Retracting");
            m_coll.enabled = false;
            rb.MovePosition(transform.position + Vector3.up * Time.deltaTime * retractSpeed);
            timeLeft = extendedTime;
            if (transform.localPosition.y >= 2.6f)
            {
                isExtended = false;
                m_coll.enabled = true;
            }
            isExtended = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isExtended == false)
        {
            pistonExtend = true;
            //Debug.Log("Squish");    
        }

    }    
}
