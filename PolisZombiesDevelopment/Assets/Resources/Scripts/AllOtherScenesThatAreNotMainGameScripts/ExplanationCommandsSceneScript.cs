using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplanationCommandsSceneScript : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the application
            Application.Quit();
        }
    }
}
