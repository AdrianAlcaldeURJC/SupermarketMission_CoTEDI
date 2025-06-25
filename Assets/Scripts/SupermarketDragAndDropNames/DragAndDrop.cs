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
        //Debug.Log("BeginDrag");
        canvasGroup.blocksRaycasts = false;
        transform.parent = initialParent;

        // Drag data
        dragItem = new Drag();
        dragItem.NumItem = (int) value;
        dragItem.TakenTime = MapListener.Instance.GetElapsedTime().ToString();
        timerIndex = MapListener.Instance.timerAux.InitTimer();
        MapListener.Instance.timerAux.StartTimer(timerIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isCorrectDrop = false;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            //For the object to come back if it's drag outside the screen
            transform.parent = initialParent;
            transform.position = iniPos;
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<DropField>() == null)
            {
                transform.parent = initialParent;
                transform.position = iniPos;
            }
            else
            {

                transform.parent = eventData.pointerEnter.gameObject.transform;
                //transform.position = new Vector3(0.0f, 0.0f, 1.0f);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Debug.Log("first");
                isCorrectDrop = true;

            }
        }

        
        // Save drag data
        dragItem.TakenDuration = MapListener.Instance.timerAux.elapsedTime[timerIndex].ToString();
        dragItem.IsDropCorrect = isCorrectDrop ? 1 : 0;
        MapListener.Instance.dragMapList.Add(dragItem.ToString());
        Debug.Log("Timer index: " + timerIndex);
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
