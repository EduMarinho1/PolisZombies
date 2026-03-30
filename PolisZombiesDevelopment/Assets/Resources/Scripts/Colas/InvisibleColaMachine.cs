using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleColaMachine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisableSprites();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            EnableSprites();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            DisableSprites();
        }
    }

    // Function to disable the child and grandchild sprites
    public void DisableSprites()
    {
        // Get the child GameObject
        Transform child = transform.GetChild(0);

        // Disable the child's sprite
        child.GetComponent<SpriteRenderer>().enabled = false;

        // Get the grandchild GameObject
        Transform grandchild = child.GetChild(0);

        // Disable the grandchild's sprite
        grandchild.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Function to enable the child and grandchild sprites
    public void EnableSprites()
    {
        // Get the child GameObject
        Transform child = transform.GetChild(0);

        // Enable the child's sprite
        child.GetComponent<SpriteRenderer>().enabled = true;

        // Get the grandchild GameObject
        Transform grandchild = child.GetChild(0);

        // Enable the grandchild's sprite
        grandchild.GetComponent<SpriteRenderer>().enabled = true;
    }

}
