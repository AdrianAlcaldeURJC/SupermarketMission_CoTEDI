using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyListener : MonoBehaviour
{
    private struct TrolleyResult
    {
        public int Item;
        public int Status;

        override readonly public string ToString()
        {
            return $"({Item}, {Status})";
        }
    }

    [SerializeField] TimerAux timerAux;
    int timerIndex;
    int minigameIndex;
    const int TrolleySizeX = 8; // Columns
    const int TrolleySizeY = 3; // Rows
    const int TrolleySize = TrolleySizeY * TrolleySizeX; 
    List<TrolleyResult> trolleyResults = new List<TrolleyResult>();

    private void Awake()
    {
        timerIndex = timerAux.InitTimer();
        timerAux.StartTimer(timerIndex);
        InitTrolleyResult();
    }

    private void OnDestroy()
    {
        UpdateTrolleyResult();

        minigameIndex = (int)GameManager.GetInstance().CurrentMinigame;
        DataStorage.Instance.minigamesData[minigameIndex].TrolleyResult = ListToString(trolleyResults);
    }

    private void InitTrolleyResult()
    {
        for (int i = 0; i < TrolleySize; ++i)
        {
            trolleyResults.Add(new TrolleyResult()
            {
                Item = -1, // Default value for item
                Status = -1 // Default value for status
            });
        }
    }

    private void SaveTrolleyResult(int col, int row, int item, int status)
    {
        int index = row + col * TrolleySizeY;
        TrolleyResult result = trolleyResults[index];
        result.Item = item;
        result.Status = status;
        trolleyResults[index] = result;
    }

    private void UpdateTrolleyResult()
    {
        Food[,] trolley = GameManager.GetInstance().trolleyStatus;
        
        for (int col = 0; col < TrolleySizeX; ++col)
        {
            for (int row = 0; row < TrolleySizeY; ++row)
            {
                if (trolley[col, row] != null)
                {
                    int Item = DataStorage.GroceryMapData.GetIDfromStringFood(trolley[col, row].foodName);
                    int Status = (int)trolley[col, row].trolleyStatus;
                    SaveTrolleyResult(col, row, Item, Status);
                } 
            }
        }
    }

    public float GetElapsedTime()
    {
        return timerAux.elapsedTime[timerIndex];
    }

    public TimerAux GetTimerAux()
    {
        return timerAux;
    }

    private string ListToString<T>(List<T> ts)
    {
        List<string> list = new List<string>();
        foreach (var item in ts)
        {
            list.Add(item.ToString());
        }

        return "[" + string.Join(", ", list) + "]";
    }

}
