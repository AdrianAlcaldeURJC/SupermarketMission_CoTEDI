using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Localization.Settings;
using System.Linq;

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
        public string UserID;
        public string Name;
        public string CreationDate;
        public int Age;
        public string Gender;
        public string Country;
        public string UserAux1; 
        public string UserAux2;

        public void setData(string _Name, int _Age, string _Gender,  
                            string _UserAux1 = null, string _UserAux2 = null)
        {
            Name = _Name.Replace(" ", "_").ToLower();
            CreationDate = DateTime.Now.ToString("M/d/yyyy"); // TODO: Check if the user is already created and use that date...
            Age = _Age;
            Gender = _Gender;

            // Creation Country
            Country = LocalizationSettings.SelectedLocale.ToString();

            UserAux1 = _UserAux1;
            UserAux2 = _UserAux2;

            UserID = generateUserID();
        }

        public string generateUserID()
        {
            // Function by https://stackoverflow.com/questions/63615950/generate-unique-id-from-string-in-c-sharp
            string hash;
            using (var hashAlgorithm = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(this.Name));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                hash = sBuilder.ToString();

                Debug.Log($"The SHA256 hash of {Name} is: {hash}.");

            }
            return hash;
        }
    }

    [Serializable]
    public class SesionData
    {
        public int SesionID;
        public int NumGames;
        public string SesionStartTime;  
        public string SesionEndTime;
        public int Platform;
        public string SesionAux1;
        public string SesionAux2;


        public void OnAwakeData()
        {
            SesionStartTime = DateTime.Now.ToString("M/d/yyyy/hh:mm:ss");
            SesionID = 1; // TODO: CORRECT ID OF THE SESION
            Platform = (int)Environment.OSVersion.Platform;
        }

        public void OnDestroyData()
        {
            SesionEndTime = DateTime.Now.ToString("M/d/yyyy/hh:mm:ss");
        }
    }

    [Serializable]
    public class GameData
    {
        public int GameID;
        public string GameStartTime;
        public string GameEndTime;
        public string GameAux1;
        public string GameAux2;
    
        public void OnAwakeData()
        {
            GameStartTime = DateTime.Now.ToString("M/d/yyyy/hh:mm:ss");
        }

        public void OnDestroyData()
        {
            GameEndTime = DateTime.Now.ToString("M/d/yyyy/hh:mm:ss");
        }
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

        public void LoadGroceryListItems(List<Food> foodList)
        {
            string groceryListStr = "[";

            foreach (Food food in foodList)
            {
                groceryListStr += GetIDfromStringFood(food.foodName).ToString() + ",";
            }

            groceryListStr = groceryListStr.Remove(groceryListStr.Length - 1);
            groceryListStr += "]";

            GroceryListItems = groceryListStr;
        }

        public static int GetIDfromStringFood(string foodName)
        {
            switch (foodName)
            {
                case "Bakery-Bread": return 0;
                case "Bakery-Cake": return 1;
                case "Bakery-Croissant": return 2;
                case "Bakery-KingCake": return 3;
                case "Bakery-Muffin": return 4;
                case "Bakery-Pastries": return 5;
                case "Bakery-SlicedBread": return 6;
                case "Bakery-WholewheatBread": return 7;
                case "Fish-Crab": return 8;
                case "Fish-Emperor": return 9;
                case "Fish-Hake": return 10;
                case "Fish-Octopus": return 11;
                case "Fish-Oyster": return 12;
                case "Fish-Prawn": return 13;
                case "Fish-Salmon": return 14;
                case "Fish-Squib": return 15;
                case "Fridge-Butter": return 16;
                case "Fridge-Cheese": return 17;
                case "Fridge-Cream": return 18;
                case "Fridge-Ham": return 19;
                case "Fridge-Milk": return 20;
                case "Fridge-Custard": return 21;
                case "Fridge-Sausages": return 22;
                case "Fridge-Yogurt": return 23;
                case "Fruits-Apple": return 24;
                case "Fruits-Banana": return 25;
                case "Fruits-Coconut": return 26;
                case "Fruits-Grapes": return 27;
                case "Fruits-Kiwi": return 28;
                case "Fruits-Mango": return 29;
                case "Fruits-Melon": return 30;
                case "Fruits-Orange": return 31;
                case "Fruits-Pear": return 32;
                case "Fruits-Pineapple": return 33;
                case "Fruits-Watermelon": return 34;
                case "Legumes-BroadBeans": return 35;
                case "Legumes-BrownBeans": return 36;
                case "Legumes-Chickpea": return 37;
                case "Legumes-KidneyBeans": return 38;
                case "Legumes-Lentils": return 39;
                case "Legumes-Peas": return 40;
                case "Legumes-WhiteBeans": return 41;
                case "Perfumery-AidBand": return 42;
                case "Perfumery-Alcohol": return 43;
                case "Perfumery-Dehodorant": return 44;
                case "Perfumery-HairBrush": return 45;
                case "Perfumery-HairRubberBand": return 46;
                case "Perfumery-Shampoo": return 47;
                case "Perfumery-ShowerGel": return 48;
                case "Perfumery-Sponge": return 49;
                case "Perfumery-Wipe": return 50;
                case "Perfumery-Toothbrush": return 51;
                case "Perfumery-Toothpaste": return 52;
                default: return -1; // Si no se encuentra
            }
        }
    }

    [Serializable]
    public class MinigamesData
    {
        public int Minigame;
        public float MinigameDuration;
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

    private void OnDestroy()
    {
        // Fill the necessary classes data
        EndClasses();
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

        // Init necessary data
        sesionData.OnAwakeData();
    }

    private void EndClasses()
    {

        sesionData.OnDestroyData();
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
