using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public GameObject iceCube;
    public GameObject player;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        //GameObject thing = Instantiate(iceCube, other.transform.position, Quaternion.identity);
        //thing.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        

        if(other.gameObject.CompareTag("Player"))
        {
            //activate bridge turning on kinematic of bridge but killing previous player
            Debug.Log("FREEZE!");
            //other.tag = "Player";
            //pc.enabled = false;
            other.tag = "Dead";
            if (other.CompareTag("Dead") == true)
            {
                other.GetComponent<PlayerController>().enabled = false;
                other.GetComponent<Animator>().enabled = false;
                GameObject thing = Instantiate(iceCube, other.transform.position + new Vector3(0f, 0.65f, 0f), Quaternion.identity);                           
            }

            GameObject playerInstance = Instantiate(player, new Vector3(-17f, -0.28f, -35), Quaternion.identity);
            playerInstance.tag = "Player";
            playerInstance.GetComponent<PlayerController>().enabled = true;
            //playerInstance.name = "Player";
            Destroy(other.gameObject);

            Destroy(this.gameObject);


        }
    }
}
