using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using Newtonsoft.Json.Linq;

/// <summary>
/// Singleton class to store multiple JSON DATA. 
/// This data is gonna be sended to a Database
/// </summary>
public class DataStorage : MonoBehaviour
{
    public static DataStorage Instance { get; private set; }

    // Necesary to transform it into a JSON
    [System.Serializable]
    public class UserData
    {
        public int      UserID { get; set; }
        public string   Name { get; set; }
        public string   CreationData { get; set; }
        public int      Age { get; set; }
        public int      Gender { get; set; }
        public string   Country { get; set; }
        public string   UserAux1 { get; set; }
        public string   UserAux2 { get; set; }
    }

    [System.Serializable]
    public class SesionData
    {
        public int SesionID { get; set; }
        public int NumGames { get; set; }
        public string SesionStartTime { get; set; }
        public string SesionEndTime { get; set; }
        public string Platform { get; set; }
        public string SesionAux1 { get; set; }
        public string SesionAux2 { get; set; }
    }


    [System.Serializable]
    public class GameData
    {
        public string GameStartTime { get; set; }
        public string GameEndTime { get; set; }
        public string GameAux1 { get; set; }
        public string GameAux2 { get; set; }
    }


    [System.Serializable]
    public class GroceryMapData
    {
        public string ListDuration { get; set; }
        public string MapDuration { get; set; }
        public string GroceryListItems { get; set; }
        public string GroceryDrops { get; set; }
        public string GroceryClicks { get; set; }
        public string MapOrder { get; set; }
        public string MapDrops { get; set; }
        public string MapClicks { get; set; }
        public string MapPickOrder { get; set; }
        public string DecisionTime { get; set; }
        public string GroceryAux1 { get; set; }
        public string GroceryAux2 { get; set; }
        public string MapAux1 { get; set; }
        public string MapAux2 { get; set; }
    }

    [System.Serializable]
    public class MinigamesData
    {
        public int Minigame { get; set; }
        public string MinigameDuration { get; set; }
        public string ColorsOpened { get; set; }
        public string ColorsPicks { get; set; }
        public string ShadowSkipped { get; set; }
        public string ShadowPicks { get; set; }
        public string ListOpened { get; set; }
        public string AddedItems { get; set; }
        public string TrolleyResult { get; set; }
        public string TrolleyDrops { get; set; }
        public string ColorsAux1 { get; set; }
        public string ColorsAux2 { get; set; }
        public string ShadowsAux1 { get; set; }
        public string ShadowsAux2 { get; set; }
        public string TrolleyAux1 { get; set; }
        public string TrolleyAux2 { get; set; }
    }


    [System.Serializable]
    public class TrolleyDodgeData
    {
        public string TrolleyDodgeDuration { get; set; }
        public string ScenesFinished { get; set; }
        public string TrolleyDodgeSlides { get; set; }
        public string TrolleyDodgeAux1 { get; set; }
        public string TrolleyDodgeAux2 { get; set; }
        public string TrolleyDodgeAux3 { get; set; }
    }


    // Wrapper to serialize the minigameData list
    [System.Serializable]
    public class Wrapper<T>
    {
        public List<T> items;
    }


    // Data containers
    public UserData userData;
    public SesionData sesionData;
    public GameData gameData;
    public GroceryMapData groceryMapData;
    public List<MinigamesData> minigamesData;
    public TrolleyDodgeData trolleyDodgeData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("SE HA INTENTADO CREAR DOS VECES UN SINGLETON");
        }

        minigamesData = new List<MinigamesData>(7);
    }

    public string GetUserDataJson()
    {
        return JsonUtility.ToJson(userData);
    }

    public string GetSesionDataJson()
    {
        return JsonUtility.ToJson(sesionData);
    }


    public string GetGameDataJson()
    {
        return JsonUtility.ToJson(gameData);
    }

    public string GetGroceryMapDataJson()
    {
        return JsonUtility.ToJson(groceryMapData);
    }

    public string GetMinigamesDataJson()
    {
        return JsonUtility.ToJson(new Wrapper<MinigamesData> { items = minigamesData});
    }

    public string GetSingleMinigameDataJson(int minigame)
    {
        return JsonUtility.ToJson(minigamesData[minigame]);
    }

    public string GetTrolleyDataJson()
    {
        return JsonUtility.ToJson(trolleyDodgeData);
    }

    public string GetCombinedJsons(int minigame)
    {
        JObject finalJson = new JObject();
        JsonMergeSettings mergeSettings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union };

        finalJson.Merge(JObject.Parse(GetUserDataJson()), mergeSettings);
        finalJson.Merge(JObject.Parse(GetSesionDataJson()), mergeSettings);
        finalJson.Merge(JObject.Parse(GetGameDataJson()), mergeSettings);
        finalJson.Merge(JObject.Parse(GetGroceryMapDataJson()), mergeSettings);
        finalJson.Merge(JObject.Parse(GetSingleMinigameDataJson(minigame)), mergeSettings);
        finalJson.Merge(JObject.Parse(GetTrolleyDataJson()), mergeSettings);


        return finalJson.ToString();
    }

}
