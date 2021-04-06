using UnityEngine;
using System.Collections;
 
 public class Piston : MonoBehaviour {

     Rigidbody rb;
     public float speedExtend; // speed piston extends
     public float speedRetract; // speed piston retracts
     public float timeExtended = 5f; // time in seconds piston should remain extended before retracting
     public Vector3 jumpspeed = new Vector3 (0, 10, 0); // player's vertical speed when piston is triggered
     public bool pistonExtend; // boolean to tell script to extend piston
     public float timeLeft; // variable to hole amount of time left before piston retracts
     public bool extended; // boolean to denote if piston is in an extended state
     private Quaternion orientation; //get worldspace orientation to make piston object work in any orientation
 
     // Use this for initialization
     void Start () {
         pistonExtend = false; 
         extended = false; //piston starts retracted
         timeLeft = timeExtended; //set time remaining to total time

         rb = GetComponent<Rigidbody>();
     }
     
     // Update is called once per frame
     void FixedUpdate () {
         orientation = transform.rotation; //set orientation quat during update so it can move in the world and always work
         if (pistonExtend == true) { //do this when we want to extend
             if(transform.localPosition.y <= 0.5f){ // extend until we reach this position
                 rb.MovePosition(rb.position+orientation*Vector3.down * speedExtend * Time.deltaTime);
                 extended = true;
             }
             timeLeft = timeLeft-Time.deltaTime; // start counting down
             if(timeLeft<0f){
                 pistonExtend=false; //when time runs out, set the retract flag
             }
         }
 
         if(pistonExtend == false & transform.localPosition.y >= 0.1f) //retract back to start position
         {
             rb.MovePosition(rb.position+orientation*Vector3.down * speedRetract * Time.deltaTime);
             timeLeft = timeExtended; //reset extended time
             extended = false;
         }
     }
 
     void OnTriggerEnter(Collider other){
         if (other.gameObject.tag == "Player" && extended == false) {
             pistonExtend=true;
             other.GetComponent<Rigidbody>().velocity = orientation * jumpspeed;
             //other.rigidbody.velocity = orientation * jumpspeed; //set player velocity according to orientation
         }
     }    
 }