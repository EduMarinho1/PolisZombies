using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseSoundAIScan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Scan());
    }

    public IEnumerator Scan(){
        yield return new WaitForSeconds(0.2f);
        AstarPath.active.Scan();
    }
}
