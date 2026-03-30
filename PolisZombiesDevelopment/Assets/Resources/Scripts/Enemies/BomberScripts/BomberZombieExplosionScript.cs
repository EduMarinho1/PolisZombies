using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberZombieExplosionScript : MonoBehaviour
{
    private bool damaged = false;
    public GameObject bomberAcid;
    [SerializeField] private AudioClip bomberExplosionSound;

    void Start()
    {
        GameObject audioObject = new GameObject("AudioObject");
        AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
        newAudioSource.clip = bomberExplosionSound;; // Directly assign hitWallSound since it's an AudioClip
        newAudioSource.Play();
        Destroy(audioObject, bomberExplosionSound.length);
        StartCoroutine(MakeAcid());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && damaged == false)
        {
            damaged = true;
            other.gameObject.GetComponent<PlayerScript>().TakeExplosionDamage(1);
        }
    }

    private IEnumerator MakeAcid()
    {
        yield return new WaitForSeconds(0.15f);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.15f);
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.15f);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.15f);
        Instantiate(bomberAcid, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
