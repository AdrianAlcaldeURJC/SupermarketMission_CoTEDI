using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

/// <summary>
/// Singleton class to store multiple JSON DATA. 
/// This data is gonna be sended to a Database
/// </summary>
public class DataStorage : MonoBehaviour
{
    public static DataStorage Instance { get; private set; }

    // Necesary to transform it into a JSON
    [Serializable] public class UserData
    {
        public int UserID;
        public string Name;
        public string CreationData;
        public int Age;
        public int Gender;
        public string Country;
        public string UserAux1; 
            public string UserAux2;
    }

    [Serializable]
    public class SesionData
    {
        public int SesionID;
        public int NumGames;
        public string SesionStartTime;  
        public string SesionEndTime;
        public string Platform;
        public string SesionAux1;
        public string SesionAux2;
    }


    [Serializable]
    public class GameData
    {
        public string GameStartTime;
        public string GameEndTime;
        public string GameAux1;
        public string GameAux2;
    }


    [Serializable]
    public class GroceryMapData
    {
        public string ListDuration;
        public string MapDuration;
        public string GroceryListItems;
        public string GroceryDrops;
        public string GroceryClicks;
        public string MapOrder;
        public string MapDrops;
        public string MapClicks;
        public string MapPickOrder;
        public string DecisionTime;
        public string GroceryAux1;
        public string GroceryAux2;
        public string MapAux1;
        public string MapAux2;
    }

    [Serializable]
    public class MinigamesData
    {
        public int Minigame;
        public string MinigameDuration;
        public string ColorsOpened;
        public string ColorsPicks;
        public string ShadowSkipped;
        public string ShadowPicks;
        public string ListOpened;
        public string AddedItems;
        public string TrolleyResult;
        public string TrolleyDrops;
        public string ColorsAux1;
        public string ColorsAux2;
        public string ShadowsAux1;
        public string ShadowsAux2;
        public string TrolleyAux1;
        public string TrolleyAux2;
    }

    [Serializable]
    public class TrolleyDodgeData
    {
        public string TrolleyDodgeDuration;
        public string ScenesFinished;
        public string TrolleyDodgeSlides;
        public string TrolleyDodgeAux1;
        public string TrolleyDodgeAux2;
        public string TrolleyDodgeAux3;
    }


    // Wrapper to serialize the minigameData list
    [Serializable]
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

        startClasses();

    }

    private void startClasses()
    {
        userData = new UserData();
        sesionData = new SesionData();
        gameData = new GameData();
        groceryMapData = new GroceryMapData();
        trolleyDodgeData = new TrolleyDodgeData();

        for (int i = 0; i < 7; i++)
        {
            minigamesData.Add(new MinigamesData());
        }
    }


    public string GetUserDataJson()
    {
        string aux = JsonUtility.ToJson(userData, true);
        

        return JsonUtility.ToJson(userData, true);
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
