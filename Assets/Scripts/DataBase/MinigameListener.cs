using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameListener : MonoBehaviour
{
    public struct ColorsOpened
    {
        public int Color;
        public int Order;
        public int IsCorrect;
        public float OpenTime;
        public float NumPicks;
        public float NumPossiblePicks;

        override readonly public string ToString()
        {
            return $"({Color}, {Order}, {IsCorrect}, {OpenTime}, {NumPicks}, {NumPossiblePicks})";
        }
    }

    public struct ColorsPicks
    {
        public int Color;
        public int Order;
        public int IsCorrect;
        public float PickTime;
        public int Picked;

        override readonly public string ToString()
        {
            return $"({Color}, {Order}, {IsCorrect}, {PickTime}, {Picked})";
        }
    }

    public struct ShadowSkipped
    {
        public int Item;
        public int IsCorrect;
        public float Time;

        override readonly public string ToString()
        {
            return $"({Item}, {IsCorrect}, {Time})";
        }
    }

    public struct ShadowPicks
    {
        public int Item;
        public int IsCorrect;
        public float Time;
        public int IsCorrectMoment;

        override readonly public string ToString()
        {
            return $"({Item}, {IsCorrect}, {Time}, {IsCorrectMoment})";
        }

    }

    public struct ListOpened
    {
        public int Order;
        public float OpenTime;
        public float CloseTime;

        public override readonly string ToString()
        {
            return $"({Order}, {OpenTime}, {CloseTime})";
        }
    }

    [SerializeField] TimerAux timerAux;
    int timerIndex;
    int minigameIndex;

    private List<ColorsOpened> colorsOpenedList = new List<ColorsOpened>();
    private List<ColorsPicks> colorsPicksList = new List<ColorsPicks>();
    private List<ShadowSkipped> shadowSkippedList = new List<ShadowSkipped>();
    private List<ShadowPicks> shadowPicksList = new List<ShadowPicks>();
    private List<ListOpened> listOpenedList = new List<ListOpened>();

    private void Awake()
    {
        timerIndex = timerAux.InitTimer();
        timerAux.StartTimer(timerIndex);
    }

    private void OnDestroy()
    {
        minigameIndex = (int)GameManager.GetInstance().CurrentMinigame;

        if (minigameIndex < 0 || minigameIndex >= 6)
            return;
            
        DataStorage.Instance.minigamesData[minigameIndex].MinigameDuration = timerAux.elapsedTime[timerIndex];
        DataStorage.Instance.minigamesData[minigameIndex].Minigame = (int)GameManager.GetInstance().CurrentMinigame;
        DataStorage.Instance.minigamesData[minigameIndex].ColorsOpened = ListToString(colorsOpenedList);
        DataStorage.Instance.minigamesData[minigameIndex].ColorsPicks = ListToString(colorsPicksList);
        DataStorage.Instance.minigamesData[minigameIndex].ShadowSkipped = ListToString(shadowSkippedList);
        DataStorage.Instance.minigamesData[minigameIndex].ShadowPicks = ListToString(shadowPicksList);
        DataStorage.Instance.minigamesData[minigameIndex].ListOpened = ListToString(listOpenedList);
        DataStorage.Instance.minigamesData[minigameIndex].AddedItems = ListToString(GameManager.GetInstance().PickedItemsToIntList());
    }

    public float GetElapsedTime()
    {
        return timerAux.elapsedTime[timerIndex];
    }

    public void AddColorOpened(int color, int order, int isCorrect, float openTime, float numPicks, float numPossiblePicks)
    {
        ColorsOpened newColor = new ColorsOpened
        {
            Color = color,
            Order = order,
            IsCorrect = isCorrect,
            OpenTime = openTime,
            NumPicks = numPicks,
            NumPossiblePicks = numPossiblePicks
        };
        colorsOpenedList.Add(newColor);
    }

    public int GetColorOpenedIndex()
    {
        return colorsOpenedList.Count;
    }

    public void AddColorPick(int color, int order, int isCorrect, float pickTime, int picked)
    {
        ColorsPicks newColor = new ColorsPicks
        {
            Color = color,
            Order = order,
            IsCorrect = isCorrect,
            PickTime = pickTime,
            Picked = picked
        };
        colorsPicksList.Add(newColor);
    }

    public int GetColorPickedIndex()
    {
        return colorsPicksList.Count;
    }

    public void AddShadowSkipped(int item, int isCorrect, float time)
    {
        ShadowSkipped newShadow = new ShadowSkipped
        {
            Item = item,
            IsCorrect = isCorrect,
            Time = time
        };
        shadowSkippedList.Add(newShadow);
    }

    public void AddShadowPick(int item, int isCorrect, float time, int isCorrectMoment)
    {
        ShadowPicks newShadow = new ShadowPicks
        {
            Item = item,
            IsCorrect = isCorrect,
            Time = time,
            IsCorrectMoment = isCorrectMoment
        };
        shadowPicksList.Add(newShadow);
    }

    public void AddListOpened(int order, float openTime, float closeTime)
    {
        ListOpened newList = new ListOpened
        {
            Order = order,
            OpenTime = openTime,
            CloseTime = closeTime
        };
        listOpenedList.Add(newList);
    }

    public int GetListOpenedIndex()
    {
        return listOpenedList.Count;
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
