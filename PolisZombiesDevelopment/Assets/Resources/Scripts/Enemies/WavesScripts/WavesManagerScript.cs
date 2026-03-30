using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManagerScript : MonoBehaviour
{
    // Audios
    public AudioClip beginningOfWaveSound;
    public AudioClip endOfWaveSound;
    public AudioClip dogEntryRoundSound;
    public AudioClip dogEndRoundSound;
    

    // General variables
    public int thisWave; //to serve as reference to other scripts
    public GameObject[] spawners;
    public int timeBetweenSpawns;
    public int totalTimeForSpawning; // total number of zombies in wave = timeBetweenSpawns * totalTimeForSpawning * spawners.Length
    public int largestAmountOfZombiesInScene = 150;
    private int currentAmountOfZombies;
    private GameObject[] zombies;
    public HUDWaveDisplayScript waveDisplayScript;
    public int dogWave;

    // Zombies prefabs
    public GameObject basicZombiePrefab;
    public GameObject coneZombiePrefab;
    public GameObject crawlerZombiePrefab;
    public GameObject bomberZombiePrefab;
    public GameObject dogZombiePrefab;
    public GameObject acidicZombiePrefab;
    public GameObject diggerZombiePrefab;
    public GameObject smokerZombiePrefab;
    public GameObject grotesqueZombiePrefab;
    public GameObject tankZombiePrefab;
    public GameObject ballZombiePrefab;
    public GameObject shockZombiePrefab;
    public GameObject shadowZombiePrefab;

    // Zombies chances of spawning (lowest chance is 0 and highest is 100, sum of all of them = 100)
    public int chanceOfSpawningBasicZombie;
    public int chanceOfSpawningConeZombie;
    public int chanceOfSpawningCrawlerZombie;
    public int chanceOfSpawningBomberZombie;
    public int chanceOfSpawningDogZombie;
    public int chanceOfSpawningAcidicZombie;
    public int chanceOfSpawningDiggerZombie;
    public int chanceOfSpawningSmokerZombie;
    public int chanceOfSpawningGrotesqueZombie;
    public int chanceOfSpawningShadowZombie;

    // Start is called before the first frame update
    void Start()
    {
        int randomDogWave = Random.Range(1, 3);
        
        if(randomDogWave == 1){dogWave = 7;}
        else {dogWave = 12;}

        thisWave = 1;
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        currentAmountOfZombies = zombies.Length;

        StartCoroutine(SpawnerRoutine());
    }

    private void UpdateCurrentAmountOfZombies()
    {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        currentAmountOfZombies = zombies.Length;
    }

    IEnumerator SpawnerRoutine()
    {
        for (int currentWave = 1; currentWave <= 21; currentWave++)
        {
            thisWave = currentWave;
            if(currentWave == dogWave){GetComponent<AudioSource>().clip = dogEntryRoundSound;}
            else{GetComponent<AudioSource>().clip = beginningOfWaveSound;}
            GetComponent<AudioSource>().Play();
            ChangeWave(currentWave);

            yield return new WaitForSeconds(beginningOfWaveSound.length);

            Debug.Log("currentWave = " + currentWave);

            if(currentWave == 5)
            {
                StartCoroutine(SpawnBoss(tankZombiePrefab));
            } else if (currentWave == 10)
            {
                StartCoroutine(SpawnBoss(ballZombiePrefab));
            } else if (currentWave == 15)
            {
                StartCoroutine(SpawnBoss(shockZombiePrefab));
            }

            for (int i = 0; i < Mathf.FloorToInt(totalTimeForSpawning / timeBetweenSpawns); i++) // Throughout the whole wave
            {
                for (int j = 0; j < spawners.Length; j++) // Every spawner spawns once
                {
                    Debug.Log("SpawnerRoutine 2 spawners");
                    GameObject currentSpawner = spawners[j];
                    SpawnerScript spawnerScript = currentSpawner.GetComponent<SpawnerScript>();
                    SpawnZombie(spawnerScript);
                }

                yield return new WaitForSeconds(timeBetweenSpawns);

                UpdateCurrentAmountOfZombies();
                while(currentAmountOfZombies >= largestAmountOfZombiesInScene)
                {
                    Debug.Log("amount of zombies = " + currentAmountOfZombies);
                    yield return new WaitForSeconds(1);
                    UpdateCurrentAmountOfZombies();
                }
            }

            UpdateCurrentAmountOfZombies();
            while (currentAmountOfZombies > 0)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Need to kill all zombies to finish wave");
                UpdateCurrentAmountOfZombies();
            }
            Debug.Log("wave finished");
            if(currentWave == dogWave){GetComponent<AudioSource>().clip = dogEndRoundSound;}
            else{GetComponent<AudioSource>().clip = endOfWaveSound;}
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(endOfWaveSound.length);
        }
    }

    IEnumerator SpawnBoss(GameObject bossZombiePrefab)
    {
        spawners[0].GetComponent<SpawnerScript>().Spawn(bossZombiePrefab);
        yield return new WaitForSeconds(0.1f);
    }

    void SpawnZombie(SpawnerScript spawnerScript)
    {
        int rand = Random.Range(1, 101);

        if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie +
            chanceOfSpawningDogZombie +
            chanceOfSpawningAcidicZombie +
            chanceOfSpawningDiggerZombie +
            chanceOfSpawningSmokerZombie +
            chanceOfSpawningGrotesqueZombie
            && chanceOfSpawningShadowZombie > 0)
            {spawnerScript.Spawn(shadowZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie +
            chanceOfSpawningDogZombie +
            chanceOfSpawningAcidicZombie +
            chanceOfSpawningDiggerZombie +
            chanceOfSpawningSmokerZombie
            && chanceOfSpawningGrotesqueZombie > 0)
            {spawnerScript.Spawn(grotesqueZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie +
            chanceOfSpawningDogZombie +
            chanceOfSpawningAcidicZombie +
            chanceOfSpawningDiggerZombie
            && chanceOfSpawningSmokerZombie > 0)
            {spawnerScript.Spawn(smokerZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie +
            chanceOfSpawningDogZombie +
            chanceOfSpawningAcidicZombie
            && chanceOfSpawningDiggerZombie > 0)
            {spawnerScript.Spawn(diggerZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie +
            chanceOfSpawningDogZombie
            && chanceOfSpawningAcidicZombie > 0)
            {spawnerScript.Spawn(acidicZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie +
            chanceOfSpawningBomberZombie
            && chanceOfSpawningDogZombie > 0)
            {spawnerScript.Spawn(dogZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie +
            chanceOfSpawningCrawlerZombie
            && chanceOfSpawningBomberZombie > 0)
            {spawnerScript.Spawn(bomberZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie +
            chanceOfSpawningConeZombie
            && chanceOfSpawningCrawlerZombie > 0)
            {spawnerScript.Spawn(crawlerZombiePrefab);}

        else if(rand >=
            chanceOfSpawningBasicZombie
            && chanceOfSpawningConeZombie > 0)
            {spawnerScript.Spawn(coneZombiePrefab);}

        else
        {spawnerScript.Spawn(basicZombiePrefab);}
    }

    void ChangeWave(int currentWave)
    {
        waveDisplayScript.SetWaveDisplay(currentWave);

        if(currentWave == 1)
        {
            chanceOfSpawningBasicZombie = 100;
            chanceOfSpawningConeZombie = 0;
            chanceOfSpawningCrawlerZombie = 0;
            chanceOfSpawningBomberZombie = 0;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 15;
            totalTimeForSpawning = 30;
        }

        if(currentWave == 2)
        {
            chanceOfSpawningBasicZombie = 90;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 0;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 14;
            totalTimeForSpawning = 56;
        }

        if(currentWave == 3)
        {
            chanceOfSpawningBasicZombie = 85;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 13;
            totalTimeForSpawning = 65;
        }

        if(currentWave == 4)
        {
            chanceOfSpawningBasicZombie = 80;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 10;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 12;
            totalTimeForSpawning = 72;
        }

        if(currentWave == 5) // Add tank zombie as a separate spawner
        {
            chanceOfSpawningBasicZombie = 80;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 10;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 12;
            totalTimeForSpawning = 72;
        }

        if(currentWave == 6)
        {
            chanceOfSpawningBasicZombie = 80;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 2;
            chanceOfSpawningBomberZombie = 9;
            chanceOfSpawningDogZombie = 7;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 11;
            totalTimeForSpawning = 66;
        }

        if(currentWave == 7 && currentWave != dogWave)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 10;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 10;
            totalTimeForSpawning = 60;
        }

        if(currentWave == 7 && currentWave == dogWave)
        {
            chanceOfSpawningBasicZombie = 0;
            chanceOfSpawningConeZombie = 0;
            chanceOfSpawningCrawlerZombie = 0;
            chanceOfSpawningBomberZombie = 0;
            chanceOfSpawningDogZombie = 100;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 5;
            totalTimeForSpawning = 25;
        }

        if(currentWave == 8)
        {
            chanceOfSpawningBasicZombie = 79;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 3;
            chanceOfSpawningDogZombie = 5;
            chanceOfSpawningAcidicZombie = 3;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 10;
            totalTimeForSpawning = 70;
        }

        if(currentWave == 9)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 5;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 9;
            totalTimeForSpawning = 63;
        }

        if(currentWave == 10)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 5;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 5;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 9;
            totalTimeForSpawning = 63;
        }

        if(currentWave == 11)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 3;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 5;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 8;
            totalTimeForSpawning = 56;
        }

        if(currentWave == 12 && currentWave != dogWave)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 4;
            chanceOfSpawningCrawlerZombie = 3;
            chanceOfSpawningBomberZombie = 5;
            chanceOfSpawningDogZombie = 5;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 3;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 8;
            totalTimeForSpawning = 64;
        }

        if(currentWave == 12 && currentWave == dogWave)
        {
            chanceOfSpawningBasicZombie = 0;
            chanceOfSpawningConeZombie = 0;
            chanceOfSpawningCrawlerZombie = 0;
            chanceOfSpawningBomberZombie = 0;
            chanceOfSpawningDogZombie = 100;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 8;
            totalTimeForSpawning = 40;
        }

        if(currentWave == 13) // could be dogWave
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 3;
            chanceOfSpawningCrawlerZombie = 3;
            chanceOfSpawningBomberZombie = 3;
            chanceOfSpawningDogZombie = 4;
            chanceOfSpawningAcidicZombie = 3;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 7;
            totalTimeForSpawning = 56;
        }

        if(currentWave == 14)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 3;
            chanceOfSpawningCrawlerZombie = 3;
            chanceOfSpawningBomberZombie = 1;
            chanceOfSpawningDogZombie = 4;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 7;
            totalTimeForSpawning = 56;
        }

        if(currentWave == 15)
        {
            chanceOfSpawningBasicZombie = 75;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 2;
            chanceOfSpawningBomberZombie = 1;
            chanceOfSpawningDogZombie = 4;
            chanceOfSpawningAcidicZombie = 6;
            chanceOfSpawningDiggerZombie = 6;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 7;
            totalTimeForSpawning = 56;
        }

        if(currentWave == 16)
        {
            chanceOfSpawningBasicZombie = 73;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 5;
            chanceOfSpawningBomberZombie = 2;
            chanceOfSpawningDogZombie = 3;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 2;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 6;
            totalTimeForSpawning = 48;
        }

        if(currentWave == 17)
        {
            chanceOfSpawningBasicZombie = 72;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 4;
            chanceOfSpawningBomberZombie = 1;
            chanceOfSpawningDogZombie = 3;
            chanceOfSpawningAcidicZombie = 6;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 3;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 5;
            totalTimeForSpawning = 40;
        }

        if(currentWave == 18)
        {
            chanceOfSpawningBasicZombie = 71;
            chanceOfSpawningConeZombie = 2;
            chanceOfSpawningCrawlerZombie = 4;
            chanceOfSpawningBomberZombie = 1;
            chanceOfSpawningDogZombie = 3;
            chanceOfSpawningAcidicZombie = 6;
            chanceOfSpawningDiggerZombie = 5;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 4;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 5;
            totalTimeForSpawning = 40;
        }

        if(currentWave == 19)
        {
            chanceOfSpawningBasicZombie = 70;
            chanceOfSpawningConeZombie = 3;
            chanceOfSpawningCrawlerZombie = 2;
            chanceOfSpawningBomberZombie = 1;
            chanceOfSpawningDogZombie = 4;
            chanceOfSpawningAcidicZombie = 5;
            chanceOfSpawningDiggerZombie = 6;
            chanceOfSpawningSmokerZombie = 4;
            chanceOfSpawningGrotesqueZombie = 5;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 4;
            totalTimeForSpawning = 32;
        }

        if(currentWave == 20)
        {
            chanceOfSpawningBasicZombie = 0;
            chanceOfSpawningConeZombie = 0;
            chanceOfSpawningCrawlerZombie = 0;
            chanceOfSpawningBomberZombie = 0;
            chanceOfSpawningDogZombie = 0;
            chanceOfSpawningAcidicZombie = 0;
            chanceOfSpawningDiggerZombie = 0;
            chanceOfSpawningSmokerZombie = 0;
            chanceOfSpawningGrotesqueZombie = 0;
            chanceOfSpawningShadowZombie = 100;
            timeBetweenSpawns = 15;
            totalTimeForSpawning = 15;
        }

        if(currentWave == 21) // extra round
        {
            chanceOfSpawningBasicZombie = 20;
            chanceOfSpawningConeZombie = 10;
            chanceOfSpawningCrawlerZombie = 10;
            chanceOfSpawningBomberZombie = 10;
            chanceOfSpawningDogZombie = 10;
            chanceOfSpawningAcidicZombie = 10;
            chanceOfSpawningDiggerZombie = 10;
            chanceOfSpawningSmokerZombie = 10;
            chanceOfSpawningGrotesqueZombie = 10;
            chanceOfSpawningShadowZombie = 0;
            timeBetweenSpawns = 1;
            totalTimeForSpawning = 100000;
        }
    }
}
