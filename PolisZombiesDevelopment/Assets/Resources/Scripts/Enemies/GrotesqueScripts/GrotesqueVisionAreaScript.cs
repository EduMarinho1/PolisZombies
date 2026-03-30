using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrotesqueVisionAreaScript : MonoBehaviour
{
    private bool isCollidingWithPlayer = false;
    private bool isCollidingWithWall = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Window")) && !isCollidingWithWall)
        {
            isCollidingWithWall = true;
            StartCoroutine(ResetWallCollision());
        }

        if (other.gameObject.CompareTag("Player") && !isCollidingWithPlayer && !isCollidingWithWall)
        {
            isCollidingWithPlayer = true;
            StartCoroutine(ResetPlayerCollision());
        }
    }

    IEnumerator ResetWallCollision()
    {
        yield return new WaitForSeconds(0.5f);
        isCollidingWithWall = false;
    }

    IEnumerator ResetPlayerCollision()
    {
        yield return new WaitForSeconds(0.5f);
        isCollidingWithPlayer = false;
    }

    public bool CanAttack()
    {
        return isCollidingWithPlayer && !isCollidingWithWall;
    }
}