using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectorListener : MonoBehaviour
{
    public TimerAux timerAux;
    int timerIndex;

    private void Awake()
    {
        timerIndex = timerAux.InitTimer();
        timerAux.StartTimer(timerIndex);
    }

    private void UpdateDecisionTime(int nextMinigame)
    {
        string mapsDecision = DataStorage.Instance.groceryMapData.DecisionTime;
        string duration = timerAux.elapsedTime[timerIndex].ToString();

        if (mapsDecision == null)
        {
            mapsDecision = "";
        }
        mapsDecision += duration;

        if (nextMinigame == 6)
        {
            mapsDecision = $"[{mapsDecision}]";
        }
        else
        {
            mapsDecision += ", ";
        }

        DataStorage.Instance.groceryMapData.DecisionTime = mapsDecision;
    }

    public void AddPickedMap(int mapIndex)
    {
        UpdateDecisionTime(mapIndex);

        // Get picked maps string
        string mapsPicked = DataStorage.Instance.groceryMapData.MapPickOrder;

        if (mapsPicked == null)
        {
            mapsPicked = "";
        }

        mapsPicked += mapIndex.ToString();

        if (mapIndex == 6)
        {
            mapsPicked = $"[{mapsPicked}]";
        }
        else
        {
            mapsPicked += ", ";
        }

        DataStorage.Instance.groceryMapData.MapPickOrder = mapsPicked;
        
    }


}