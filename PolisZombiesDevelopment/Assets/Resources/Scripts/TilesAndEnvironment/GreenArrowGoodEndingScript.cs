using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenArrowGoodEndingScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameOverDataScript.setEndGameData(2);
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
