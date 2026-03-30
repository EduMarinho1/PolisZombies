using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour {
    
    public AudioClip shootAudio;
    public float bulletForce;
    public float timeBetweenShots;
    public int totalAmmo;
    public int totalMagazineAmmo;
    public int price;
    public float timeBetweenBuy;
    public float timeSinceLastBuy;
    public float precision;
    public int reloadTime;

//
    public int damage;
    public float bulletTime;
//

    public abstract void Shot(ref int playerAmmo, Transform firePoint);
    public abstract void Reload(ref int playerAmmo, ref int totalPlayerAmmo);
    public abstract int GetTotalAmmo();
    public abstract float GetTimeBetweenShots();
}