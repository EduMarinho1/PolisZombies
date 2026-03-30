using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{

    void Start() {
        StartCoroutine(FlipSprite()); 
    }
    private bool playerDamaged = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player") && playerDamaged == false)
        {
            playerDamaged = true;
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(1);
        }
    }

    IEnumerator FlipSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

}
