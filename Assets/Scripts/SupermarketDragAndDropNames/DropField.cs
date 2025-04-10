using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropField : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Food.Category value;
    [SerializeField]
    private int index;

    public bool isOccupied = false;
    public DragAndDrop element;

    [SerializeField]
    private DragAndDropManager dndManager;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");
        if (eventData.pointerDrag != null)
        {
            if (this.transform.childCount == 0)
            {
                if (eventData.pointerDrag.GetComponent<DragAndDrop>().getValue() == this.value)
                {
                    Debug.Log("Correct");
                    //dndManager.SetResult(index, true);
                }
                else
                {
                    Debug.Log("Bad");
                    //dndManager.SetResult(index, false);
                }
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                //Mandarlo de vuelta en la pos ini
                eventData.pointerDrag.GetComponent<DragAndDrop>().SendBackToIni();
            }
        }
    }

    public Food.Category getValue()
    {
        return this.value;
    }

    public void setValue(Food.Category category)
    {
        this.value = category;
    }
}
