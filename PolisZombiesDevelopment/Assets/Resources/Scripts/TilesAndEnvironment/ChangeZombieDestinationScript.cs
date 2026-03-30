using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChangeZombieDestinationScript : MonoBehaviour
{
    private int enemyLayer = 1 << 6;
    private int playerLayer = 1 << 3;
    private int radius = 100;

    void Start()
    {
        StartCoroutine(CheckIfPlayerInGrid());
    }


    public void ChangeAllZombiesDestinationsWithinGrid(bool zombieDestinationStairs)
    {
        Debug.Log("ChangingDestinaion");
        GameObject player = GameObject.Find("Player");
        Collider2D[] hitCollidersEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D hitCollider in hitCollidersEnemies)
        {
            AIDestinationSetter enemy = hitCollider.GetComponent<AIDestinationSetter>();
            Debug.Log("got one enemy");

            if (enemy != null)
            {
                Debug.Log("enemy not null");
                if (zombieDestinationStairs)
                {
                    enemy.target = transform.parent;
                    Debug.Log("to stairs");
                }
                else
                {
                    enemy.target = player.transform;
                }
            }
        }
    }

    IEnumerator CheckIfPlayerInGrid()
    {
        while (true)
        {
            Collider2D[] hitColliderPlayer = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
            if(hitColliderPlayer.Length > 0)
            {
                ChangeAllZombiesDestinationsWithinGrid(false);
            }
            else
            {
                ChangeAllZombiesDestinationsWithinGrid(true);
            }
            yield return new WaitForSeconds(2);
        }
    }
}
