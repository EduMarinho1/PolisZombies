using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrapWallScript : MonoBehaviour
{
    public int WallCost;
    private float timeBetweenBuy;
    private float timeSinceLastBuy;
    public GameObject purchaseSound;

    void Start()
    {
        timeBetweenBuy = 1.5f;
        timeSinceLastBuy = 0;
    }

    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("collided on scrapWall");
        if(other.gameObject.tag == "Player" && Input.GetKey("space") && other.gameObject.GetComponent<PlayerScript>().money >= WallCost)
        {
            Debug.Log("destroyed");
            other.gameObject.GetComponent<PlayerScript>().money = other.gameObject.GetComponent<PlayerScript>().money - WallCost;
            timeSinceLastBuy = 0;
            Destroy(Instantiate(purchaseSound, transform.position, transform.rotation), 5f);
            Destroy(transform.parent.gameObject);            
        }

        else if(timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space") && other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            timeSinceLastBuy = 0;
        }
    }

void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.tag == "Player")
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
        GameObject purchaseLabel = purchaseLabelTransform.gameObject;
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase wall (" + WallCost + ")";
    }
}

private void OnTriggerExit2D(Collider2D other) 
{
    if(other.gameObject.tag == "Player")
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
        GameObject purchaseLabel = purchaseLabelTransform.gameObject;
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "";
    }
}
}
