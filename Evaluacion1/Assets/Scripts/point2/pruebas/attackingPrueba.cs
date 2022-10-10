using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackingPrueba : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Attack")
        {
            Destroy(gameObject);
        }
    }
}
