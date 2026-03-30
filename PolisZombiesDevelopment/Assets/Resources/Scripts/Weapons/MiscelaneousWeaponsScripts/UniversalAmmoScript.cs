using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UniversalAmmoScript : MonoBehaviour
{
    public int price;
    private float timeBetweenBuy; //TBB TEM QUE SER PLAYER ATTRIBUTE
    private float timeSinceLastBuy; //TSLB TEM QUE SER PLAYER ATTRIBUTE
    public AudioClip purchaseAcceptedSound;
    public AudioClip purchaseRejectedSound;

void Awake()
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
        if(other.gameObject.tag == "Player" && timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space") && other.gameObject.GetComponent<PlayerScript>().money >= price && !other.gameObject.GetComponent<PlayerScript>().equippedWeaponPrefab.name.Contains("EmptyGun"))
        {
            GetComponent<AudioSource>().clip = purchaseAcceptedSound;
            GetComponent<AudioSource>().Play();
            PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.playerAmmo = playerScript.totalPlayerMagazineAmmo;
            playerScript.totalPlayerAmmo = playerScript.equippedWeaponPrefab.GetComponent<WeaponScript>().totalAmmo;
            playerScript.money = playerScript.money - price;

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
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price + ")";
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
