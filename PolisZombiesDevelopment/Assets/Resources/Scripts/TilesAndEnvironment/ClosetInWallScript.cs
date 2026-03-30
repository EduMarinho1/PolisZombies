using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetInWallScript : MonoBehaviour
{
    public GameObject blackSquares;
    public int WallCost;
    public GameObject purchaseSound;
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("collided on scrapWall");
        if(other.gameObject.tag == "Player" && Input.GetKey("space") && other.gameObject.GetComponent<PlayerScript>().money >= WallCost)
        {
            Debug.Log("destroyed");
            other.gameObject.GetComponent<PlayerScript>().money = other.gameObject.GetComponent<PlayerScript>().money - WallCost;
            Destroy(Instantiate(purchaseSound, transform.position, transform.rotation), 5f);
            Destroy(blackSquares);
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
