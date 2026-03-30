using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GrotesqueZombieScript : ZombieScript
{
    [SerializeField] float health;
    public Transform playerTransform;
    public TongueScript tongueScript;
    public GameObject grotesqueVisionArea;
    private bool canRotate = true;
    float lastAttackTime;
    float lastHitTime;
    int damage;

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        // Find the TongueScript component in the children of this GameObject
        tongueScript = GetComponentInChildren<TongueScript>();
        //
        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {GetComponent<AIPath>().maxSpeed = 1.5f;}
        else if(waveThis < 11) {GetComponent<AIPath>().maxSpeed = 2f;}
        else if(waveThis < 16) {GetComponent<AIPath>().maxSpeed = 2.5f;}
        else {GetComponent<AIPath>().maxSpeed = 3f;}
        //
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (Time.time - lastHitTime < 2)
        {
            return;
        }

        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            lastHitTime = Time.time;
        }
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

    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (canRotate)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            Vector3 direction = playerTransform.position - transform.position;
            direction.z = 0;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Check if it's time to attack
        if (Time.time - lastAttackTime >= 4 && grotesqueVisionArea.GetComponent<GrotesqueVisionAreaScript>().CanAttack() && Vector3.Distance(playerTransform.position, gameObject.transform.position) < 10)
        {
            GetComponent<AudioSource>().Play();
            canRotate = false;
            tongueScript.StartAttack();
            StartCoroutine(CanRotateAgain());
            lastAttackTime = Time.time;
        }
    }

    IEnumerator CanRotateAgain()
    {
        yield return new WaitForSeconds(1.1f);
        canRotate = true;
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