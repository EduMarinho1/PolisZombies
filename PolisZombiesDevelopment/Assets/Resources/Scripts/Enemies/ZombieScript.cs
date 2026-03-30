using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieScript : MonoBehaviour
{
    public bool tookExpDamage;
    public AudioClip zombieDamagedAudio;
    protected List<AudioSource> audioSources = new List<AudioSource>();
    protected bool tookExplosionDamage = false;
    public abstract void TakeDamage(int damageAmount);
    void Start()
    {
        tookExpDamage = false;
    }
    public void TakeExplosionDamage(int damageAmount, int moneyAmount)
    {
        if(tookExpDamage == false)
        {
            tookExpDamage = true;
            GameObject.Find("Player").GetComponent<PlayerScript>().AddMoney(moneyAmount);
            TakeDamage(damageAmount);
            StartCoroutine(WaitForDamageAgain());
        }
    }

    IEnumerator WaitForDamageAgain()
    {
        yield return new WaitForSeconds(0.05f);
        tookExpDamage = false;
    }
}
