using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStainZombieHittedScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("blood instantiated");
        StartCoroutine(TimeToDestroyBlood());
    }

    IEnumerator TimeToDestroyBlood()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
