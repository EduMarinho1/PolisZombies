using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 v = new Vector3(target.position.x, target.position.y, -1f);
        transform.position = v;
    }
}
