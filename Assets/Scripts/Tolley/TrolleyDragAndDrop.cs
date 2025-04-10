using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrolleyDragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    private TrolleyDragAndDropManager dndManager;


    void Start()
    {
        dndManager = FindObjectOfType<TrolleyDragAndDropManager>();
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iniPos = transform.localPosition;
        initialParent = GameObject.Find("NewElementsPanel").transform;
        scrollIndex = transform.GetSiblingIndex();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        canvasGroup.blocksRaycasts = false;
        //Borrar el objecto de la lista cuando se le esta sacando de un drop field
        if (this.transform.parent.GetComponentInParent<TrolleyDropField>())
        {
            Debug.Log("El padre es un drop field " + this.transform.parent);
            dndManager.trolley[this.transform.parent.GetComponentInParent<TrolleyDropField>().GetIndexes()[1], this.transform.parent.GetComponentInParent<TrolleyDropField>().GetIndexes()[0]] = null;
            this.transform.parent.GetComponentInParent<TrolleyDropField>().RelocateColumnElements(this.transform.parent.GetComponentInParent<TrolleyDropField>().GetIndexes()[0]);
        }

        this.transform.SetParent(upperParent.gameObject.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter == null)
        {
            //For the object to come back if it's drag outside the screen
            transform.SetParent(initialParent);
            this.GetComponent<Image>().color = Color.white;
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<TrolleyDropField>() == null)
            {
                transform.SetParent(initialParent) ;
                    this.GetComponent<Image>().color = Color.white;
            }
        }

    }

    public string getValue()
    {
        return this.value;
    }
    public void SendBackToIni()
    {
        this.GetComponent<Image>().color = Color.white;
    }
}
