using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject fire;
    public PlayerController player;
    // Start is called before the first frame update
    public GameObject pModel;
    void Start()
    {
        player.GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.Log("Player is not loaded");
        }

        pModel.GetComponent<GameObject>();    
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<PlayerController>();
    }

    /*void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.tag = "Dead";
            player.SetFireState();
            Debug.Log("On Fire!");
            GameObject thing = Instantiate(fire, other.transform.position, Quaternion.identity);
            thing.transform.parent = other.transform;             
        }
        //else do nothing;

        //I need to make a corrountine to kill player after lit on fire probably 3 seconds. 
        //Maybe I can make a function instead in here that will destroy anyobject on fire
    }
    */


    
}
