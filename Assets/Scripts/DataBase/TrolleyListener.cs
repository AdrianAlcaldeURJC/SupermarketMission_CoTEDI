using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyListener : MonoBehaviour
{
    private struct TrolleyResult
    {
        public int Item;
        public int Status;
        public int Weight;
        public int Hardness;

        override readonly public string ToString()
        {
            return $"({Item}, {Status}, {Weight}, {Hardness})";
        }
    }

    public struct TrolleyDrop
    {
        public int NumItem;
        public float TakenTime;
        public float TakenDuration;
        public int IsDropCorrect;
        public string ColStatus;
        public string ColWeight;
        public string ColHardness;
        public int InitialPos;
        public int FinalPos;

        override readonly public string ToString()
        {
            return $"({NumItem}, {TakenTime}, {TakenDuration}, {IsDropCorrect}, {ColStatus}, {ColWeight}, {ColHardness}, {InitialPos}, {FinalPos})";
        }
    }

    [SerializeField] TimerAux timerAux;
    int timerIndex;
    int minigameIndex;
    const int TrolleySizeX = 8; // Columns
    const int TrolleySizeY = 3; // Rows
    const int TrolleySize = TrolleySizeY * TrolleySizeX; 
    List<TrolleyResult> trolleyResults = new List<TrolleyResult>();
    List<TrolleyDrop> trolleyDrops = new List<TrolleyDrop>();

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
        DataStorage.Instance.minigamesData[minigameIndex].TrolleyDrops = ListToString(trolleyDrops);
    }

    private void InitTrolleyResult()
    {
        for (int i = 0; i < TrolleySize; ++i)
        {
            trolleyResults.Add(new TrolleyResult()
            {
                Item = -1,      // Default value for item
                Status = -1,    // Default value for status
                Weight = -1,    // Default value for weight
                Hardness = -1   // Default value for hardness
            });
        }
    }

    private void SaveTrolleyResult(int col, int row, int item, int status, int weight, int hardness)
    {
        int index = row + col * TrolleySizeY;
        TrolleyResult result = trolleyResults[index];
        result.Item = item;
        result.Status = status;
        result.Weight = weight;
        result.Hardness = hardness; 
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
                    int Weight = (int)trolley[col, row].weight;
                    int Hardness = (int)trolley[col, row].hardness;
                    SaveTrolleyResult(col, row, Item, Status, Weight, Hardness);
                } 
            }
        }
    }

    public void AddTrolleyDrop(int numItem, float takenTime, float takenDuration, int isDropCorrect, string colStatus, string colWeight, string colHardness, int initialPos, int finalPos)
    {
        TrolleyDrop drop = new TrolleyDrop()
        {
            NumItem = numItem,
            TakenTime = takenTime,
            TakenDuration = takenDuration,
            IsDropCorrect = isDropCorrect,
            ColStatus = colStatus,
            ColWeight = colWeight,
            ColHardness = colHardness,
            InitialPos = initialPos,
            FinalPos = finalPos
        };

        trolleyDrops.Add(drop);
    }

    public void AddTrolleyDrop(TrolleyDrop drop)
    {
        trolleyDrops.Add(drop);
    }

    public float GetElapsedTime()
    {
        return timerAux.elapsedTime[timerIndex];
    }

    public TimerAux GetTimerAux()
    {
        return timerAux;
    }

    public string ListToString<T>(List<T> ts, string brackets = "[]")
    {
        List<string> list = new List<string>();
        foreach (var item in ts)
        {
            list.Add(item.ToString());
        }

        return brackets[0] + string.Join(", ", list) + brackets[1];
    }

}
