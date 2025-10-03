using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextPlayerScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameA;
    [SerializeField] private TextMeshProUGUI nameB;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image readyButtonImage;
    [SerializeField] private TextMeshProUGUI textButton;
    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;

    public void Awake()
    {
        NextScene();
    }

    public void NextScene()
    {
        if (GameManager.GetInstance().nextPlayer == 1)
        {
            nameA.gameObject.SetActive(false);
            nameB.gameObject.SetActive(true);
            title.color = colorB;

            nameB.text = GameManager.GetInstance().playerNameB;
            GameManager.GetInstance().nextPlayer = 0;
        }
        else
        {
            nameA.gameObject.SetActive(true);
            nameB.gameObject.SetActive(false);
            title.color = colorA;

            nameA.text = GameManager.GetInstance().playerNameA;
            GameManager.GetInstance().nextPlayer = 1;
        }

    }

}
