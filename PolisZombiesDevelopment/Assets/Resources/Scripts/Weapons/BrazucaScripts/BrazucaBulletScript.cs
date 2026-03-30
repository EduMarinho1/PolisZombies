using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazucaBulletScript : BulletScript
{
    private bool hitEnemy = false;
    public GameObject bullet;
    public GameObject brazucaExplosion;
    public GameObject weapon;

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
        Instantiate(brazucaExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

