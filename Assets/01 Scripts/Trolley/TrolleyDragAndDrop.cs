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

    // Drag data
    private TrolleyListener trolleyListener;
    private int timerIndex;
    private TrolleyListener.TrolleyDrop trolleyDrop;
    [SerializeField]
    public Image statusImage;

    void Start()
    {
        dndManager = FindObjectOfType<TrolleyDragAndDropManager>();
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iniPos = transform.localPosition;
        initialParent = GameObject.Find("NewElementsPanel").transform;
        scrollIndex = transform.GetSiblingIndex();

        // Initialize drag data
        trolleyListener = FindObjectOfType<TrolleyListener>();
        timerIndex = trolleyListener.GetTimerAux().InitTimer();
        
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

        // Save drag data
        trolleyDrop = new TrolleyListener.TrolleyDrop();
        trolleyListener.GetTimerAux().StartTimer(timerIndex);
        trolleyDrop.TakenTime = trolleyListener.GetElapsedTime();
        trolleyDrop.NumItem = DataStorage.GroceryMapData.GetIDfromStringFood(GetComponent<Food>().foodName);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        int isDropCorrect = 0;
        if (eventData.pointerEnter == null)
        {
            //For the object to come back if it's drag outside the screen
            transform.SetParent(initialParent);
            statusImage.color = new Color(1,1,1, 0.65f);
        }
        else
        {
            if (eventData.pointerEnter.GetComponent<TrolleyDropField>() == null)
            {
                transform.SetParent(initialParent);
                statusImage.color = new Color(1,1,1, 0.65f);
            }
            isDropCorrect = 1;

        }


        // Save drag data
        float takenDuration = trolleyListener.GetTimerAux().elapsedTime[timerIndex];

        List<int> finalPos = GetPosition();
        trolleyDrop.FinalPos = finalPos[0] * 3 + finalPos[1]; // Reads in vertical order
        trolleyDrop.IsDropCorrect = isDropCorrect;
        trolleyDrop.TakenDuration = takenDuration;
        FillColData(finalPos[0], finalPos[1]);
        trolleyListener.AddTrolleyDrop(trolleyDrop);
    }

    public List<int> GetPosition()
    {
        if(this.transform.parent.GetComponentInParent<TrolleyDropField>())
        {
            int[] indexes = this.transform.parent.GetComponentInParent<TrolleyDropField>().GetIndexes();
            return new List<int>() { indexes[0], indexes[1] };
        }
        else
        {
            return new List<int>() { -1, -1 };
        }
    }

    public void FillColData(int row, int col)
    {
        if(row == -1 && col == -1)
        {
            // If the item is not dropped in a valid position, we don't fill the data
            trolleyDrop.ColStatus = "";
            trolleyDrop.ColWeight = "";
            trolleyDrop.ColHardness = "";
            return;
        }
        List<int> status = new List<int>();
        List<int> weight = new List<int>();
        List<int> hardness = new List<int>();

        for (int i = 0; i < 3; ++i)
        {
            if (dndManager.trolley[col, i] != null)
            {
                Food food = dndManager.trolley[col, i].GetComponent<Food>();
                status.Add((int)food.trolleyStatus);
                weight.Add((int)food.weight);
                hardness.Add((int)food.hardness);
            }
            else
            {
                status.Add(-1);
                weight.Add(-1);
                hardness.Add(-1);
            }
        }

        trolleyDrop.ColStatus = trolleyListener.ListToString(status,    "()");
        trolleyDrop.ColWeight = trolleyListener.ListToString(weight,    "()");
        trolleyDrop.ColHardness = trolleyListener.ListToString(hardness,  "()");
    }

    public string GetValue()
    {
        return this.value;
    }
    public void SendBackToIni()
    {
        statusImage.color = new Color(1, 1, 1, 0.65f);
    }
}
