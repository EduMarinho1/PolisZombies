using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BallZombieScript : ZombieScript
{
    public float maxHealth;
    public float health;
    float lastAttackTime;
    public int damage;
    public GameObject basicZombie;
    public GameObject explosionSoundPrefab;
    public Sprite smallBall;
    public Sprite bigBall;
    private float damageThreshold = 700f; // Threshold to trigger zombie split
    private float accumulatedDamage = 0f; // Tracks damage taken since last split
    private bool previousStateBig;

    void Start() {
        previousStateBig = true;
        StartCoroutine(FlipBall());
        GetComponent<CircleCollider2D>().radius = 0.8f;
        GetComponent<AIPath>().maxSpeed = 2f;
    }

    public override void TakeDamage(int damageAmount)
    {
        accumulatedDamage = accumulatedDamage + damageAmount;

        // Check if accumulated damage exceeds the threshold
        if (accumulatedDamage >= damageThreshold)
        {
            explosionSoundPrefab.GetComponent<AudioSource>().Play();
            SpawnZombiesAndPush(); // Call the function
            health = health - 5 * basicZombie.GetComponent<BasicZombieScript>().maxHealth;
            accumulatedDamage = 0; // Reset the counter
        }

        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.clip = zombieDamagedAudio;
            availableSource.Play();
        }


        PlayerScript playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        int critChance = Random.Range(0, 100);
        if(playerScript.HasCola("HeadShot") && critChance < playerScript.criticalChance)
        {
            health = health - damageAmount * playerScript.criticalMultiplier;
        } else {
            health = health - damageAmount;
        }

        if(health <= maxHealth/3 && previousStateBig) {
            previousStateBig = false;
            explosionSoundPrefab.GetComponent<AudioSource>().Play();
            SpawnZombiesAndPush(); // Call the function
            health = health - 5 * basicZombie.GetComponent<BasicZombieScript>().maxHealth;
            accumulatedDamage = 0; // Reset the counter
            GetComponent<SpriteRenderer>().sprite = smallBall;
            GetComponent<CircleCollider2D>().radius = 0.5f;
            GetComponent<AIPath>().maxSpeed = 3f;
        }

        if(health <= 0)
        {

            if (availableSource != null)
            {
                SpawnZombiesAndPush(); // Call the function
                health = health - 5 * basicZombie.GetComponent<BasicZombieScript>().maxHealth;
                accumulatedDamage = 0; // Reset the counter
                // Create a new GameObject for the AudioSource
                GameObject audioObject = new GameObject("ZombieDeathSound");
                AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
                newAudioSource.clip = explosionSoundPrefab.GetComponent<AudioSource>().clip;
                newAudioSource.Play();
                Destroy(audioObject, newAudioSource.clip.length);
            }

            Instantiate(basicZombie, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<BasicZombieScript>(out BasicZombieScript enemyComponent) && health < maxHealth)
        {
            Destroy(other.gameObject);
            heal(enemyComponent.health);
        }

        if (Time.time - lastAttackTime < 2)
        {
            return;
        }

        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    private void heal(float recover) {
        health = health + recover;
        if(health > maxHealth/3 && !previousStateBig) {
            previousStateBig = true;
            GetComponent<SpriteRenderer>().sprite = bigBall;
            GetComponent<CircleCollider2D>().radius = 0.8f;
            GetComponent<AIPath>().maxSpeed = 2f;
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

    IEnumerator FlipBall()
    {
        while (true)
        {
            Debug.Log("flip");
            yield return new WaitForSeconds(0.7f);
            var transform = GetComponent<Transform>();
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(0.7f);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }

private void SpawnZombiesAndPush()
{
    // Instantiate 5 zombies and push them outward
    for (int i = 0; i < 5; i++)
    {
        // Calculate a random angle
        float randomAngle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;

        // Calculate a random distance (minimum 5 units, maximum 7 units for example)
        float randomDistance = 1.5f;

        // Calculate spawn position
        Vector2 spawnPosition = (Vector2)transform.position + direction * randomDistance;

        // Instantiate the zombie at the calculated position
        GameObject newZombie = Instantiate(basicZombie, spawnPosition, Quaternion.identity);

        // Apply force to the zombie
        Rigidbody2D rb = newZombie.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float forceMagnitude = 7f; // Adjust the force as needed
            rb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse); // Use Impulse for immediate force application
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on spawned zombie prefab!");
        }
    }
}
}
