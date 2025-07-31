using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector3 iniPos;

    [SerializeField]
    private Transform initialParent;
    
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Food.Category value;

    PointerEventData eData;

    // Drag item to be sent to database
    private Drag dragItem;
    private int timerIndex;
    private bool isCorrectDrop = false;

    // Original size data
    [SerializeField] private Vector2Int rectTransformOriginalSize = new Vector2Int(180, 50);

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iniPos = transform.position;
        initialParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(initialParent);

        // Drag data
        dragItem = new Drag();
        dragItem.NumItem = (int) value;
        dragItem.TakenTime = MapListener.Instance.GetElapsedTime().ToString();
        timerIndex = MapListener.Instance.timerAux.InitTimer();
        MapListener.Instance.timerAux.StartTimer(timerIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isCorrectDrop = false;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            //For the object to come back if it's drag outside the screen
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransformOriginalSize.x);
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransformOriginalSize.y);
            transform.SetParent(initialParent);
            transform.position = iniPos;
        }
        else
        {
            if (eventData.pointerEnter.transform.GetComponent<DropField>() == null)
            {
                transform.SetParent(initialParent);
                transform.position = iniPos;
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransformOriginalSize.x);
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransformOriginalSize.y);
            }
            else
            {
                transform.SetParent(eventData.pointerEnter.gameObject.transform);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                isCorrectDrop = true;
            }
        }

        
        // Save drag data
        dragItem.TakenDuration = MapListener.Instance.timerAux.elapsedTime[timerIndex].ToString();
        dragItem.IsDropCorrect = isCorrectDrop ? 1 : 0;
        MapListener.Instance.dragMapList.Add(dragItem.ToString());
    }

    public Food.Category getValue()
    {
        return this.value;
    }

    public void SendBackToIni()
    {
        transform.position = iniPos;

    }
}
