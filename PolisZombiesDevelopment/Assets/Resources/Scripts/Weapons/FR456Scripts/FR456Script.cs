using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FR456Script : WeaponScript
{
    private int localAmmo;
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }

    public override void Shot(ref int playerAmmo, Transform firePoint)
    {
        localAmmo = playerAmmo;
        StartCoroutine(RajadaFR456(firePoint));
        if (playerAmmo >= 3) {playerAmmo = playerAmmo - 3;}
        else if (playerAmmo == 2) {playerAmmo = playerAmmo - 2;}
        else {playerAmmo = playerAmmo - 1;} // when player ammo = 1
    }

    IEnumerator RajadaFR456(Transform firePoint)
    {
        for(int i = 0; i < 3 ; i++)
        {
            if(localAmmo > 0)
            {
            localAmmo = localAmmo - 1;
            firePoint.GetComponent<FirePointScript>().UpdateRotation();

            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = shootAudio;
                availableSource.Play();
            }

            GameObject bullet2 = Instantiate(GetComponent<WeaponAcessoriesScript>().bulletPrefab, firePoint.position, firePoint.rotation);
            Destroy(bullet2, bulletTime);
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            rb2.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            firePoint.GetComponent<FirePointScript>().ResetRotation();
            yield return new WaitForSeconds(0.07f);
            }
        }
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
