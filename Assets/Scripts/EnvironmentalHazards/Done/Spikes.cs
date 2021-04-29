using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameObject player;
    PlayerController pc;
    Animator animate;
    Animator deadAni;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //animate = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        //deadAni = GameObject.FindGameObjectWithTag("Dead").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 originalpos = transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, originalpos + new Vector3(0, -7f, 0), .025f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //activate bridge turning on kinematic of bridge but killing previous player
            Debug.Log("DEAD");
            //other.tag = "Player";
            //pc.enabled = false;
            other.tag = "Dead";
            if (other.CompareTag("Dead") == true)
            {
                //animate.SetInteger("crushed", 1);
                other.GetComponent<PlayerController>().enabled = false;
                other.GetComponent<Rigidbody>().useGravity = true;
               //other.GetComponent<Collider>().material.dynamicFriction = 10f;
                //other.GetComponent<Collider>().material.staticFriction = 10f;
                //other.GetComponent<Animator>().enabled = false;
            }

            GameObject playerInstance = Instantiate(player, new Vector3(-20.49f, 6.22f, -35f), Quaternion.identity);
            playerInstance.tag = "Player";
            playerInstance.GetComponent<PlayerController>().enabled = true;
            playerInstance.GetComponent<Collider>().material.dynamicFriction = 0f;
            playerInstance.GetComponent<Collider>().material.staticFriction = 0f;
            playerInstance.GetComponent<Rigidbody>().useGravity = false;
            //playerInstance.name = "Player";
            Destroy(other.gameObject, 15f);

        }

        //when player collides with spike
        //that tag n
    }
}