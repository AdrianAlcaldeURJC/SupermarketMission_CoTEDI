using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject popUpPrefab;

    public void SpawnPopUp()
    {
        GameObject spawned = Instantiate(popUpPrefab);
        spawned.transform.SetParent(gameObject.transform);
        spawned.transform.position = gameObject.transform.position;
    }

    public void SetSpawnPosition(Vector3 newGlobalPosition)
    {
        gameObject.transform.position = newGlobalPosition;
    }
}
