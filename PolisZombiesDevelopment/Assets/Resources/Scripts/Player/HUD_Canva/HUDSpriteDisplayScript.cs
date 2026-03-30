using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSpriteDisplayScript : MonoBehaviour
{

    public void changeSpriteDisplay(Sprite spriteToDisplay)
    {
        this.gameObject.GetComponent<Image>().sprite = spriteToDisplay;
    }
}
