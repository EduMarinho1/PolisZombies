using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryPlacementManagerScript : MonoBehaviour
{
    public GameObject[] objectsToMove;  // Array to hold references to the objects you want to move
    public Transform[] targetPositions; // Array to hold the references to the placeholder positions
    private List<int> temporaryRandomPlacements;
    private List<int> randomPlacements;

    void Start()
    {
        temporaryRandomPlacements = new List<int>();
        for (int i = 0; i < targetPositions.Length; i++)
        {
            temporaryRandomPlacements.Add(i);
        }

        randomPlacements = new List<int>();

        // Ensure the arrays are properly set up
        if (objectsToMove.Length != targetPositions.Length)
        {
            Debug.LogError("Objects to move and target positions arrays must be of the same length.");
            return;
        }

        for (int i = 0; i < objectsToMove.Length; i++)
        {
            int randomNumber = Random.Range(0, temporaryRandomPlacements.Count);
            randomPlacements.Add(temporaryRandomPlacements[randomNumber]);
            temporaryRandomPlacements.RemoveAt(randomNumber);
        }

        Debug.Log("Random Placements: " + string.Join(", ", randomPlacements));
        Debug.Log("Temporary Random Placements: " + string.Join(", ", temporaryRandomPlacements));

        // Assign each object to the corresponding position and rotation
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            objectsToMove[i].transform.position = targetPositions[randomPlacements[i]].position;
            objectsToMove[i].transform.rotation = targetPositions[randomPlacements[i]].rotation;
        }
    }
}
