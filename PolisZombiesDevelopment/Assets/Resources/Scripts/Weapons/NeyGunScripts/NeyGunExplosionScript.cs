using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeyGunExplosionScript : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    void Start()
    {
        PlayerScript playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        int neyGunDamage = GameObject.Find("NeyGun").GetComponent<WeaponScript>().damage;
        circleCollider = GetComponent<CircleCollider2D>();
        float explosionRadius = circleCollider.radius * 1.5f;
        int enemyLayer = 1 << 6;
        int shockLayer = 1 << 16;
        int playerLayer = 1 << 3;
        circleCollider.enabled = false;
        
        Collider2D[] hitCollidersEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D hitCollider in hitCollidersEnemies)
        {
            // Try to get the Enemy component (or any script that handles taking damage)
            ZombieScript enemy = hitCollider.GetComponent<ZombieScript>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeExplosionDamage(neyGunDamage, 4);
            }
        }

        Collider2D[] hitCollidersShockEnemy = Physics2D.OverlapCircleAll(transform.position, explosionRadius, shockLayer);
        foreach (Collider2D hitCollider in hitCollidersShockEnemy)
        {
            // Try to get the Enemy component (or any script that handles taking damage)
            ZombieScript enemy = hitCollider.GetComponent<ZombieScript>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeExplosionDamage(neyGunDamage, 4);
            }
        }

        Collider2D[] hitCollidersPlayer = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);

        if(hitCollidersPlayer.Length > 0)
        {
            playerScript.TakeExplosionDamage(1);
        }

        StartCoroutine(EndExplosion());
    }

    IEnumerator EndExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        var transform = GetComponent<Transform>();
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

        Destroy(gameObject);
    }
}
