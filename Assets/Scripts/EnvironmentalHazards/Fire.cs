using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject fire;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //set onFire variable to true
        Debug.Log("On Fire!");
        GameObject thing = Instantiate(fire, other.transform.position, Quaternion.identity);
        thing.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;

        //else do nothing;

        //I need to make a corrountine to kill player after lit on fire probably 3 seconds. 
        //Maybe I can make a function instead in here that will destroy anyobject on fire
    }
}
