using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDAmmoDisplayScript : MonoBehaviour
{
    public TextMeshProUGUI HUDAmmoPrefab;
    private GameObject player;

    void Update()
    {
        player = GameObject.Find("Player");
        HUDAmmoPrefab.text = player.GetComponent<PlayerScript>().playerAmmo.ToString() + "/" + player.GetComponent<PlayerScript>().totalPlayerMagazineAmmo.ToString()
            + "/" + player.GetComponent<PlayerScript>().totalPlayerAmmo.ToString();
    }
}
