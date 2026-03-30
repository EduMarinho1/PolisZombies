using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealthScript : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 5f;
    public GameObject player;
    public int playerHealth;
    public int playerMaxHealth;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMaxHealth = player.GetComponent<PlayerScript>().maxHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth = player.GetComponent<PlayerScript>().health;
        playerMaxHealth = player.GetComponent<PlayerScript>().maxHealth;
        healthAmount = playerHealth - damage;
        healthBar.fillAmount = healthAmount / playerMaxHealth;
    }

    public void Heal(int healingAmount)
    {
        playerHealth = player.GetComponent<PlayerScript>().health;
        playerMaxHealth = player.GetComponent<PlayerScript>().maxHealth;
        healthAmount = playerHealth + healingAmount;
        healthAmount = Mathf.Clamp(playerHealth, 0, playerMaxHealth);
        healthBar.fillAmount = healthAmount / playerMaxHealth;
    }
}
