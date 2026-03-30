using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TeleportationStairScript : MonoBehaviour
{

    public Transform stairsDestination; // Assign the destination in the Inspector
    public ChangeZombieDestinationScript czds;
    public int floorToTeleport; // 0 is first floor, 2 is second floor. This is because grid mask 0 is normal first floor, 1 is digger first floor and 2 is second floor.
    private int zombieLayer = 6;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(other.CompareTag("Player"))
        //{
        //    czds.ChangeAllZombiesDestinationsWithinGrid(true);
        //}
        Debug.Log("other: " + other.gameObject.name);
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            Debug.Log("teleport");
            other.transform.position = stairsDestination.position;
            // Optionally, you can add an effect or sound here
        }

        if (other.CompareTag("Zombie"))
        {
            if(other.gameObject.name.Contains("DiggerZombie") && floorToTeleport == 2)
            {
                other.gameObject.GetComponent<AudioSource>().Stop();
                Debug.Log("stopcoroutine");
                other.gameObject.GetComponent<DiggerZombieScript>().StopSwitchGraphs();
                other.gameObject.layer = zombieLayer;
                other.gameObject.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<DiggerZombieScript>().diggerZombieSprite;
                other.gameObject.GetComponent<AIPath>().maxSpeed = 2;
            }
            else if(other.gameObject.name.Contains("DiggerZombie") && floorToTeleport == 0)
            {
                other.gameObject.GetComponent<AudioSource>().Stop();
                Debug.Log("startcoroutine");
                other.gameObject.GetComponent<DiggerZombieScript>().StartSwitchGraphs();
            }

            other.gameObject.GetComponent<AIDestinationSetter>().target = GameObject.Find("Player").transform;
            other.gameObject.GetComponent<Seeker>().graphMask = 1 << floorToTeleport;
            Debug.Log("Using only the seond floor graph");
        }
    }
}
