using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{
    private Quaternion initialRotation;

    // Reference the Gaz GameObject in the Inspector
    //private GameObject gazObject;
    //public Sprite gazSprite;
    //private GameObject canvas;
    //Transform gaz;

    public GazScript gazScript;

    void Start()
    {        
        GameObject canvas = GameObject.Find("Canvas");
        Transform gaz = canvas.transform.Find("Gaz");

        GameObject gazObject = gaz.gameObject;
        gazScript = gazObject.GetComponent<GazScript>();

        // Store the initial rotation of the child object
        initialRotation = transform.rotation;
        StartCoroutine(FlipSmoke());
    }

    void Update()
    {
        // Reset the rotation of the child object to its initial rotation
        transform.rotation = initialRotation;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //if (other.gameObject.name == "Player" && gazObject != null && !gazObject.activeSelf)
        if (other.gameObject.name == "Player" && gazScript.getGazActivation() == false)
        {
            //gazObject.SetActive(true);
            //gazObject.GetComponent<SpriteRenderer>().sprite = gazSprite;
            gazScript.setGazActivation(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.name == "Player" && gazObject != null && gazObject.activeSelf)
        if (other.gameObject.name == "Player" && gazScript.getGazActivation() == true)
        {
            //gazObject.SetActive(false);
            //gazObject.GetComponent<SpriteRenderer>().sprite = null;
            gazScript.setGazActivation(false);
        }
    }

    IEnumerator FlipSmoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            var transform = GetComponent<Transform>();
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(0.7f);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }
}