using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{
    private Toggle[] toggles;
    // Start is called before the first frame update
    void Start()
    {
        toggles = GetComponentsInChildren<Toggle>();
    }

    public void SelectedObjects()
    {
        foreach (Toggle t in toggles)
        {
        }
    }

    
}
