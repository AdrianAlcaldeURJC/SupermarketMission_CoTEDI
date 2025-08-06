using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class SectionFigureMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject darkIconPrefab;
    private Queue<GameObject> darkIcons = new Queue<GameObject>();
    [SerializeField] private FoodResourcesManager foodManager;
    [SerializeField] GameObject darkIconsPanel;
    [SerializeField] LevelLoader lvlLoader;

    private int currentIndex;
    List<Food> possibilities;
    List<Food> groceryList;

    public bool stopMiniGame = false;
    private int actualPID = 0;

    private int wrongSelected = 0;
    List<Food> correctItems = new List<Food>();

    // Saving data
    [SerializeField] private MinigameListener minigameListener;
    private bool itemSkipped = true;
    public void StartMiniGame()
    {
        PrepareMiniGame();
        PrepareToggles();
        //Esperar 3 segundos y mostrar el siguiente item
        StartCoroutine(PickedItemCoroutine());
    }

    IEnumerator PickedItemCoroutine()
    {
        int pid = ++actualPID;
        while (pid == actualPID && !stopMiniGame)
        {
            this.NextItem();
            yield return new WaitForSeconds(2);

            if (itemSkipped)
            {
                int itemID = DataStorage.GroceryMapData.GetIDfromStringFood(darkIcons.Peek().GetComponent<Food>().foodName);
                minigameListener.AddShadowSkipped(
                        itemID,
                        IsCorrectItem(darkIcons.Peek()),
                        minigameListener.GetElapsedTime()
                    );
            }

            itemSkipped = true;
        }
        if (stopMiniGame)
        {
            this.SaveCorrectItems();
        }
    }

    void PrepareToggles()
    {
        Toggle[] toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener(delegate
            {
                ObjectSelected(t.gameObject);
            });
        }
    }

    void PrepareMiniGame()
    {
        currentIndex = 0;

        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                possibilities = foodManager.bakeryFoods;
                groceryList = GameManager.GetInstance().bakeryFoodList;
                break;
            case Food.Category.fruit:
                possibilities = foodManager.fruitsFoods;
                groceryList = GameManager.GetInstance().fruitFoodList;
                break;
            case Food.Category.legume:
                possibilities = foodManager.legumeFoods;
                groceryList = GameManager.GetInstance().legumeFoodList;
                break;
            case Food.Category.fridge:
                possibilities = foodManager.fridgeFoods;
                groceryList = GameManager.GetInstance().fridgeFoodList;
                break;
            case Food.Category.fish:
                possibilities = foodManager.fishFoods;
                groceryList = GameManager.GetInstance().fishFoodList;
                break;
            case Food.Category.perfumery:
                possibilities = foodManager.perfumeryFoods;
                groceryList = GameManager.GetInstance().perfumeryFoodList;
                break;
            default:
                possibilities = new List<Food>();
                groceryList = new List<Food>();
                break;
        }
        CountAlreadyTakenItems(groceryList);
        ShuffleList(possibilities);
        //Instanciar la fila de figuras
        for (currentIndex = 0; currentIndex < 6; currentIndex++)
        {
            GameObject gO = Instantiate(darkIconPrefab);
            gO.transform.GetChild(0).GetComponent<Image>().sprite = possibilities[currentIndex].sprite;
            gO.AddComponent<Food>().CopyFood(possibilities[currentIndex]);
            gO.transform.SetParent(darkIconsPanel.transform, false);
            darkIcons.Enqueue(gO);
        }
    }

    void CountAlreadyTakenItems(List<Food> list)
    {
        int countAlreadytaken = 0;
        foreach (Food f in list)
        {
            if (f.alreadyTaken)
                countAlreadytaken++;
        }
        if (countAlreadytaken == list.Count)
            stopMiniGame = true;
    }

    void ShuffleList(List<Food> list)
    {

        Food food1;
        int randIndex = Random.Range(0, list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            food1 = list[i];
            list[i] = list[randIndex];
            list[randIndex] = food1;
        }
    }

    void NextItem()
    {
        // Recoger datos del item 

        //Borrar el primero
        Destroy(darkIcons.Dequeue());
        //Crear un ultimo
        GameObject gO = Instantiate(darkIconPrefab);
        gO.transform.GetChild(0).GetComponent<Image>().sprite = possibilities[currentIndex].sprite;
        gO.AddComponent<Food>().CopyFood(possibilities[currentIndex]);
        gO.transform.SetParent(darkIconsPanel.transform, false);
        darkIcons.Enqueue(gO);
        //Incrementar el index
        currentIndex = ++currentIndex % possibilities.Count;
    }

    void ObjectSelected(GameObject foodSelected)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        GameObject food = darkIcons.Peek();
        bool alreadyTaken = false;
        bool isCorrect = false;

        //Check if the toggle activated is the actual possible object

        if (foodSelected.GetComponent<Food>().foodName == food.GetComponent<Food>().foodName)
        {
            //correcto
            itemSkipped = false;
            //foodSelected.GetComponent<Renderer>().enabled = false;
            //ColorBlock c = foodSelected.GetComponent<Toggle>().colors;
            //c.pressedColor = new Color(0,0,0, 1f);
            foodSelected.GetComponent<Toggle>().interactable = false;
            int index = groceryList.FindIndex(s => s.foodName == foodSelected.GetComponent<Food>().foodName);
            int index2 = -1;
            if (index != -1)
            {
                alreadyTaken = CheckIfTaken(foodSelected, ref index2);
                if (!alreadyTaken)
                    ModifyFoodTaken(index2);

                if (!alreadyTaken)
                {
                    correctItems.Add(groceryList[index]);
                    GameManager.GetInstance().pickedListItems++;
                    isCorrect = true;
                }
                else
                {
                    wrongSelected++;
                }

                if (correctItems.Count == groceryList.Count)
                {
                    isCorrect = true;
                    stopMiniGame = true;
                    EventManager.OnTimerStop();
                }
            }
            else
            {
                //No esta en la lista de la compra
                wrongSelected++;
            }

            StartCoroutine(PickedItemCoroutine());
        }
        else
        {
            foodSelected.GetComponent<Toggle>().isOn = false;
        }

        int isCorrectMoment = foodSelected.GetComponent<Food>().foodName == food.GetComponent<Food>().foodName ? 1 : 0;
        
        minigameListener.AddShadowPick(
            DataStorage.GroceryMapData.GetIDfromStringFood(foodSelected.GetComponent<Food>().foodName),
            isCorrect ? 1 : 0,
            minigameListener.GetElapsedTime(),
            isCorrectMoment
        );

    }

    private bool CheckIfTaken(GameObject foodSelected, ref int index2)
    {
        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                index2 = GameManager.GetInstance().bakeryFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().bakeryFoodList[index2].alreadyTaken)
                    return true;
                break;
            case Food.Category.fruit:
                index2 = GameManager.GetInstance().fruitFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().fruitFoodList[index2].alreadyTaken)
                    return true;
                break;
            case Food.Category.legume:
                index2 = GameManager.GetInstance().legumeFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().legumeFoodList[index2].alreadyTaken)
                    return true;
                break;
            case Food.Category.fridge:
                index2 = GameManager.GetInstance().fridgeFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().fridgeFoodList[index2].alreadyTaken)
                    return true;
                break;
            case Food.Category.fish:
                index2 = GameManager.GetInstance().fishFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().fishFoodList[index2].alreadyTaken)
                    return true;
                break;
            case Food.Category.perfumery:
                index2 = GameManager.GetInstance().perfumeryFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                if (GameManager.GetInstance().perfumeryFoodList[index2].alreadyTaken)
                    return true;
                break;
            default:
                break;
        }
        return false; // No section matches
    }

    private bool ModifyFoodTaken(int index)
    {
        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                GameManager.GetInstance().bakeryFoodList[index].alreadyTaken = true;
                break;
            case Food.Category.fruit:
                GameManager.GetInstance().fruitFoodList[index].alreadyTaken = true;
                break;
            case Food.Category.legume:
                GameManager.GetInstance().legumeFoodList[index].alreadyTaken = true;
                break;
            case Food.Category.fridge:
                    GameManager.GetInstance().fridgeFoodList[index].alreadyTaken = true;
                break;
            case Food.Category.fish:
                GameManager.GetInstance().fishFoodList[index].alreadyTaken = true;
                break;
            case Food.Category.perfumery:
                GameManager.GetInstance().perfumeryFoodList[index].alreadyTaken = true;
                break;
            default:
                break;
        }
        return false; // No section matches
    }

    void SaveCorrectItems()
    {
        GameManager.GetInstance().pickedItems = correctItems;
        GameManager.GetInstance().numWrongPickedItems += wrongSelected;
        EventManager.OnSaveTimer();
        lvlLoader.LoadNextLevel("TrolleyScene 1");


    }

    int IsCorrectItem(GameObject foodSelected)
    {
        string foodName = foodSelected.GetComponent<Food>().foodName;
        int index = groceryList.FindIndex(s => s.foodName == foodName);
        if (index != -1)
        {
            if (!CheckIfTaken(foodSelected, ref index))
                return 1; // Correct item
        }
        return 0;
    }
}

