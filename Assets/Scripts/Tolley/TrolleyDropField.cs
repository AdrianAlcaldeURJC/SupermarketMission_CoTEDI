using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrolleyDropField : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private int indexI, indexJ;

    public bool isOccupied = false;
    public DragAndDrop element;

    private TrolleyDragAndDropManager dndManager;

    [SerializeField] private GameObject[] columnDropFields = new GameObject[3];

    void Start()
    {
        dndManager = FindObjectOfType<TrolleyDragAndDropManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");
        if (eventData.pointerDrag != null)
        {
            if (this.transform.childCount == 0 && dndManager.trolley[indexJ,indexI] == null)
            {
                if (eventData.pointerDrag.GetComponent<TrolleyDragAndDrop>())
                {
                    Debug.Log("Mi pos: " + this.transform.position);

                    PositionElement(indexJ, indexI, eventData.pointerDrag.gameObject);
                }

            }
            else
            {
                //Mandarlo de vuelta en la pos ini
                eventData.pointerDrag.GetComponent<TrolleyDragAndDrop>().SendBackToIni();
            }
        }
    }

    void PositionElement(int indexJ, int indexI, GameObject element)
    {
        Debug.Log("Indices: " + indexJ + ", "+ indexI);

        if (indexI < 2)
        {
            
            if (dndManager.trolley[indexJ, indexI + 1])
            {
                Debug.Log("El de abajo ocupado" + element.name);
                //Con el nuevo padre
                element.GetComponent<RectTransform>().position = columnDropFields[indexI].GetComponent<RectTransform>().position;
                element.transform.SetParent(columnDropFields[indexI].transform, true);
                //
                dndManager.trolley[indexJ, indexI] = element.gameObject;
                dndManager.evaluateColumn(indexJ);
            }
            else
            {
                PositionElement(indexJ, indexI + 1, element);
            }
        }
        else
        {
            element.GetComponent<RectTransform>().position = columnDropFields[indexI].GetComponent<RectTransform>().position;
            element.transform.SetParent(columnDropFields[indexI].transform, true) ;
            dndManager.trolley[indexJ, indexI] = element.gameObject;
            dndManager.evaluateColumn(indexJ);
        }
    }

    public void RelocateColumnElements(int indexI)
    {
        if (indexI > 0)
        {
            Debug.Log("Quizas Colocando uno");
            //Ver si en la posicion superior a este hay un elemtno
            if (dndManager.trolley[indexJ, indexI - 1])
            {
                Debug.Log("Colocando uno");
                //Si lo hay, colocarlo en la actual
                dndManager.trolley[indexJ, indexI - 1] = null;
                dndManager.trolley[indexJ, indexI] = null;
                dndManager.trolley[indexJ, indexI] = columnDropFields[indexI-1].transform.GetChild(0).gameObject;

                columnDropFields[indexI - 1].transform.GetChild(0).GetComponent<RectTransform>().position = columnDropFields[indexI].GetComponent<RectTransform>().position;
                columnDropFields[indexI - 1].transform.GetChild(0).SetParent(columnDropFields[indexI].transform,true);
                
                //Y llamar a recursividad con la pos de arriba
                RelocateColumnElements(indexI-1);
            }
            else
            {
                //Si no lo hay, acabar
                dndManager.evaluateColumn(indexJ);
            }
        }
        else
        {
            dndManager.evaluateColumn(indexJ);
        }
    }

    public int[] GetIndexes()
    {
        return new int[] {indexI, indexJ};
    }
}
