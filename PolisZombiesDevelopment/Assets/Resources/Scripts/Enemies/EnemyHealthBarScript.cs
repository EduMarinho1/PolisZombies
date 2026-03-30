using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarScript : MonoBehaviour
{
    public Transform healthBar;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        maxHealth = GetComponentInParent<BasicZombieScript>().health;
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentHealth = GetComponentInParent<BasicZombieScript>().health;
        Vector3 scale = healthBar.localScale;
        scale.x = currentHealth / maxHealth;
        healthBar.localScale = scale;
    }
}
