using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMoneyDisplayScript : MonoBehaviour
{
    public TextMeshProUGUI HUDMoneyPrefab;
    private GameObject player;

    void Update()
    {
        player = GameObject.Find("Player");
        HUDMoneyPrefab.text = "Gold: " + player.GetComponent<PlayerScript>().money.ToString();
    }
}
