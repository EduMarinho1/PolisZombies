using System.Collections;
using UnityEngine;
using Pathfinding;

public class TankZombieScript : ZombieScript
{
    [SerializeField] float health;
    public PlayerScript playerScript;
    float lastAttackTime;
    public int damage;
    public float pushForce = 10f;
    public float pushDuration = 0.25f;

    public float rushSpeed = 2f;
    public float rushDuration = 0.5f;
    public float rushCooldown = 10f;
    private bool isRushing = false;
    private Rigidbody2D rb;
    private bool canRotate;
    private AIPath aiPath;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        canRotate = true;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RushPeriodically());

        int waveThis = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave;
        if(waveThis < 6) {GetComponent<AIPath>().maxSpeed = 3f;}
        else if(waveThis < 11) {GetComponent<AIPath>().maxSpeed = 3.25f;}
        else if(waveThis < 16) {GetComponent<AIPath>().maxSpeed = 3.5f;}
        else {GetComponent<AIPath>().maxSpeed = 4f;}
    }

    void Update()
    {
        if (canRotate)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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

        if (health <= 0)
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

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision with player detected");
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            Rigidbody2D playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            lastAttackTime = Time.time;

            if (playerRigidbody != null)
            {
                Debug.Log("Player Rigidbody2D found");
                Vector2 pushDirection = (other.gameObject.transform.position - transform.position).normalized;
                other.gameObject.GetComponent<PlayerScript>().TemporarilyRemoveMovement(pushDuration + 0.5f);
                StartCoroutine(PushPlayer(playerRigidbody, pushDirection));
            }
        }
    }

    private IEnumerator PushPlayer(Rigidbody2D playerRigidbody, Vector2 direction)
    {
        Vector2 originalVelocity = playerRigidbody.velocity;
        playerRigidbody.velocity = direction * pushForce;

        yield return new WaitForSeconds(pushDuration);

        playerRigidbody.velocity = originalVelocity;
    }

    private IEnumerator RushPeriodically()
    {
        while (true)
        {
            if(Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < 10)
            {
                GetComponent<AudioSource>().Play();
                StartCoroutine(Rush());
            }

            yield return new WaitForSeconds(rushCooldown);
        }
    }

    private IEnumerator Rush()
    {
        aiPath.canMove = false;
        aiPath.enableRotation = false;
        canRotate = false;
        isRushing = true;

        Vector2 rushDirection = transform.up; // Assuming the zombie moves in the direction it is facing
        rb.velocity = rushDirection * rushSpeed * 2;

        yield return new WaitForSeconds(rushDuration);

        rb.velocity = Vector2.zero; // Stop rushing
        isRushing = false;
        canRotate = true;
        aiPath.canMove = true;
        aiPath.enableRotation = true;
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