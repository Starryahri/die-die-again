using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(GravityOff());

        }
    }

    IEnumerator GravityOff()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
