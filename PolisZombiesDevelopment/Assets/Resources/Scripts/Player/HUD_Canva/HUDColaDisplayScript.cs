using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDColaDisplayScript : MonoBehaviour
{
    public Sprite jugDrinkSprite;
    public Sprite staminaDrinkSprite;
    public Sprite reloadDrinkSprite;
    public Sprite pinaDrinkSprite;
    public Sprite headShotDrinkSprite;
    public Sprite regenDrinkSprite;
    public Sprite deathPerceptionSprite;

    public void changeColaSprite(string cola)
    {
        Debug.Log("called cola display");
        if(cola == "Jug") { this.gameObject.GetComponent<Image>().sprite = jugDrinkSprite; }
        if(cola == "Stamina") { this.gameObject.GetComponent<Image>().sprite = staminaDrinkSprite; }
        if(cola == "Reload") { this.gameObject.GetComponent<Image>().sprite = reloadDrinkSprite; }
        if(cola == "Pina") { this.gameObject.GetComponent<Image>().sprite = pinaDrinkSprite; }
        if(cola == "HeadShot") { this.gameObject.GetComponent<Image>().sprite = headShotDrinkSprite; }
        if(cola == "Regen") { this.gameObject.GetComponent<Image>().sprite = regenDrinkSprite; }
        if(cola == "DeathPerception") { this.gameObject.GetComponent<Image>().sprite = deathPerceptionSprite; }
    }
}
