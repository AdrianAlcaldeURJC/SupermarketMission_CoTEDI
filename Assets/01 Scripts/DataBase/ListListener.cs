using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListListener : MonoBehaviour
{
    [SerializeField] public TimerAux timerAux;
    ClickAux clickAux;
    int timerIndex;
    public List<String> dragGroceryList;

    public static ListListener Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("TRIED TO CREATE A SINGLETON TWO TIMES");

        }

        dragGroceryList = new List<string>();


    }

    private void Start()
    {
        clickAux = new ClickAux();

        timerIndex = timerAux.InitTimer();
        timerAux.StartTimer(timerIndex);


        // ClickAux
        clickAux.timerAux = timerAux;
        clickAux.timerIndex = timerIndex;
    }
    private void OnDestroy()
    {
        timerAux.StopTimer(timerIndex);

        DataStorage.Instance.groceryMapData.ListDuration = timerAux.elapsedTime[timerIndex].ToString();
        DataStorage.Instance.groceryMapData.GroceryClicks = clickAux.GetClickInfo();
        DataStorage.Instance.groceryMapData.GroceryDrops = ListToString(dragGroceryList);
    }

    private void Update()
    {
        clickAux.SaveClickOrTouchInfo();
    }


    public float GetElapsedTime()
    {
        return timerAux.elapsedTime[timerIndex];
    }

    private string ListToString(List<string> list)
    {
        return "[" + string.Join(", ", list) + "]";

    }

}
