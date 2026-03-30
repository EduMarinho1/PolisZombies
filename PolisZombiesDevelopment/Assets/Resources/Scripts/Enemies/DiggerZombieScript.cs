using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DiggerZombieScript : ZombieScript
{
    [SerializeField] float health;
    public PlayerScript playerScript;
    private Transform playerTransform;
    float lastAttackTime;
    private Seeker seeker;
    public Sprite diggerZombieSprite;
    public Sprite diggerZombieUndergroundSprite;
    private SpriteRenderer spriteRenderer;
    public AIPath aiPath;
    private Coroutine switchGraphsCoroutine;
    private int zombieLayer = 6;
    private int diggedZombieLayer = 12;
    public float diggerMaxSpeed;

    void Start() {
        playerTransform = GameObject.Find("Player").transform;
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //
        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {diggerMaxSpeed = 1.5f;}
        else if(waveThis < 11) {diggerMaxSpeed = 2f;}
        else if(waveThis < 16) {diggerMaxSpeed = 2.25f;}
        else {diggerMaxSpeed = 2.75f;}
        StartSwitchGraphs();
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

    public void StartSwitchGraphs()
    {
        if (switchGraphsCoroutine == null)
        {
            switchGraphsCoroutine = StartCoroutine(SwitchGraphs());
        }
    }

    public void StopSwitchGraphs()
    {
        if (switchGraphsCoroutine != null)
        {
            StopCoroutine(switchGraphsCoroutine);
            switchGraphsCoroutine = null;
        }
    }

    public IEnumerator SwitchGraphs() {
        while (true)
        {
            if(Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < 100)
            {
                // Consider only the second graph
                GetComponent<AudioSource>().Play();
                gameObject.layer = diggedZombieLayer;
                spriteRenderer.sprite = diggerZombieUndergroundSprite;
                aiPath.maxSpeed = diggerMaxSpeed * 1.5f;
                seeker.graphMask = 1 << 1;
                Debug.Log("Switched to using only the second graph");
                yield return new WaitForSeconds(Random.Range(2, 4));
                GetComponent<AudioSource>().Stop();

                // Consider only the first graph
                gameObject.layer = zombieLayer;
                spriteRenderer.sprite = diggerZombieSprite;
                aiPath.maxSpeed = diggerMaxSpeed;
                seeker.graphMask = 1 << 0;
                Debug.Log("Using only the first graph");
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
