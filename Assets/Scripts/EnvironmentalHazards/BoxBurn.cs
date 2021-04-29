using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBurn : MonoBehaviour
{
    public GameObject fire;
    // Start is called before the first frame update
    void Start()
    {
        //fire = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(Collider player)
    {
        Debug.Log("Hellow?");
        if (player.gameObject.CompareTag("Dead"))
        {
            Debug.Log("BURN BABY BURN DISCO INFERNO");
            Instantiate(fire, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 5);
        }
    }
}
