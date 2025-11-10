using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroceryListHighlight : MonoBehaviour
{
    [SerializeField] private List<Image> highlights;
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
        // TODO: Just get all the images of the child and change their alpha
        for (int i = 0; i < highlights.Count; i = i+2)
        {
            Color colorTop = highlights[i].color;
            Color colorBottom = highlights[i+1].color;

            if (i == indexSection * 2)
            {
                colorTop.a = 100;
                colorBottom.a = 100;
            }
            else
            {
                colorTop.a = transparency;
                colorBottom.a = transparency;
            }   
            highlights[i].color = colorTop;
            highlights[i+1].color = colorBottom;
        }
    }

    private void ResetHighlights()
    {
        for (int i = 0; i < highlights.Count; i = i+2)
        {
            Color colorTop = highlights[i].color;
            Color colorBottom = highlights[i+1].color;
            
            colorTop.a = 255;
            colorBottom.a = 255;
            highlights[i].color = colorTop;
            highlights[i+1].color = colorBottom;
        }
    }
}
