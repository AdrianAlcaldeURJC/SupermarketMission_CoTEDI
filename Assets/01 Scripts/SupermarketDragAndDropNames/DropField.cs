using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropField : MonoBehaviour, IDropHandler
{
    [SerializeField] private Food.Category value;
    [SerializeField] private int index;
    [SerializeField] private int allowedChildCount = 0;
    [SerializeField] private int modifiedWidth = 180;
    [SerializeField] private int modifiedAngleZ = 0;
    public bool isOccupied = false;
    public DragAndDrop element;

    [SerializeField]
    private DragAndDropManager dndManager;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");
        if (eventData.pointerDrag != null)
        {
            if (this.transform.childCount == allowedChildCount)
            {
                if (eventData.pointerDrag.GetComponent<DragAndDrop>().getValue() == this.value)
                {
                    Debug.Log("Correct");
                }
                else
                {
                    Debug.Log("Bad");
                }

                eventData.pointerDrag.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, modifiedWidth);
                eventData.pointerDrag.GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, modifiedAngleZ));

            }
            else
            {
                //Mandarlo de vuelta en la pos ini
                eventData.pointerDrag.GetComponent<DragAndDrop>().SendBackToIni();
            }
        }
    }

    public Food.Category GetValue()
    {
        return this.value;
    }

    public void SetValue(Food.Category category)
    {
        this.value = category;
    }
}
