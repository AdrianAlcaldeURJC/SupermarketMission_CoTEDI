using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseButton : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToSet;

    public void ActiveDeactivateGO()
    {
        Time.timeScale = 1.0f;
        gameObjectToSet.SetActive(!gameObjectToSet.activeSelf);
    } 
}
