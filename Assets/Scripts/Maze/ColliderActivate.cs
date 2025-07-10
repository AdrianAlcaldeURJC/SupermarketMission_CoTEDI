using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActivate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    
}
