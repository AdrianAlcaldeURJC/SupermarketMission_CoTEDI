using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroceryListHighlight : MonoBehaviour
{
    [SerializeField] private List<GameObject> highlights;
    [SerializeField] private float transparency;
    [SerializeField] private GameObject groceryListCanvas;

    public void HighlightSection(Food.Category section, bool resetHighlight = false)
    {
        int index = (int)section;
        if (section == Food.Category.fruit)
            index = (int)Food.Category.bakery;
        else if (section == Food.Category.bakery)
            index = (int)Food.Category.fruit;

        if (index > 5 || resetHighlight)
        {
            ResetHighlights();
        }
        else
        {
            Highlight(index);
        }
    }
    
    private void Highlight(int indexSection)
    {
        for (int i = 0; i < highlights.Count; ++i)
        {
            Image[] images =  highlights[i].GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = i == indexSection ? 1f : transparency;
                image.color = color;
            }
        }
    }

    private void ResetHighlights()
    {
        for (int i = 0; i < highlights.Count; i = i+2)
        {
            Image[] images =  highlights[i].GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
        }
    }
}
