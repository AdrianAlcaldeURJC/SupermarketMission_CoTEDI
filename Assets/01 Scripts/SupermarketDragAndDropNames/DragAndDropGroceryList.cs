using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

    // Struct to load Drag data. To be sent to database later
    class Drag
    {
        public int NumItem { get; set; }
        public string TakenTime { get; set; }
        public string TakenDuration { get; set; }
        public int IsDropCorrect { get; set; }
        public string Aux1 { get; set; }

        override public string ToString()
        {
            return "(" + NumItem + ", " + TakenTime + ", " + TakenDuration + ", " + IsDropCorrect + ")";
        }
    }

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

    Drag dragItem;
    int timerIndex;

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

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        canvasGroup.blocksRaycasts = false;
        // Borrar el objeto de la lista cuando se le esta sacando de un drop field
        if (this.transform.parent.GetComponentInParent<DropFieldGroceryList>())
        {
            this.transform.parent.GetComponentInParent<DropFieldGroceryList>().items.Remove(this.gameObject);
        }

        this.transform.SetParent(upperParent.gameObject.transform);

        // Save drag data
        dragItem = new Drag();
        dragItem.NumItem = DataStorage.GroceryMapData.GetIDfromStringFood(GetComponent<Food>().foodName);
        dragItem.TakenTime = ListListener.Instance.GetElapsedTime().ToString();
        timerIndex = ListListener.Instance.timerAux.InitTimer();
        ListListener.Instance.timerAux.StartTimer(timerIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isCorrect = false;
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter == null)
        {
            // For the object to come back if it's drag outside the screen
            transform.SetParent(initialParent, false);
            this.transform.SetSiblingIndex(scrollIndex);
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<DropFieldGroceryList>() == null)
            {
                if (eventData.pointerEnter.transform.parent.GetComponent<DragAndDropGroceryList>() != null)
                {
                    if (eventData.pointerEnter.gameObject.GetComponentInParent<DropFieldGroceryList>() != null)
                    {
                        GameObject usefulParent = eventData.pointerEnter.gameObject.GetComponentInParent<DropFieldGroceryList>().gameObject;
                        //Si se suelta encima de un alimento que ya esta asignado, meterlo en su misma asignacion

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
                    isCorrect = true;

                }
                else
                {
                    transform.position = iniPos;
                    transform.SetParent(initialParent);
                    this.transform.SetSiblingIndex(scrollIndex);
                }

            }
        }

        // Save drag data
        dragItem.TakenDuration = ListListener.Instance.timerAux.elapsedTime[timerIndex].ToString();
        ListListener.Instance.timerAux.StopTimer(timerIndex);
        dragItem.IsDropCorrect = isCorrect ? 1 : 0;
        ListListener.Instance.dragGroceryList.Add(dragItem.ToString());
    }

    public string GetValue()
    {
        return this.value;
    }


}
