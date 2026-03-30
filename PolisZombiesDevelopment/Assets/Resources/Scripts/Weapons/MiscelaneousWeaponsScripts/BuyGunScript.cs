using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyGunScript : MonoBehaviour
{
    GameObject weaponObject;
    public int totalAmmo;
    public int totalMagazineAmmo;
    public int price;
    private float timeBetweenBuy; //TBB TEM QUE SER PLAYER ATTRIBUTE
    private float timeSinceLastBuy; //TSLB TEM QUE SER PLAYER ATTRIBUTE
    public AudioClip purchaseAcceptedSound;
    public AudioClip purchaseRejectedSound;

void Awake()
{
    weaponObject = gameObject;
    totalAmmo = weaponObject.GetComponent<WeaponScript>().totalAmmo;
    totalMagazineAmmo = weaponObject.GetComponent<WeaponScript>().totalMagazineAmmo;
    price = weaponObject.GetComponent<WeaponScript>().price;
    timeBetweenBuy = 2;
    timeSinceLastBuy = 0;
}

void Update()
{
    timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
}

void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space"))
        {
            PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
            GameObject canvas = GameObject.Find("Canvas");
            Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
            GameObject purchaseLabel = purchaseLabelTransform.gameObject;

            Debug.Log(playerScript.equippedWeaponPrefab.name.Contains("EmptyAccessory"));
            // Buy if player have no guns at all, if its the first gun the player is buying
            if (playerScript.equippedWeaponPrefab.name.Contains("EmptyGun") && other.gameObject.GetComponent<PlayerScript>().money >= price)
            {
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                Debug.Log("Buy gun first");
                playerScript.totalPlayerAmmo = totalAmmo;
                playerScript.totalPlayerMagazineAmmo = totalMagazineAmmo;
                playerScript.playerAmmo = totalMagazineAmmo;
                playerScript.equippedWeaponPrefab = weaponObject;
                playerScript.money = playerScript.money - price;
                playerScript.updateHUDWeaponsAndAccessories();
                playerScript.reloadTime = weaponObject.GetComponent<WeaponScript>().reloadTime;
                playerScript.timeBetweenShots = weaponObject.GetComponent<WeaponScript>().timeBetweenShots;
                purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
            }

            // Reload in equipedPrefab
            else if(playerScript.equippedWeaponPrefab.name == gameObject.name && other.gameObject.GetComponent<PlayerScript>().money >= price/2)
            {
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                Debug.Log("Reload first");
                //playerScript.playerAmmo = totalMagazineAmmo;
                //playerScript.totalPlayerAmmo = totalAmmo;
                playerScript.playerAmmo = playerScript.totalPlayerMagazineAmmo;
                playerScript.totalPlayerAmmo = playerScript.equippedWeaponPrefab.GetComponent<WeaponScript>().totalAmmo;
                playerScript.money = playerScript.money - price / 2;
                purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
            }

            // Reload in secondWeaponPrefab
            else if(playerScript.secondWeaponPrefab.name == gameObject.name && other.gameObject.GetComponent<PlayerScript>().money >= price/2)
            {
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                Debug.Log("Reload second");
                playerScript.secondPlayerAmmo = totalMagazineAmmo;
                playerScript.secondTotalPlayerAmmo = totalAmmo;
                playerScript.money = playerScript.money - price / 2;
                purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
            }

            // Buy gun in secondWeaponPrefav
            else if(playerScript.secondWeaponPrefab.name == "EmptyGun" && other.gameObject.GetComponent<PlayerScript>().money >= price)
            {
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                Debug.Log("Buy gun second");
                playerScript.secondPlayerAmmo = totalMagazineAmmo;
                playerScript.secondTotalPlayerMagazineAmmo = totalMagazineAmmo;
                playerScript.secondWeaponPrefab = weaponObject;
                playerScript.money = playerScript.money - price;
                playerScript.updateHUDWeaponsAndAccessories();
                playerScript.secondTotalPlayerAmmo = totalAmmo;
                purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
            }

            // Buy gun in equippedWeaponPrefab
            else if(other.gameObject.GetComponent<PlayerScript>().money >= price)
            {
                GetComponent<AudioSource>().clip = purchaseAcceptedSound;
                GetComponent<AudioSource>().Play();
                DetachAccessories(playerScript.equippedWeaponPrefab);
                Debug.Log("Buy gun first");
                playerScript.totalPlayerAmmo = totalAmmo;
                playerScript.playerAmmo = totalMagazineAmmo;
                playerScript.totalPlayerMagazineAmmo = totalMagazineAmmo;
                playerScript.equippedWeaponPrefab = weaponObject;
                playerScript.money = playerScript.money - price;
                playerScript.updateHUDWeaponsAndAccessories();
                playerScript.reloadTime = weaponObject.GetComponent<WeaponScript>().reloadTime;
                playerScript.timeBetweenShots = weaponObject.GetComponent<WeaponScript>().timeBetweenShots;
                purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
            }

            else
            {
                GetComponent<AudioSource>().clip = purchaseRejectedSound;
                GetComponent<AudioSource>().Play();
            }

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
        if(other.gameObject.GetComponent<PlayerScript>().equippedWeaponPrefab.name == weaponObject.name || other.gameObject.GetComponent<PlayerScript>().secondWeaponPrefab.name == weaponObject.name)
        {
            purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase ammo (" + price/2 + ")";
        }
        else
        {
            purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase gun (" + price + ")";
        }
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

void DetachAccessories(GameObject equippedWeaponPrefab)
{
    equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().DetachAccessories();
}
}
