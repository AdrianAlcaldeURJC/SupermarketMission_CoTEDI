using System.Collections;
using System.Collections.Generic;
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
            transform.position = iniPos;
            transform.parent = initialParent;
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<DropField>() == null)
            {
                transform.position = iniPos;
                transform.parent = initialParent;
            }
            else {

                    transform.parent = eventData.pointerEnter.gameObject.transform;

            }
        }

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
