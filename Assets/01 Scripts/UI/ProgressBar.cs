using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    [SerializeField] int maximum;
    [SerializeField] private int current;
    [SerializeField] Image mask;
    [SerializeField] ObstaclesGame minigameManager;
    [SerializeField] Slider progressBar;
    [SerializeField] RectTransform fillArea;
    [SerializeField] RectTransform cartImage;

    // TODO: Redo this code
    void Update()
    {
        current = minigameManager.GetNumObstacles();
        float fillAmount = (float)current / (float)maximum;

        float normalizedValue = progressBar.normalizedValue;
        progressBar.value = current;
    }
}
