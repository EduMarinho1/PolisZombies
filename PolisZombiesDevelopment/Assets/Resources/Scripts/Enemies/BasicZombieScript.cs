using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicZombieScript : ZombieScript
{
    public float maxHealth; // Serves as reference to BallZombieScript
    public float health;
    float lastAttackTime;
    public int damage;
    private AIPath aiPath;
    private Vector2 lastPosition;
    public float stuckTimeLimit = 10f; // Time in seconds before the zombie gets destroyed

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = false;
        StartCoroutine(startMoving());
        StartCoroutine(CheckIfStuck());
        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {GetComponent<AIPath>().maxSpeed = 1.5f;}
        else if(waveThis < 11) {GetComponent<AIPath>().maxSpeed = 2.5f;}
        else if(waveThis < 16) {GetComponent<AIPath>().maxSpeed = 3f;}
        else {GetComponent<AIPath>().maxSpeed = 3.5f;}
    }

    IEnumerator CheckIfStuck()
    {
        float timeSinceLastMove = 0f;

        while (true) // Continuously check if the zombie is stuck
        {
            if (Vector2.Distance(lastPosition, transform.position) < 0.01f) // Threshold for detecting no movement
            {
                timeSinceLastMove += 0.5f; // Increment the timer (adjust interval as needed)
                if (timeSinceLastMove >= stuckTimeLimit)
                {
                    Destroy(gameObject); // Destroy the zombie if stuck for too long
                    yield break; // Exit the coroutine
                }
            }
            else
            {
                timeSinceLastMove = 0f; // Reset the timer if movement is detected
            }

            lastPosition = transform.position; // Update the last known position
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds (adjust as needed)
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
}
