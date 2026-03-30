using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaMachineScript : MonoBehaviour
{
    public int price;
    public AudioClip purchaseAcceptedSound;
    public AudioClip purchaseRejectedSound;
    private float timeBetweenBuy;
    private float timeSinceLastBuy;
    
    void Start()
    {
        timeBetweenBuy = 2;
        timeSinceLastBuy = 0;
    }

    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !other.GetComponent<PlayerScript>().HasCola("Stamina") &&
            Input.GetKey("space") && other.gameObject.GetComponent<PlayerScript>().money >= price && other.gameObject.GetComponent<PlayerScript>().playerColas.Count < 3
             && timeSinceLastBuy >= timeBetweenBuy)
        {
            other.gameObject.GetComponent<PlayerScript>().money = other.gameObject.GetComponent<PlayerScript>().money - price;
            other.GetComponent<PlayerScript>().AddCola("Stamina");
            GetComponent<AudioSource>().clip = purchaseAcceptedSound;
            GetComponent<AudioSource>().Play();
            timeSinceLastBuy = 0;
        }

        else if(timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space") && other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().clip = purchaseRejectedSound;
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
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase cola (" + price + "): Move faster";
    }
}

private void OnTriggerExit2D(Collider2D other) 
{
    if (other.gameObject.tag == "Player")
    {    
        GameObject canvas = GameObject.Find("Canvas");
        Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
        GameObject purchaseLabel = purchaseLabelTransform.gameObject;
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "";
    }
}
}
