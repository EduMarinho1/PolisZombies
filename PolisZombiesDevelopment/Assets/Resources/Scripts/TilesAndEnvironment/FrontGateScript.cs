using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontGateScript : MonoBehaviour
{
    public GameObject openGate;
    public GameObject purchaseSound;
    private int WallCost = 3000;
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("collided on gate");
        if(other.gameObject.tag == "Player" && Input.GetKey("space") && other.gameObject.GetComponent<PlayerScript>().money >= WallCost)
        {
            Debug.Log("destroyed");
            other.gameObject.GetComponent<PlayerScript>().money = other.gameObject.GetComponent<PlayerScript>().money - WallCost;
            Instantiate(openGate, new Vector3(transform.position.x - 0.4f, transform.position.y - 1.5f, transform.position.z), transform.rotation);
            Destroy(Instantiate(purchaseSound, transform.position, transform.rotation), 5f);
            Destroy(transform.parent.gameObject);
        }
    }
}
