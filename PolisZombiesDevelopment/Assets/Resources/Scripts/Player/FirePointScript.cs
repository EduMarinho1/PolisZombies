using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointScript : MonoBehaviour
{
    float randomAngle;
    public GameObject player;
    private float precision;

    public void UpdateRotation()
    {
        precision = player.GetComponent<PlayerScript>().equippedWeaponPrefab.GetComponent<WeaponScript>().precision;
        randomAngle = Random.Range(-precision, precision);
        gameObject.transform.Rotate(0f, 0f, randomAngle, Space.Self);
    }

    public void ResetRotation()
    {
        gameObject.transform.Rotate(0f, 0f, -randomAngle, Space.Self);
    }
}
