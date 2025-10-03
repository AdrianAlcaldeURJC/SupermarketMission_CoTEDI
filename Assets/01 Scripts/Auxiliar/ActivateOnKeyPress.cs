using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnKeyPress : MonoBehaviour
{
    [SerializeField] KeyCode key;
    void Update()
    {
            Debug.Log("KeyPressed");

        if (Input.GetKey(key))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
