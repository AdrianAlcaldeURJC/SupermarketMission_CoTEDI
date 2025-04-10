using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public string playerName;
    public string playerInitial;
    public int playerAge;
    public string playerGender;
    public bool daltonicUser = false;

    public float musicVolume = 0.5f;
    public float SFXVolume = 0.5f;

    public bool[] firstTimeScene = { true, true, true, true, true, true, true };

    public float currentSpentTime = 0f;
    public float groceryListSpentTime = 0f;
    public float SupermarketMapSpentTime = 0f;
    public float trolleySpentTime = 0f;
    public float[] minigamesSpentTime = {0f,0f,0f,0f,0f,0f};

    public Food.Category actualSection;
    public Food.Category[] sectionDistribution = new Food.Category[6];

    public Food[,] trolleyStatus;       //8x3
    public List<Food> pickedItems;

    public List<Food> bakeryFoodList;
    public List<Food> fruitFoodList;
    public List<Food> legumeFoodList;
    public List<Food> fridgeFoodList;
    public List<Food> fishFoodList;
    public List<Food> perfumeryFoodList;

    public int pickedListItems = 0;
    public int numWrongPickedItems = 0;
    public int numElementsCorrectPositionTrolley = 0;
    public int numElementsModeratePositionTrolley = 0;
    public int numElementsWrongPositionTrolley = 0;

    DataBaseComunicator dbCom;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("GameManager")) as GameObject;
        GameObject.DontDestroyOnLoad(main);
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
            this.InitializeGame();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void InitializeGame()
    {
        trolleyStatus = new Food[8, 3];
        musicVolume = 0.5f;
        SFXVolume = 0.5f;
        dbCom = this.gameObject.GetComponent<DataBaseComunicator>();
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public void GoToScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void SavePlayerData(string name, int age, string gender)
    {
        playerName = name;
        playerInitial = playerName.Substring(0, 1).ToUpper();
        playerAge = age;
        playerGender = gender;
    }

    public void ResetGameManager()
    {
        playerName = "";
        playerInitial = "";
        playerAge = 7;
        playerGender = "";
        daltonicUser = false;

        //firstTimeScene = { true, true, true, true, true, true, true };

        currentSpentTime = 0f;
        groceryListSpentTime = 0f;
        SupermarketMapSpentTime = 0f;
        trolleySpentTime = 0f;

        minigamesSpentTime = new float[6];
        trolleyStatus = new Food[8, 3];

        //for (int i = 0; i < minigamesSpentTime.Length; i++)
        //{
        //    minigamesSpentTime[i] = 0;
        //}
        //for (int i = 0; i < trolleyStatus.GetLength(0); i++)
        //{
        //    for (int j = 0; j < trolleyStatus.GetLength(1); j++)
        //    {
        //        trolleyStatus[i,j] = null;
        //    }
        //}
        pickedItems = new List<Food>();

        bakeryFoodList=new List<Food>();
        fruitFoodList = new List<Food>();
        legumeFoodList = new List<Food>();
        fridgeFoodList = new List<Food>();
        fishFoodList = new List<Food>();
        perfumeryFoodList = new List<Food>();

        pickedListItems = 0;
        numWrongPickedItems = 0;
        numElementsCorrectPositionTrolley = 0;
        numElementsModeratePositionTrolley = 0;
        numElementsWrongPositionTrolley = 0;
    }

    public void SendResultToDB()
    {
        Debug.Log(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        string date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //string score = @"""name"": ""name2"", ""start"": """ + date + @""", ""end"": ""2024-07-09 19:27:34""";

        string score = @"""name"": """ + this.playerName + @""", 
                        ""age"": """ + this.playerAge + @""",
                        ""gender"": """ + this.playerGender + @""",
                        ""totalTime"": """ + this.currentSpentTime.ToString().Replace(",", ".") + @""",
                        ""clasifyListTime"": """ + this.groceryListSpentTime.ToString().Replace(",", ".") + @""",
                        ""identifyMapTime"": """ + this.SupermarketMapSpentTime.ToString().Replace(",", ".") + @""",
                        ""organizeTrolleyTime"": """ + this.trolleySpentTime.ToString().Replace(",", ".") + @""",
                        ""bakeryMGTime"": """ + this.minigamesSpentTime[0].ToString().Replace(",", ".") + @""",
                        ""fruitsMGTime"": """ + this.minigamesSpentTime[1].ToString().Replace(",", ".") + @""",
                        ""legumesMGTime"": """ + this.minigamesSpentTime[2].ToString().Replace(",", ".") + @""",
                        ""fridgeMGTime"": """ + this.minigamesSpentTime[3].ToString().Replace(",", ".") + @""",
                        ""fishMGTime"": """ + this.minigamesSpentTime[4].ToString().Replace(",", ".") + @""",
                        ""perfumeryMGTime"": """ + this.minigamesSpentTime[5].ToString().Replace(",", ".") + @""",
                        ""correctPickedItems"": """ + this.pickedListItems + @""",
                        ""wrongPickedItems"": """ + this.numWrongPickedItems + @""",
                        ""correctPositionTroley"": """ + this.numElementsCorrectPositionTrolley + @""",
                        ""moderatePositionTrolley"": """ + this.numElementsModeratePositionTrolley + @""",
                        ""wrongPositionTrolley"": """ + this.numElementsWrongPositionTrolley + @""",
                        ""date"": """ + date + @"""";

        Debug.Log(score);
        dbCom.SendInsertRequest(score);
    }

    public void EvaluateFinalTrolley()
    {
        for (int i = 0; i < trolleyStatus.GetLength(0); i++)
        {
            //Si existe el de arriba
            if (trolleyStatus[i, 0])
            {
                trolleyStatus[i, 0].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.mid && trolleyStatus[i, 1].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }
                else
                if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 1].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus = Food.positionStatus.wrong;
                    numElementsWrongPositionTrolley++;
                }
                else
                if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 1].GetComponent<Food>().hardness == Food.hardnessLevel.mid)
                {
                    trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }
                if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.mid && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }
                else
                if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.wrong;
                    numElementsWrongPositionTrolley++;
                }
                else if (trolleyStatus[i, 0].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.mid)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }
            }
            else
            {
                if (trolleyStatus[i, 1])
                {
                    trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                }
                if (trolleyStatus[i, 2])
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                }
            }
            //Si existe el del medio
            if (trolleyStatus[i, 1] && trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus != Food.positionStatus.wrong)
            {
                if (trolleyStatus[i, 1].GetComponent<Food>().weight == Food.weightLevel.mid && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }
                else
                if (trolleyStatus[i, 1].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.wrong;
                    numElementsWrongPositionTrolley++;
                }
                else
                if (trolleyStatus[i, 1].GetComponent<Food>().weight == Food.weightLevel.heavy && trolleyStatus[i, 2].GetComponent<Food>().hardness == Food.hardnessLevel.mid)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.moderate;
                    numElementsModeratePositionTrolley++;
                }

            }
            else
            {
                if (trolleyStatus[i, 2] && trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus != Food.positionStatus.wrong)
                {
                    trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus = Food.positionStatus.good;
                }
            }

            if(trolleyStatus[i, 2])
                if(trolleyStatus[i, 2].GetComponent<Food>().trolleyStatus == Food.positionStatus.good)
                    numElementsCorrectPositionTrolley++;
            if (trolleyStatus[i, 1]) 
                if (trolleyStatus[i, 1].GetComponent<Food>().trolleyStatus == Food.positionStatus.good)
                    numElementsCorrectPositionTrolley++;
            if (trolleyStatus[i, 0]) 
                if (trolleyStatus[i, 0].GetComponent<Food>().trolleyStatus == Food.positionStatus.good)
                    numElementsCorrectPositionTrolley++;

        }
    }
}
