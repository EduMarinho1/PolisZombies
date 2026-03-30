using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShockZombieScript : ZombieScript
{
    public float maxHealth; // Serves as reference to BallZombieScript
    public float health;
    float lastAttackTime;
    public int damage;
    private AIPath aiPath;
    public GameObject littleShocks;
    public GameObject shockWavePrefab;
    public GameObject zapAudioChildPrefab;
    public GameObject shockTrail;
    public Transform player;
    public Sprite shockBallSprite;
    public Sprite shockZombieSprite;
    private bool rorationActive;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = false;
        StartCoroutine(startMoving());
        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        GetComponent<AIPath>().maxSpeed = 3f;

        StartCoroutine(RotateChildSprite());
        StartCoroutine(ShockBall());
        StartCoroutine(ShockWave());
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction vector from the object to the player
            Vector2 direction = player.position - transform.position;

            // Calculate the angle in degrees and set the rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    IEnumerator startMoving() {
        yield return new WaitForSeconds(0.6f);
        aiPath.canMove = true;
    }

    public override void TakeDamage(int damageAmount)
    {
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

        if(health <= 0)
        {

            if (availableSource != null)
            {
                // Create a new GameObject for the AudioSource
                GameObject audioObject = new GameObject("ZombieDeathSound");
                AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
                newAudioSource.clip = availableSource.clip;
                newAudioSource.Play();
                Destroy(audioObject, newAudioSource.clip.length);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        GetComponent<SpriteRenderer>().sprite = shockZombieSprite;
        aiPath.canMove = true;
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

    IEnumerator RotateChildSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            littleShocks.transform.rotation = Quaternion.Euler(0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            littleShocks.transform.rotation = Quaternion.Euler(0, 0, 90);
            yield return new WaitForSeconds(0.5f);
            littleShocks.transform.rotation = Quaternion.Euler(0, 0, 180);
            yield return new WaitForSeconds(0.5f);
            littleShocks.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    IEnumerator ShockWave() {
        while (true) {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < 15) {
                yield return new WaitForSeconds(10);
                Destroy(Instantiate(shockWavePrefab, transform.position, transform.rotation), 2f);
            }

            yield return new WaitForSeconds(Random.Range(4, 6));
        }
    }

    IEnumerator ShockBall() {
        while (true) {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < 15) {
                // Disable movement while sprinting
                aiPath.canMove = false;

                // Lock the current direction of the object
                Vector2 sprintDirection = transform.right.normalized;

                // Apply a strong force in the locked direction
                GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Clear existing velocity
                GetComponent<Rigidbody2D>().AddForce(sprintDirection * 1200);

                // Change sprite to indicate sprinting
                GetComponent<SpriteRenderer>().sprite = shockBallSprite;

                // Play zap sound
                zapAudioChildPrefab.GetComponent<AudioSource>().Play();

                // Create shock trails while sprinting
                for (int i = 0; i < 8; i++) {
                    Destroy(Instantiate(shockTrail, transform.position, transform.rotation * Quaternion.Euler(0, 0, 90)), 0.3f);
                    yield return new WaitForSeconds(0.1f);
                }
            }

            // Reset to normal behavior after sprint
            GetComponent<SpriteRenderer>().sprite = shockZombieSprite;
            aiPath.canMove = true;

            // Wait before the next sprint
            yield return new WaitForSeconds(Random.Range(7, 10));
    }
}
}
