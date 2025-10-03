using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigButton : MonoBehaviour
{
    private GameObject configGO;
    void Start()
    {
        configGO = GameObject.FindWithTag("Config");
    }

    public void OnClick()
    {
        Time.timeScale = 0.0f;
        configGO.transform.GetChild(0).gameObject.SetActive(configGO.activeSelf);
    }
}
