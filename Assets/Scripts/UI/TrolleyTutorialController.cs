using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrolleyTutorialController : MonoBehaviour
{
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private List<Toggle> pageIndicator;
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private int maxPages = 3;
    private int currentPage;

    void Awake()
    {
        currentPage = 0;
        leftArrow.interactable = false;
        pageIndicator[currentPage].isOn = true;
    }

    public void OnLeftArrow()
    {
        pages[currentPage].SetActive(false);
        pageIndicator[currentPage].isOn = false;
        currentPage = Mathf.Max(currentPage - 1, 0);
        pages[currentPage].SetActive(true);
        pageIndicator[currentPage].isOn = true;
 
        if (currentPage == 0)
            leftArrow.interactable = false;

        rightArrow.interactable = true;
    }

    public void OnRightArrow()
    {
        pages[currentPage].SetActive(false);
        pageIndicator[currentPage].isOn = false;
        currentPage = Mathf.Min(currentPage + 1, maxPages - 1);
        pages[currentPage].SetActive(true);
        pageIndicator[currentPage].isOn = true;

        if (currentPage == maxPages - 1)
            rightArrow.interactable = false;

        leftArrow.interactable = true;
    }

}
