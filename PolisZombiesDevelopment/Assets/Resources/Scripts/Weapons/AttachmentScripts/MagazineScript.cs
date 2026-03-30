using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagazineScript : MonoBehaviour
{
    public int price = 2;
    public AudioClip purchaseAcceptedSound;
    public AudioClip purchaseRejectedSound;
    private float timeBetweenBuy;
    private float timeSinceLastBuy;
    
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenBuy = 2;
        timeSinceLastBuy = 0;
    }

    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space")
                && other.gameObject.GetComponent<PlayerScript>().money >= price && !other.gameObject.GetComponent<PlayerScript>().equippedWeaponPrefab.name.Contains("EmptyGun")
                    //&& other.gameObject.GetComponent<PlayerScript>().equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().playerAccessoryPurchased == false
                    )
        {
            if (!other.gameObject.GetComponent<PlayerScript>().equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().magazinePrefab.name.Contains("Magazine"))
                {
                Debug.Log("MagBought");
                other.gameObject.GetComponent<PlayerScript>().money = other.gameObject.GetComponent<PlayerScript>().money - price;
                other.GetComponent<PlayerScript>().equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().BuyAccessory(transform.parent.gameObject);
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                timeSinceLastBuy = 0;    
            }            
            else
            {
                GetComponent<AudioSource>().clip = purchaseRejectedSound;
                GetComponent<AudioSource>().Play();
                timeSinceLastBuy = 0;
            }    
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
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase(" + price + "): Increased ammo in magazine";
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
