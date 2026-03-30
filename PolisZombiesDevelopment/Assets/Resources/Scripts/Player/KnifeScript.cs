using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public Sprite knifeSprite;
    public Sprite goldenKnifeSprite;
    public float timeBetweenKnifes = 1f;
    [HideInInspector] public float timeSinceLastKnife = 0;
    public int knifeDamage;
    [HideInInspector] public GameObject player;

    void Start()
    {
        knifeDamage = 40;
        GetComponent<SpriteRenderer>().sprite = knifeSprite;
        timeSinceLastKnife = timeBetweenKnifes;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        timeSinceLastKnife = timeSinceLastKnife + Time.deltaTime;
        if(collision.gameObject.TryGetComponent<ZombieScript>(out ZombieScript enemyComponent) && Input.GetKey(KeyCode.E) && timeSinceLastKnife >= timeBetweenKnifes)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled=true;
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerScript>().AddMoney(8);
            Debug.Log(player.GetComponent<PlayerScript>().money + " money");
            timeSinceLastKnife = 0;
            StartCoroutine(WaitABit(enemyComponent));
        }
    }

    IEnumerator WaitABit(ZombieScript enemyComponent) {
        enemyComponent.TakeDamage(knifeDamage);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().enabled=false;
        yield return new WaitForSeconds(timeBetweenKnifes - 0.3f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void UpgradeToGoldenKnife()
    {
        GetComponent<SpriteRenderer>().sprite = goldenKnifeSprite;
        knifeDamage = knifeDamage + 460;
        Debug.Log("knife damage = " + knifeDamage);
    }
}
