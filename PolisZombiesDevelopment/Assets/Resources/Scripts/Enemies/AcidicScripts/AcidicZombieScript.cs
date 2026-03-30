using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AcidicZombieScript : ZombieScript
{
    [SerializeField] float health;
    public GameObject player;
    public GameObject acidSpit;
    private bool hasDied = false;
    float lastAttackTime;

    void Start()
    {
        player = GameObject.Find("Player");
        Spit();
        //
        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {GetComponent<AIPath>().maxSpeed = 1.25f;}
        else if(waveThis < 11) {GetComponent<AIPath>().maxSpeed = 2f;}
        else if(waveThis < 16) {GetComponent<AIPath>().maxSpeed = 2.5f;}
        else {GetComponent<AIPath>().maxSpeed = 3.25f;}
        //
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

    public void Spit()
    {
        StartCoroutine(SpitRepeat());
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

    IEnumerator SpitRepeat()
    {
        while (true)
        {
            if(Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < 15)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                // Instantiate and launch the projectile from the center of the enemy.
                GameObject projectileInstance = Instantiate(acidSpit, transform.position, Quaternion.identity);
                projectileInstance.GetComponent<AcidSpitScript>().Launch(direction);
            }

            yield return new WaitForSeconds(Random.Range(4, 6));
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

