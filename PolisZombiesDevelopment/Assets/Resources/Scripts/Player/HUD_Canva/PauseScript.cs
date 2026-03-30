using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScript : MonoBehaviour
{
    private bool isPaused;
    TextMeshProUGUI pausedText;

    void Start()
    {
        pausedText = GetComponent<TextMeshProUGUI>();
        pausedText.text = "";
        
        isPaused = false;
    }

    void Update()
    {
        // Check if the player presses the Pause button (e.g., "Escape" key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;  // Pause the game
        isPaused = true;
        pausedText.text = "PAUSED \n Player starts without weapon \n W, A, S, D -> movement \n Space -> Purchase \n Left mouse key -> Shoot \n Right mouse key -> Sprint \n R -> Reload \n E -> Knife enemies (only triggers when close enough) \n Y -> Change between primary and secondary weapon \n Esc -> Quit game";
        // Optionally, show the pause menu UI here
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;  // Resume the game
        isPaused = false;
        pausedText.text = "";
        // Optionally, hide the pause menu UI here
    }
}