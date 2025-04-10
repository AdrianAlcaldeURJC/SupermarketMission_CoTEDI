using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropFieldGroceryList : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public Food.Category value;
    [SerializeField]
    private int index;

    [SerializeField]
    private Transform panelList;

    public List<GameObject> items = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped "+eventData);
        if(eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Food>())
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
                items.Add(eventData.pointerDrag);
                eventData.pointerDrag.transform.SetParent(panelList);
            }
        }
    }
}
