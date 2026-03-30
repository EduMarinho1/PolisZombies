using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletScript : MonoBehaviour
{
    public GameObject bloodStainZombieHitted;






                                    //BETTER WITHOUT HITTING WALL SOUND
    /*
    [SerializeField]protected AudioClip hitWallSound;

            if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "ScrapWall")
        {
                // Create a new GameObject for the AudioSource
                GameObject audioObject = new GameObject("AudioObject");
                AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
                newAudioSource.clip = hitWallSound;
                newAudioSource.Play();
                Destroy(audioObject, newAudioSource.clip.length);
        }
    */





}
