using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour                                  // TODO: O jogador vence o jogo quando chega no round 20, aí tem um último round extra surpresa.
{
    //private void Start() 
    //{
    //    StartCoroutine(Spawner());
    //}

    //private IEnumerator Spawner()
    //{
    //    WaitForSeconds wait = new WaitForSeconds(spawnRate);
    //
    //        while(canSpawn)
    //        {
    //            yield return wait;
    //            int rand = Random.Range(0, enemyPrefabs.Length);
    //            GameObject enemyToSpawn = enemyPrefabs[rand];
    //
    //            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
    //        }
    //}

    public void Spawn(GameObject zombieToSpawn)
    {
        Instantiate(zombieToSpawn, transform.position, Quaternion.identity);
    }
}
