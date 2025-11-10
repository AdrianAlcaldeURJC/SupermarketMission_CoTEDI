using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject popUpCorrect;
    [SerializeField] GameObject popUpWrong;

    public void SpawnPopUp(bool isCorrect)
    {
        GameObject spawned = isCorrect  ? Instantiate(popUpCorrect)  : Instantiate(popUpWrong);
        spawned.transform.SetParent(gameObject.transform);
        spawned.transform.position = gameObject.transform.position;
    }

    public void SetSpawnPosition(Vector3 newGlobalPosition)
    {
        gameObject.transform.position = newGlobalPosition;
    }
}
