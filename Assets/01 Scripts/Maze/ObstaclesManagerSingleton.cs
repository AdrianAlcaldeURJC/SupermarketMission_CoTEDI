using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManagerSingleton : MonoBehaviour
{
    private static ObstaclesManagerSingleton Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
