using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public GameObject fire;
    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void OnTriggerEnter(Collider other)
    {
        if(pc.gameObject.GetComponent<PlayerController>().isOnFire || other.gameObject.CompareTag("Dead"))
        {
            Debug.Log("BURN BABY BURN DISCO INFERNO");
            GameObject fireone = Instantiate(fire, new Vector3(-6.47f, 10.32f, -35.41f), Quaternion.identity);
            fireone.transform.parent = transform;
            StartCoroutine(Burning());
            //transform.localPosition = Vector3.up * 0.75f;
            //other.GetComponent.spawnPoint = new Vector3(-8.29f, 0.27f, -35f);
            //door.transform.Translate(new Vector3(-10.5945f, -4f, -29.41f));
        }

    }

    IEnumerator Burning()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject firetwo = Instantiate(fire, new Vector3(-5.11f, 10.32f, -36.17f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        firetwo.transform.parent = transform;
        
        GameObject firethree = Instantiate(fire, new Vector3(-5.43f, 11.22f, -34.55f), Quaternion.identity);
        firethree.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        GameObject firefour = Instantiate(fire, new Vector3(-4.514f, 10.23f, -35.332f), Quaternion.identity);
        firefour.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        GameObject firefive = Instantiate(fire, new Vector3(-6.17f, 10.23f, -35.332f), Quaternion.identity);
        firefive.transform.parent = transform;
        Destroy(this.gameObject, 1);
    }
}
