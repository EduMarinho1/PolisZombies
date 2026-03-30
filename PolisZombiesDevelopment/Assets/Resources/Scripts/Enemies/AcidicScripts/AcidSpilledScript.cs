using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpilledScript : MonoBehaviour
{
    private bool damaged = true;

    void Start()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(TimeToStartToTakeDamageAndDestroyObject());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && damaged == false)
        {
            damaged = true;
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(1);
            StartCoroutine(WaitToDamage());
            GameObject.Find("Player").GetComponent<PlayerScript>().Slow();
        }
    }

    private IEnumerator WaitToDamage()
    {
        yield return new WaitForSeconds(2f);
        damaged = false;
    }

    private IEnumerator TimeToStartToTakeDamageAndDestroyObject()
    {
        yield return new WaitForSeconds(0.01f);
        damaged = false;
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}
