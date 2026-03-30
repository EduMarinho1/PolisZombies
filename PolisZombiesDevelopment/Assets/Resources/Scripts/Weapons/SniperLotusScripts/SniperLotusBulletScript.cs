using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperLotusBulletScript :BulletScript
{
    public GameObject bullet;
    [HideInInspector] public GameObject player;
    public GameObject weapon;
    private HashSet<Collider2D> enemiesHit = new HashSet<Collider2D>();

    public int maxZombiesHit;

    public int zombiesHitted;

    void Start() {
        maxZombiesHit = 5;
        zombiesHitted = 0;
    }


    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent) && !enemiesHit.Contains(collision))
        {
            enemiesHit.Add(collision);
            enemyComponent.TakeDamage(GameObject.Find("SniperLotus").GetComponent<WeaponScript>().damage);
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerScript>().AddMoney(4);
            Debug.Log(player.GetComponent<PlayerScript>().money + " money");
            Instantiate(bloodStainZombieHitted, transform.position, transform.rotation);
            zombiesHitted = zombiesHitted + 1;
            if (zombiesHitted >= maxZombiesHit) {
                Debug.Log("Sniper Buller reached maximum zombies");
                Destroy(bullet);
            }

        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Zombie"))
        {
            Destroy(bullet);
        }
    }
}
