using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStairsNotification : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public Sprite oneZombieSprite;
    public Sprite twoZombieSprite;
    public Sprite multipleZombiesSprite;
    public int zombiesColliding = 0; // Counter for zombies colliding

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start with the sprite disabled
        spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has a ZombieScript
        if (collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent))
        {
            zombiesColliding++;
            spriteRenderer.enabled = true; // Enable the sprite

            UpdateSprite();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the exiting object has a ZombieScript
        if (collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent))
        {
            zombiesColliding--;

            // Disable the sprite if no more zombies are colliding
            if (zombiesColliding <= 0)
            {
                spriteRenderer.enabled = false;
            } else {
                UpdateSprite();
            }
        }
    }

    private void UpdateSprite()
    {
        if (zombiesColliding == 1)
        {
            spriteRenderer.sprite = oneZombieSprite;
        }
        else if (zombiesColliding == 2)
        {
            spriteRenderer.sprite = twoZombieSprite;
        }
        else if (zombiesColliding > 2)
        {
            spriteRenderer.sprite = multipleZombiesSprite;
        }
    }
}