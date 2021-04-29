using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCredits : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("On");
        SceneManager.LoadScene(3);
    }
}
