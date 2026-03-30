using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDWaveDisplayScript : MonoBehaviour
{
    //public TextMeshProUGUI HUDMoneyPrefab;
    public TextMeshProUGUI HUDWaveDisplayPrefab;

    void Start()
    {
        HUDWaveDisplayPrefab.text = "Wave: " + 1;
    }
    public void SetWaveDisplay(int currentWave)
    {
        //HUDMoneyPrefab.text = "Gold: " + player.GetComponent<PlayerScript>().money.ToString();
        HUDWaveDisplayPrefab.text = "Wave: " + currentWave;
    }
}
