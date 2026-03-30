using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazucaScript : WeaponScript
{
    private List<AudioSource> audioSources = new List<AudioSource>();
    
    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }

    public override void Shot(ref int playerAmmo, Transform firePoint)
    {
        firePoint.GetComponent<FirePointScript>().UpdateRotation();
        playerAmmo = playerAmmo - 1;

        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.clip = shootAudio;
            availableSource.Play();
        }

        GameObject bullet = Instantiate(GetComponent<WeaponAcessoriesScript>().bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<BrazucaBulletScript>().TimeToExplode(bulletTime);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        firePoint.GetComponent<FirePointScript>().ResetRotation();
    }

    public override void Reload(ref int playerAmmo, ref int totalPlayerAmmo)
    {
        if (totalPlayerAmmo < totalMagazineAmmo)
        {
            playerAmmo = totalPlayerAmmo;
            totalPlayerAmmo = 0;
        } else {
            totalPlayerAmmo = totalPlayerAmmo - (totalMagazineAmmo - playerAmmo);
            playerAmmo = totalMagazineAmmo;
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return CreateNewAudioSource();
    }

    private AudioSource CreateNewAudioSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }

    // From WeaponScript interface
    public override int GetTotalAmmo()
    {return totalAmmo;}
    public override float GetTimeBetweenShots()
    {return timeBetweenShots;}
}
