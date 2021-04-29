using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public PlayerController player;
    // Start is called before the first frame update
    public Transform door;
    void Start()
    {
        //door = transform.Find("Door");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Dead"))
        {
            Debug.Log("On");
            transform.localPosition = Vector3.up * 0.75f;
            //other.GetComponent.spawnPoint = new Vector3(-8.29f, 0.27f, -35f);
            door.transform.Translate(new Vector3(-11.76f, 19.93f, -73.65f));
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Dead"))
        {
            Debug.Log("Off");
            transform.localPosition = Vector3.up * 1.25f;
        }

    }
}
