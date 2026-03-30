using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpitScript : MonoBehaviour
{
    public float speed = 10f;
    public GameObject acidSpill;
    private bool destroyed;

    void Start()
    {
        GetComponent<AudioSource>().Play();
        destroyed = false;
        StartCoroutine(TimeToDestroy());
    }

    public void Launch(Vector3 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Wall" || other.gameObject.tag == "ScrapWall")
        {
            destroyed = true;
            Instantiate(acidSpill, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(Random.Range(0.7f, 1.3f));
        if(destroyed == false)
        {
            Instantiate(acidSpill, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
