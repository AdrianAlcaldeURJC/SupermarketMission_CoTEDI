using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropGroceryList : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector3 iniPos;
    private int scrollIndex;

    [SerializeField]
    public Canvas upperParent;
    private Canvas targetParent;
    private Transform initialParent;
    
    [SerializeField]
    public Canvas canvas;
    [SerializeField]
    private string value;


    //Scara como hijo del panel para que no le afecte la mascara al dragear
    //Guardar indice del scroll con transform.GetSiblingIndex() & transform.SetSiblingIndex() posiblemente, pero ni idea
    //En caso de soltar fuera de un dropfield, volver a hacer hijo y volver a darle su posicion
    //Si se suelta dentro de un drop field, pasa a colocarse dentro suyo y a guardarse en la list<> del drop

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iniPos = transform.position;
        initialParent = transform.parent;
        scrollIndex = transform.GetSiblingIndex();

        //Debug.Log("Parent: " + initialParent + " Index: " + scrollIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        //Debug.Log("BeginDrag");
        canvasGroup.blocksRaycasts = false;
        //Borrar el objecto de la lista cuando se le esta sacando de un drop field
        if (this.transform.parent.GetComponentInParent<DropFieldGroceryList>())
        {
            Debug.Log("El padre es un drop field "+ this.transform.parent);
            this.transform.parent.GetComponentInParent<DropFieldGroceryList>().items.Remove(this.gameObject);
        }

        this.transform.SetParent(upperParent.gameObject.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Enddrag");
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter==null)
        {
            //For the object to come back if it's drag outside the screen
            //transform.position = iniPos;
            transform.SetParent(initialParent, false);
            this.transform.SetSiblingIndex(scrollIndex);
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<DropFieldGroceryList>() == null)
            {
                if (eventData.pointerEnter.transform.parent.GetComponent<DragAndDropGroceryList>() != null)
                {
                    Debug.Log("Nombre objeto " + eventData.pointerEnter.gameObject.name);
                    if (eventData.pointerEnter.gameObject.GetComponentInParent<DropFieldGroceryList>() != null)
                    {
                        GameObject usefulParent = eventData.pointerEnter.gameObject.GetComponentInParent<DropFieldGroceryList>().gameObject;
                        //Si se suelta encima de un alimento que ya esta asignado, meterlo en su misma asignacion
                    
                        Debug.Log("Nombre objeto hihihi " + usefulParent.gameObject.name);

                        this.GetComponent<RectTransform>().anchoredPosition = usefulParent.GetComponent<RectTransform>().anchoredPosition;
                        //usefulParent.GetComponent<DropFieldGroceryList>().AddItemToList(this.gameObject);
                        this.transform.SetParent(usefulParent.transform.GetChild(0).transform);
                    }
                    else
                    {
                        transform.position = iniPos;
                        transform.SetParent(initialParent);
                        this.transform.SetSiblingIndex(scrollIndex);
                    }
                }
                else {
                    transform.position = iniPos;
                    transform.SetParent(initialParent);
                    this.transform.SetSiblingIndex(scrollIndex);
                }
                
            }
        }

    }

    public string getValue()
    {
        return this.value;
    }
}
