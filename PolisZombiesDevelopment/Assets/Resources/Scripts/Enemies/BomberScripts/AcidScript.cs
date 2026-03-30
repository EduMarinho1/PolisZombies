using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidScript : MonoBehaviour
{
    private bool damaged = true;

    void Start()
    {
        StartCoroutine(TimeToStartToTakeDamageAndDestroyObject());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && damaged == false)
        {
            damaged = true;
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(1);
            GameObject.Find("Player").GetComponent<PlayerScript>().Slow();
            StartCoroutine(WaitToDamage());
        }
    }

    private IEnumerator WaitToDamage()
    {
        yield return new WaitForSeconds(2f);
        damaged = false;
    }

    private IEnumerator TimeToStartToTakeDamageAndDestroyObject()
    {
        yield return new WaitForSeconds(0.1f);
        damaged = false;
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
