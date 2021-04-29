using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ToTheEnd : MonoBehaviour
{
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
        if(other.gameObject.CompareTag("Player"))
            Debug.Log("BURN BABY BURN DISCO INFERNO");
            //SceneManager.LoadScene(3);
            //transform.localPosition = Vector3.up * 0.75f;
            //other.GetComponent.spawnPoint = new Vector3(-8.29f, 0.27f, -35f);
            //door.transform.Translate(new Vector3(-10.5945f, -4f, -29.41f));
        }

}
