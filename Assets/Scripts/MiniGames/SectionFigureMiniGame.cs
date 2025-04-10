using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void StartMiniGame()
    {
        PrepareMiniGame();
        PrepareToggles();
        //Esperar 3 segundos y mostrar el siguiente item
        StartCoroutine(PickedItemCorrutine());
    }

    IEnumerator PickedItemCorrutine()
    {
        int pid = ++actualPID;
        Debug.Log("Mi PID es: " + pid);
        while (pid == actualPID && !stopMiniGame)
        {
            this.NextItem();
            yield return new WaitForSeconds(2);
        }
        if (stopMiniGame)
        {
            this.SaveCorrectItems();
        }
        Debug.Log("Me destruyo: " + pid);
    }

    void PrepareToggles()
    {
        Toggle[] toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener(delegate{ 
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
        //Instanciar la fial de figuras
        for (currentIndex=0; currentIndex<6; currentIndex++)
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
        Debug.Log("alimento cogidos anteriormente " + countAlreadytaken + " alimentos en la lista "+list.Count);
    }

    void ShuffleList(List<Food> list)
    {

        Food food1;
        int randIndex = Random.Range(0,list.Count);
        for(int i=0; i < list.Count; i++)
        {
            food1 = list[i];
            list[i] = list[randIndex];
            list[randIndex] = food1;
        }
    }

    void NextItem()
    {
        //Borrar el primero
        //darkIcons.Dequeue();
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
        //Check if the toggle activated is the actual posiible object
        if(foodSelected.GetComponent<Food>().foodName == food.GetComponent<Food>().foodName)
        {
            //correcto
            //foodSelected.GetComponent<Renderer>().enabled = false;
            //ColorBlock c = foodSelected.GetComponent<Toggle>().colors;
            //c.pressedColor = new Color(0,0,0, 1f);
            foodSelected.GetComponent<Toggle>().interactable = false;
            int index = groceryList.FindIndex(s => s.foodName == foodSelected.GetComponent<Food>().foodName);
            int index2 = -1;
            bool alreadyTaken = false;
            if (index != -1)
            {
                Debug.Log("Esta en la listaaaa");
                switch (GameManager.GetInstance().actualSection)
                {
                    case Food.Category.bakery:
                        index2 = GameManager.GetInstance().bakeryFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().bakeryFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().bakeryFoodList[index2].alreadyTaken = true;
                        break;
                    case Food.Category.fruit:
                        index2 = GameManager.GetInstance().fruitFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().fruitFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().fruitFoodList[index2].alreadyTaken = true;
                        break;
                    case Food.Category.legume:
                        index2 = GameManager.GetInstance().legumeFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().legumeFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().legumeFoodList[index2].alreadyTaken = true;
                        break;
                    case Food.Category.fridge:
                        index2 = GameManager.GetInstance().fridgeFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().fridgeFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().fridgeFoodList[index2].alreadyTaken = true;
                        break;
                    case Food.Category.fish:
                        index2 = GameManager.GetInstance().fishFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().fishFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().fishFoodList[index2].alreadyTaken = true;
                        break;
                    case Food.Category.perfumery:
                        index2 = GameManager.GetInstance().perfumeryFoodList.FindIndex(s => s.GetComponent<Food>().foodName == foodSelected.GetComponent<Food>().foodName);
                        if (GameManager.GetInstance().perfumeryFoodList[index2].alreadyTaken)
                            alreadyTaken = true;
                        else
                            GameManager.GetInstance().perfumeryFoodList[index2].alreadyTaken = true;
                        break;
                    default:
                        break;
                }
                Debug.Log("valor alreadyTaken: "+alreadyTaken);
                if (!alreadyTaken)
                {
                    correctItems.Add(groceryList[index]);
                    GameManager.GetInstance().pickedListItems++;
                }
                else
                {
                    wrongSelected++;
                }
                //groceryList.RemoveAt(index);
                Debug.Log("se han cogido correctos " + correctItems.Count +" y en la lista hay "+ groceryList.Count);
                if (correctItems.Count == groceryList.Count)
                {
                    stopMiniGame = true;
                    EventManager.OnTimerStop();
                }
            }
            else
            {
                //No esta en la lista de la compra
                wrongSelected++;
            }
            StartCoroutine(PickedItemCorrutine());
        }
        else
        {
            foodSelected.GetComponent<Toggle>().isOn = false;
        }
        
    }

    void SaveCorrectItems()
    {
        Debug.Log("Son correctos: " + correctItems.Count);
        GameManager.GetInstance().pickedItems = correctItems;
        GameManager.GetInstance().numWrongPickedItems += wrongSelected;
        EventManager.OnSaveTimer();
        lvlLoader.LoadNextLevel("TrolleyScene 1");
    }
}
