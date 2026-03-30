using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameOverDataScript
{
    // When is game over, the text should change depending on which type of game over it is,
    //and the background sprite should change to a good ending if the player get into the car.

    // If 0, then normal game over, if 1, then game over at round 21, if 2, then the player won game and got the car.
    public static int endGameData;

    public static void setEndGameData(int endgameState)
    {
        endGameData = endgameState;
    }

    public static int getEndGameData()
    {
        return endGameData;
    }
}
