using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryListButton : MonoBehaviour
{
    [SerializeField] Canvas groceryListCanvas;

    public void Start()
    {
        groceryListCanvas.gameObject.SetActive(false);
    }
    
    public void OnClickGroceryList()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        groceryListCanvas.gameObject.SetActive(!groceryListCanvas.gameObject.activeSelf);
        groceryListCanvas.gameObject.GetComponent<GroceryListDisplay>().RefreshSection();
    }   
}
