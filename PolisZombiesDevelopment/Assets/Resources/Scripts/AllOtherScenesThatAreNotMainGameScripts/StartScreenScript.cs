using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }
    
    void Update()
    {
        if(Input.GetKey("space"))
        {
            SceneManager.LoadScene("PolisScene");
        }

        else if(Input.GetKey("t"))
        {
            SceneManager.LoadScene("ExplainCommandsScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the application
            Application.Quit();
        }
    }
}
