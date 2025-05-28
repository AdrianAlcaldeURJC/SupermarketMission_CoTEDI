using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class MapListener : MonoBehaviour
{
    [SerializeField] public TimerAux timerAux;
    ClickAux clickAux;
    int timerIndex;
    public List<string> dragMapList;
    public List<int> mapLayout;

    public static MapListener Instance { get; private set; }

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

        dragMapList = new List<string>();
        mapLayout = new List<int>();

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

        DataStorage.Instance.groceryMapData.MapDuration = timerAux.elapsedTime[timerIndex].ToString();
        DataStorage.Instance.groceryMapData.MapClicks = clickAux.GetClickInfo();
        DataStorage.Instance.groceryMapData.MapDrops = ListToString(dragMapList);
        DataStorage.Instance.groceryMapData.MapOrder = ListToString(mapLayout);
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

    private string ListToString(List<int> list)
    {
        return "[" + string.Join(", ", list) + "]";
    }

}
