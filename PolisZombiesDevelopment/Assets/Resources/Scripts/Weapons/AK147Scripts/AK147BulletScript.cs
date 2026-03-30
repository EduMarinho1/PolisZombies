using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK147BulletScript : BulletScript
{
    private bool hitEnemy = false;
    public GameObject bullet;
    [HideInInspector] public GameObject player;

    public GameObject weapon;

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent) && !hitEnemy)
        {
            hitEnemy = true;
            enemyComponent.TakeDamage(GameObject.Find("AK147").GetComponent<WeaponScript>().damage);
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerScript>().AddMoney(1);
            Debug.Log(player.GetComponent<PlayerScript>().money + " money");

            Instantiate(bloodStainZombieHitted, transform.position, transform.rotation);
        }

        Destroy(bullet);
    }
}

