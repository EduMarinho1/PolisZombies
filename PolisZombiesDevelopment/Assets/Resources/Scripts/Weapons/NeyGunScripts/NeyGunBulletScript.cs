using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeyGunBulletScript : BulletScript
{
    private bool hitEnemy = false;
    public GameObject bullet;
    public GameObject neyGunExplosion;
    public GameObject weapon;
    public GameObject NeyGunCloud;

    void Start()
    {
        StartCoroutine(MakeClouds());
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent) && !hitEnemy)
        {
            hitEnemy = true;
        }

        MakeExplosion();
    }

    public void TimeToExplode(float bulletTime)
    {
        StartCoroutine(TTE(bulletTime));
    }

    IEnumerator TTE(float bulletTime)
    {
        yield return new WaitForSeconds(bulletTime);
        MakeExplosion();
    }

    public void MakeExplosion()
    {
        Instantiate(neyGunExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator MakeClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.04f);
            Instantiate(NeyGunCloud, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90));

        }
    }
}

