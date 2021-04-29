using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // to demo level
    }

    public void Credits()
    {
        SceneManager.LoadScene(2); // to credits
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameComplete()
    {
        SceneManager.LoadScene(3); //game complete screen
    }

    public void Back()
    {
        SceneManager.LoadScene(0); //back to title
    }
}
