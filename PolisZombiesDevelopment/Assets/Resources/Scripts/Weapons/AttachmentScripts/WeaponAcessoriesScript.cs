using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAcessoriesScript : MonoBehaviour                                 //TODO: Só existem 4 ou 5 acessorios no total (1 acessorio para cada tipo de acessorio),
                                                                                    // e o jogador pode comprar só 1 acessorio. O resto dos acessorios tem que ser adquirido
{                                                                                   // aleatoriamente na dubious box, sendo que a dubious box dá armas aleatorias e acessorios (pode ser
                                                                                    // entre 0 acessorios e todos os  slots de acessorios do jogo -1) para essa arma que foi comprada.
    // Current weapon GameObject
    private GameObject thisWeapon;

    // Bullet that guns shoots (not worth the time to put this into the weapon scripts)
    public GameObject bulletPrefab;

    // Accessories prefabs
    public GameObject bulletPrefabAccessory;
    public GameObject magazinePrefab;
    public GameObject scopePrefab;
    public GameObject barrelPrefab;
    public GameObject bagPrefab;
    public GameObject emptyAccessoryPrefab;
    //public bool playerAccessoryPurchased;

    // How much each accessory changes
    public int changeBulletFactor;
    public int changeMagazineFactor;
    public int changeScopeFactor;
    public float changeBarrelFactor;
    public int changeBagFactor;

    void Start()
    {        
        //playerAccessoryPurchased = false;

        bulletPrefabAccessory = emptyAccessoryPrefab;
        magazinePrefab = emptyAccessoryPrefab;
        scopePrefab = emptyAccessoryPrefab;
        barrelPrefab = emptyAccessoryPrefab;
        bagPrefab = emptyAccessoryPrefab;
    }

    public void BuyAccessory(GameObject accessory)
    {
        //playerAccessoryPurchased = true;
        AttachAccessories(accessory);
    }


    public void AttachAccessories(GameObject accessory)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(accessory.name.Contains("Bullet") && bulletPrefabAccessory.name == "EmptyAccessory")
        {
            bulletPrefabAccessory = accessory;
            gameObject.GetComponent<WeaponScript>().damage = gameObject.GetComponent<WeaponScript>().damage + changeBulletFactor;
        }

        if(accessory.name.Contains("Magazine") && magazinePrefab.name.Contains("EmptyAccessory"))
        {
            magazinePrefab = accessory;
            gameObject.GetComponent<WeaponScript>().totalMagazineAmmo = gameObject.GetComponent<WeaponScript>().totalMagazineAmmo + changeMagazineFactor;
            player.GetComponent<PlayerScript>().totalPlayerMagazineAmmo = player.GetComponent<PlayerScript>().totalPlayerMagazineAmmo + changeMagazineFactor;
        }

        if(accessory.name.Contains("Scope") && scopePrefab.name == "EmptyAccessory")
        {
            scopePrefab = accessory;
            gameObject.GetComponent<WeaponScript>().precision = gameObject.GetComponent<WeaponScript>().precision - changeScopeFactor;
        }

        if(accessory.name.Contains("Barrel") && barrelPrefab.name == "EmptyAccessory")
        {
            barrelPrefab = accessory;
            gameObject.GetComponent<WeaponScript>().bulletTime =  gameObject.GetComponent<WeaponScript>().bulletTime + changeBarrelFactor;
        }

        if(accessory.name.Contains("Bag") && bagPrefab.name == "EmptyAccessory")
        {
            bagPrefab = accessory;
            gameObject.GetComponent<WeaponScript>().totalAmmo = gameObject.GetComponent<WeaponScript>().totalAmmo + changeBagFactor;
        }

        player.GetComponent<PlayerScript>().updateHUDWeaponsAndAccessories();
    }

    public void AttachAccessoriesSecondaryWeapon(GameObject accessory)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(accessory.name.Contains("Bullet") && bulletPrefabAccessory.name == "EmptyAccessory")
        {
            bulletPrefabAccessory = accessory;
            gameObject.GetComponent<WeaponScript>().damage = gameObject.GetComponent<WeaponScript>().damage + changeBulletFactor;
        }

        if(accessory.name.Contains("Magazine") && magazinePrefab.name.Contains("EmptyAccessory"))
        {
            magazinePrefab = accessory;
            gameObject.GetComponent<WeaponScript>().totalMagazineAmmo = gameObject.GetComponent<WeaponScript>().totalMagazineAmmo + changeMagazineFactor;
            player.GetComponent<PlayerScript>().secondTotalPlayerMagazineAmmo = player.GetComponent<PlayerScript>().secondTotalPlayerMagazineAmmo + changeMagazineFactor;
        }

        if(accessory.name.Contains("Scope") && scopePrefab.name == "EmptyAccessory")
        {
            scopePrefab = accessory;
            gameObject.GetComponent<WeaponScript>().precision = gameObject.GetComponent<WeaponScript>().precision - changeScopeFactor;
        }

        if(accessory.name.Contains("Barrel") && barrelPrefab.name == "EmptyAccessory")
        {
            barrelPrefab = accessory;
            gameObject.GetComponent<WeaponScript>().bulletTime =  gameObject.GetComponent<WeaponScript>().bulletTime + changeBarrelFactor;
        }

        if(accessory.name.Contains("Bag") && barrelPrefab.name == "EmptyAccessory")
        {
            bagPrefab = accessory;
            gameObject.GetComponent<WeaponScript>().totalAmmo = gameObject.GetComponent<WeaponScript>().totalAmmo + changeBagFactor;
        }

        player.GetComponent<PlayerScript>().updateHUDWeaponsAndAccessories();
    }

    public void DetachAccessories()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (bulletPrefabAccessory.name != "EmptyAccessory")
        {
            gameObject.GetComponent<WeaponScript>().damage = gameObject.GetComponent<WeaponScript>().damage - changeBulletFactor;
        }

        if (magazinePrefab.name != "EmptyAccessory")
        {
            gameObject.GetComponent<WeaponScript>().totalMagazineAmmo = gameObject.GetComponent<WeaponScript>().totalMagazineAmmo - changeMagazineFactor;
        }

        if (scopePrefab.name != "EmptyAccessory")
        {
            gameObject.GetComponent<WeaponScript>().precision = gameObject.GetComponent<WeaponScript>().precision + changeScopeFactor;
        }

        if (barrelPrefab.name != "EmptyAccessory")
        {
             gameObject.GetComponent<WeaponScript>().bulletTime =  gameObject.GetComponent<WeaponScript>().bulletTime - changeBarrelFactor;
        }

        if (bagPrefab.name != "EmptyAccessory")
        {
            gameObject.GetComponent<WeaponScript>().totalAmmo = gameObject.GetComponent<WeaponScript>().totalAmmo - changeBagFactor;
        }

        bulletPrefabAccessory = emptyAccessoryPrefab;
        magazinePrefab = emptyAccessoryPrefab;
        scopePrefab = emptyAccessoryPrefab;
        barrelPrefab = emptyAccessoryPrefab;
        bagPrefab = emptyAccessoryPrefab;

        //playerAccessoryPurchased = false;

        player.GetComponent<PlayerScript>().updateHUDWeaponsAndAccessories();
    }
}
