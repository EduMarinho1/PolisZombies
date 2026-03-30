using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundScript : MonoBehaviour
{
    //General variables
    private CircleCollider2D circleCollider;
    private float timeBetweenChecks;
    private float timePassedSinceLastCheck;
    private int currentBasicZombieAudio;
    private int currentBomberZombieAudio;
    private int currentAcidicZombieAudio;
    private int currentGrotesqueZombieAudio;
    private int currentSmokerZombieAudio;
    private int currentDogZombieAudio;
    private int currentTankZombieAudio;
    private int currentDiggerZombieAudio;
    private int currentCrawlerZombieAudio;
    private bool smokeBool;

    //Booleans variable if new sound can be made
    private bool basicZombieBool;
    private bool bomberZombieBool;
    private bool acidicZombieBool;
    private bool grotesqueZombieBool;
    private bool smokerZombieBool;
    private bool tankZombieBool;
    private bool diggerZombieBool;
    private bool dogZombieBool;
    private bool crawlerZombieBool;

    //Audio sources
    private AudioSource[] audioSources;
    private AudioSource basicZombieAudioSource;
    private AudioSource bomberZombieAudioSource;
    private AudioSource acidicZombieAudioSource;
    private AudioSource grotesqueZombieAudioSource;
    private AudioSource smokerZombieAudioSource;
    private AudioSource tankZombieAudioSource;
    private AudioSource diggerZombieAudioSource;
    private AudioSource dogZombieAudioSource;
    private AudioSource smokeAudioSource;
    private AudioSource crawlerZombieAudioSource;

    //Audio clips
    public AudioClip basicZombieAudioClip1;
    public AudioClip basicZombieAudioClip2;
    public AudioClip basicZombieAudioClip3;
    public AudioClip basicZombieAudioClip4;
    public AudioClip basicZombieAudioClip5;
    public AudioClip basicZombieAudioClip6;
    public AudioClip bomberZombieAudioClip1;
    public AudioClip bomberZombieAudioClip2;
    public AudioClip bomberZombieAudioClip3;
    public AudioClip acidicZombieAudioClip1;
    public AudioClip acidicZombieAudioClip2;
    public AudioClip acidicZombieAudioClip3;
    public AudioClip grotesqueZombieAudioClip1;
    public AudioClip grotesqueZombieAudioClip2;
    public AudioClip grotesqueZombieAudioClip3;
    public AudioClip smokerZombieAudioClip1;
    public AudioClip smokerZombieAudioClip2;
    public AudioClip smokerZombieAudioClip3;
    public AudioClip tankZombieAudioClip1;
    public AudioClip tankZombieAudioClip2;
    public AudioClip tankZombieAudioClip3;
    public AudioClip diggerZombieAudioClip1;
    public AudioClip diggerZombieAudioClip2;
    public AudioClip diggerZombieAudioClip3;
    public AudioClip dogZombieBarkAudioClip;
    public AudioClip dogZombieAudioClip1;
    public AudioClip dogZombieAudioClip2;
    public AudioClip dogZombieAudioClip3;
    public AudioClip smokeAudioClip;
    public AudioClip crawlerZombieAudioClip1;
    public AudioClip crawlerZombieAudioClip2;
    public AudioClip crawlerZombieAudioClip3;

    void Start()
    {
        currentBasicZombieAudio = 0;
        currentBomberZombieAudio = 0;
        currentAcidicZombieAudio = 0;
        currentGrotesqueZombieAudio = 0;
        currentSmokerZombieAudio = 0;
        currentDogZombieAudio = 0;
        currentTankZombieAudio = 0;
        currentDiggerZombieAudio = 0;
        currentCrawlerZombieAudio = 0;
        Debug.Log("start");
        timeBetweenChecks = 1;
        circleCollider = GetComponent<CircleCollider2D>();

        audioSources = GetComponents<AudioSource>();
        basicZombieAudioSource = audioSources[0];
        bomberZombieAudioSource = audioSources[1];
        acidicZombieAudioSource = audioSources[2];
        grotesqueZombieAudioSource = audioSources[3];
        smokerZombieAudioSource = audioSources[4];
        tankZombieAudioSource = audioSources[5];
        diggerZombieAudioSource = audioSources[6];
        dogZombieAudioSource = audioSources[7];
        smokeAudioSource = audioSources[8];
        crawlerZombieAudioSource = audioSources[9];

        dogZombieAudioSource.clip = dogZombieAudioClip1;
        smokeAudioSource.clip = smokeAudioClip;

        basicZombieBool = true;
        bomberZombieBool = true;
        acidicZombieBool = true;
        grotesqueZombieBool = true;
        smokerZombieBool = true;
        smokeBool = true;
        tankZombieBool = true;
        diggerZombieBool = true;
        dogZombieBool = true;
        crawlerZombieBool = true;
    }

    void Update()
    {
        timePassedSinceLastCheck = timePassedSinceLastCheck + Time.deltaTime;
        if(timePassedSinceLastCheck > timeBetweenChecks)
        {
            timePassedSinceLastCheck = 0;
            Debug.Log("1 second passed");
            PlayerScript playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
            float checkRadius = circleCollider.radius;
            int enemyLayer = 1 << 6;

            Collider2D[] hitCollidersEnemies = Physics2D.OverlapCircleAll(transform.position, checkRadius, enemyLayer);
            int basicZombieCount = 0;

            foreach (Collider2D hitCollider in hitCollidersEnemies)
            {
                ZombieScript basicZombie = hitCollider.GetComponent<BasicZombieScript>();
                ZombieScript crawlerZombie = hitCollider.GetComponent<CrawlerZombieScript>();
                ZombieScript coneZombie = hitCollider.GetComponent<ConeZombie>();

                ZombieScript bomberZombie = hitCollider.GetComponent<BomberZombieScript>();

                ZombieScript acidicZombie = hitCollider.GetComponent<AcidicZombieScript>();
                ZombieScript grotesqueZombie = hitCollider.GetComponent<GrotesqueZombieScript>();
                ZombieScript tankZombie = hitCollider.GetComponent<TankZombieScript>();
                ZombieScript diggerZombie = hitCollider.GetComponent<DiggerZombieScript>();
                string spriteName = hitCollider.GetComponent<SpriteRenderer>().sprite.name;

                if ((basicZombie != null || coneZombie != null))
                {
                    basicZombieCount++;
                }

                Debug.Log("basic zombie sound: basicZombieCount = " + basicZombieCount);
                if (basicZombieCount > 0 && basicZombieBool)
                {
                    basicZombieBool = false;
                    StartCoroutine(BasicZombieSound(basicZombieCount));
                }

                if (crawlerZombie != null && crawlerZombieBool)
                {
                    Debug.Log("bomber sound");
                    crawlerZombieBool = false;
                    StartCoroutine(SpecialZombieSound(crawlerZombieAudioSource, "crawler", crawlerZombieAudioClip1, crawlerZombieAudioClip2, crawlerZombieAudioClip3));
                }

                if (bomberZombie != null && bomberZombieBool)
                {
                    Debug.Log("bomber sound");
                    bomberZombieBool = false;
                    StartCoroutine(SpecialZombieSound(bomberZombieAudioSource, "bomber", bomberZombieAudioClip1, bomberZombieAudioClip2, bomberZombieAudioClip3));
                }

                if (acidicZombie != null && acidicZombieBool)
                {
                    acidicZombieBool = false;
                    StartCoroutine(SpecialZombieSound(acidicZombieAudioSource, "acidic", acidicZombieAudioClip1, acidicZombieAudioClip2, acidicZombieAudioClip3));
                }

                if (grotesqueZombie != null && grotesqueZombieBool)
                {
                    grotesqueZombieBool = false;
                    StartCoroutine(SpecialZombieSound(grotesqueZombieAudioSource, "grotesque", grotesqueZombieAudioClip1, grotesqueZombieAudioClip2, grotesqueZombieAudioClip3));
                }

                if (tankZombie != null && tankZombieBool)
                {
                    tankZombieBool = false;
                    StartCoroutine(SpecialZombieSound(tankZombieAudioSource, "tank", tankZombieAudioClip1, tankZombieAudioClip2, tankZombieAudioClip3));
                }

                if (diggerZombie != null && diggerZombieBool)
                {
                    diggerZombieBool = false;
                    StartCoroutine(SpecialZombieSound(diggerZombieAudioSource, "digger", diggerZombieAudioClip1, diggerZombieAudioClip2, diggerZombieAudioClip3));
                }

                if(spriteName == "Smoker")
                {
                    if(smokerZombieBool)
                    {
                        smokerZombieBool = false;
                        StartCoroutine(SpecialZombieSound(smokerZombieAudioSource, "smoker", smokerZombieAudioClip1, smokerZombieAudioClip2, smokerZombieAudioClip3));
                    }

                    if(smokeBool)
                    {
                        smokeBool = false;
                        StartCoroutine(SmokeSound());
                    }
                }

                if(spriteName == "DogZombie" && dogZombieBool)
                {
                    WavesManagerScript wms = GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>();
                    Debug.Log("this wave " + wms.thisWave + " dog wave  " + wms.thisWave);
                    if(wms.thisWave == wms.dogWave)
                    {
                        dogZombieBool = false;
                        StartCoroutine(SpecialZombieSound(dogZombieAudioSource, "dog", dogZombieAudioClip1, dogZombieAudioClip2, dogZombieAudioClip3));
                    }

                    else
                    {
                        dogZombieBool = false;
                        StartCoroutine(DogZombieBarkSound());
                    }
                }
            }            
        }
    }

    IEnumerator SmokeSound()
    {
        Debug.Log("smokeSound");
        smokeAudioSource.Play();
        yield return new WaitForSeconds(smokeAudioClip.length);
        smokeBool = true;
    }

    IEnumerator BasicZombieSound(int basicZombieCount)
    {
        Debug.Log("basic zombie sound: ienumerator called");
        int r = 0;
        Debug.Log("basic zombie sound: wave " + GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave);
        if(GameObject.Find("SpawnerManager").GetComponent<WavesManagerScript>().thisWave < 11)
        {
            r = Random.Range(1, 4);
            while(currentBasicZombieAudio == r){r = Random.Range(1, 4);}
        }
        else
        {
            r = Random.Range(4, 7);
            while(currentBasicZombieAudio == r){r = Random.Range(4, 7);}
        }

        currentBasicZombieAudio = r;

        Debug.Log("basic zombie sound: r = " + r);
        Debug.Log("basic zombie sound: basicZombieCount = " + basicZombieCount);
        if(r == 1) {basicZombieAudioSource.clip = basicZombieAudioClip1;}
        if(r == 2) {basicZombieAudioSource.clip = basicZombieAudioClip2;}
        if(r == 3) {basicZombieAudioSource.clip = basicZombieAudioClip3;}
        if(r == 4) {basicZombieAudioSource.clip = basicZombieAudioClip4;}
        if(r == 5) {basicZombieAudioSource.clip = basicZombieAudioClip5;}
        if(r == 6) {basicZombieAudioSource.clip = basicZombieAudioClip6;}

        if (basicZombieCount <= 3)
        {
            basicZombieAudioSource.Play();
            yield return new WaitForSeconds(Random.Range(6, 10));
        }
        else if (basicZombieCount > 3  && basicZombieCount < 6)
        {
            basicZombieAudioSource.Play();
            yield return new WaitForSeconds(Random.Range(4, 6));
        }
        else
        {
            basicZombieAudioSource.Play();
            yield return new WaitForSeconds(Random.Range(1, 3));
        }

        basicZombieBool = true;
    }

    IEnumerator SpecialZombieSound(AudioSource specialZombieAudioSource, string zombieString, AudioClip specialZombieAudioClip1, AudioClip specialZombieAudioClip2, AudioClip specialZombieAudioClip3)
    {
        int currentSpecialZombieAudio = 0;
        int rn = 0;
        rn = Random.Range(1, 4);

        if (zombieString == "bomber") {currentSpecialZombieAudio = currentBomberZombieAudio;}
        if (zombieString == "acidic") {currentSpecialZombieAudio = currentAcidicZombieAudio;}
        if (zombieString == "grotesque") {currentSpecialZombieAudio = currentGrotesqueZombieAudio;}
        if (zombieString == "tank") {currentSpecialZombieAudio = currentTankZombieAudio;}
        if (zombieString == "digger") {currentSpecialZombieAudio = currentDiggerZombieAudio;}
        if (zombieString == "smoker") {currentSpecialZombieAudio = currentSmokerZombieAudio;}
        if (zombieString == "dog") {currentSpecialZombieAudio = currentDogZombieAudio;}
        if (zombieString == "crawler") {currentSpecialZombieAudio = currentCrawlerZombieAudio;}

        while(currentSpecialZombieAudio == rn){rn = Random.Range(1, 4);}

        if (zombieString == "bomber") {currentBomberZombieAudio = rn;}
        if (zombieString == "acidic") {currentAcidicZombieAudio = rn;}
        if (zombieString == "grotesque") {currentGrotesqueZombieAudio = rn;}
        if (zombieString == "tank") {currentTankZombieAudio = rn;}
        if (zombieString == "digger") {currentDiggerZombieAudio = rn;}
        if (zombieString == "smoker") {currentSmokerZombieAudio = rn;}
        if (zombieString == "dog") {currentDogZombieAudio = rn;}
        if (zombieString == "crawler") {currentCrawlerZombieAudio = rn;}

        Debug.Log(zombieString + "zombie sound: rn = " + rn);
        if(rn == 1) {specialZombieAudioSource.clip = specialZombieAudioClip1;}
        if(rn == 2) {specialZombieAudioSource.clip = specialZombieAudioClip2;}
        if(rn == 3) {specialZombieAudioSource.clip = specialZombieAudioClip3;}

        specialZombieAudioSource.Play();
        yield return new WaitForSeconds(Random.Range(3, 4) + specialZombieAudioSource.clip.length);

        if (zombieString == "bomber") {bomberZombieBool = true;}
        if (zombieString == "acidic") {acidicZombieBool = true;}
        if (zombieString == "grotesque") {grotesqueZombieBool = true;}
        if (zombieString == "tank") {tankZombieBool = true;}
        if (zombieString == "digger") {diggerZombieBool = true;}
        if (zombieString == "smoker") {smokerZombieBool = true;}
        if (zombieString == "dog") {dogZombieBool = true;}
        if (zombieString == "crawler") {crawlerZombieBool = true;}
    }

    IEnumerator DogZombieBarkSound()
    {
        Debug.Log("bark");
        dogZombieAudioSource.clip = dogZombieBarkAudioClip;
        dogZombieAudioSource.Play();
        yield return new WaitForSeconds(Random.Range(4, 8));
        dogZombieBool = true;
    }
}

