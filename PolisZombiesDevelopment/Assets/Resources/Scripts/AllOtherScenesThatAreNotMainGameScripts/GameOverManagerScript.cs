using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManagerScript : MonoBehaviour
{
    public Sprite gameOverSprite;
    public Sprite goodEndingSprite;
    public AudioClip gameOverClip;
    public AudioClip goodEndingClip;
    // Start is called before the first frame update
    void Start()
    {
        if(GameOverDataScript.getEndGameData() == 0)
        {
            GameObject.Find("ScreenText").GetComponent<TextMeshProUGUI>().text = "Game over";
            GameObject.Find("PlayAgainScreenText").GetComponent<TextMeshProUGUI>().text = "(Press Space to try again)";
            GameObject.Find("GameOverScreen").GetComponent<SpriteRenderer>().sprite = gameOverSprite;
            GetComponent<AudioSource>().clip = gameOverClip;
        }
        else if(GameOverDataScript.getEndGameData() == 1)
        {
            GameObject.Find("ScreenText").GetComponent<TextMeshProUGUI>().text = "Congratulations, you reached the final infinite round! Now to win the game, one of the easter eggs is to save as much money as possible (which is a lot) to buy the front gate and get the good ending. Good luck on your next try!";
            GameObject.Find("PlayAgainScreenText").GetComponent<TextMeshProUGUI>().text = "(Press Space to try again)";
            GameObject.Find("GameOverScreen").GetComponent<SpriteRenderer>().sprite = gameOverSprite;
            GetComponent<AudioSource>().clip = gameOverClip;
        }
        else
        {
            GameObject.Find("ScreenText").GetComponent<TextMeshProUGUI>().text = "Congratulations, you beat the game!";
            GameObject.Find("PlayAgainScreenText").GetComponent<TextMeshProUGUI>().text = "(Press Space to play again)";
            GameObject.Find("GameOverScreen").GetComponent<SpriteRenderer>().sprite = goodEndingSprite;
            GetComponent<AudioSource>().clip = goodEndingClip;
        }

        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
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
