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

        //ApplyCurrentFill();


        float normalizedValue = progressBar.normalizedValue;
        progressBar.value = current;

/*         float fillWidth = fillArea.rect.width;
                Vector2 newPos = cartImage.anchoredPosition;
                newPos.x = fillWidth * normalizedValue + fillAmount * fillWidth;
                Debug.Log(newPos);
                cartImage.anchoredPosition = new Vector2(newPos.x, cartImage.anchoredPosition.y);  */
    }

    void ApplyCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;
    }
}
