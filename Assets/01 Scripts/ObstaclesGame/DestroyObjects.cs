using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    private ObstaclesGame miniManager;

    private void Start()
    {
        miniManager = FindObjectOfType<ObstaclesGame>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
            miniManager.ObstacleReachedTheEnd();
        Destroy(other.gameObject);
    }
}
