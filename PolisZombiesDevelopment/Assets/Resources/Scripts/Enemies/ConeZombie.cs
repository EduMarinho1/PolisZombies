using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ConeZombie : ZombieScript
{
    public int maxHealth;
    [SerializeField] float health;
    public PlayerScript playerScript;
    float lastAttackTime;
    public Sprite basicZombie;
    private SpriteRenderer spriteRenderer;

    private int zombieLayer = 6;
    private int diggedZombieLayer = 12;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;

        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {GetComponent<AIPath>().maxSpeed = 1.5f;}
        else if(waveThis < 11) {GetComponent<AIPath>().maxSpeed = 2.5f;}
        else if(waveThis < 16) {GetComponent<AIPath>().maxSpeed = 3f;}
        else {GetComponent<AIPath>().maxSpeed = 3.5f;}
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

        if (health <= 50)
        {
            spriteRenderer.sprite = basicZombie;
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
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(1);
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
