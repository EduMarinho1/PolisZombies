using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSecondWeaponDisplayScript : MonoBehaviour
{
    public void changeSecondWeaponDisplay(Sprite secondWeaponSprite)
    {
        this.gameObject.GetComponent<Image>().sprite = secondWeaponSprite;
    }
}
