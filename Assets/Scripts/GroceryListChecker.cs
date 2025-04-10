using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroceryListChecker : MonoBehaviour
{
    private LevelLoader levelLoader;
    [SerializeField] private ExplanationCanvas explanationCanvas;
    [SerializeField] private Canvas notificationCanvas;

    [SerializeField] private GameObject prefabFoodManager;
    [SerializeField] private GameObject prefabFoodItemList;

    [SerializeField] private GameObject parentList;
    [SerializeField] private Canvas canvas;

    [SerializeField] Sprite bakeryIcons, fruitsIcons, legumesIcons, fridgeIcons, fishIcons, perfumeryIcons;

    //List of pending to classify item
    List<Food> foodPendings = new List<Food>();
    int numItems = 20;
    DropFieldGroceryList[] dropFields = new DropFieldGroceryList[6];

    // Start is called before the first frame update
    private void Awake()
    {
        explanationCanvas = FindObjectOfType<ExplanationCanvas>();

        explanationCanvas.SetText("Para ser un buen agente, lo primero que hay que hacer es planificar la misión. " +
                "Así que vamos a clasificar los objetivos en las distintas secciones del supermercado donde podemos encontrarlos. \n" +
                "¿Listo? ¡Pues vamos allá! Arrastra los elementos en cada sección.");
    }

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        AudioManager.GetInstance().PlayMusicClip(AudioManager.GetInstance().generalMusic);
        notificationCanvas.gameObject.SetActive(false);
        this.GenerateGroceryListV2();
        dropFields = FindObjectsOfType<DropFieldGroceryList>();
    }

    bool checkClassification()
    {
        bool isCorrect = true;
        //Todo: Check is groceries are correctly classify
        //Check if pending item list is empty
        if(parentList.transform.childCount != 0)
        {
            isCorrect = false;
            Debug.Log("No se ha clasificado todo - " + parentList.transform.childCount);
        }
        else
        {
            //Then check each category, run through every item and check its category propert
            for(int i=0; i<dropFields.Length && isCorrect; i++)
            {
                Debug.Log("Estoy comprobando " + dropFields[i].GetComponent<DropFieldGroceryList>().value);

                foreach (GameObject item in dropFields[i].items)
                {
                    if (item.GetComponent<Food>().category != dropFields[i].GetComponent<DropFieldGroceryList>().value)
                    {
                        isCorrect = false;
                        Debug.Log("Algo mal con "+item.GetComponent<Food>().category);
                    }
                }
            }
        }
        return isCorrect;
    }

    public void OnClickCheck()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        //Check if correct
        if (checkClassification())
        {
            EventManager.OnSaveTimer();
            levelLoader.LoadNextLevel("SupermarketMap");
        }
        else
        {
            Debug.Log("Hay algo mal");
            notificationCanvas.gameObject.SetActive(true);
            notificationCanvas.GetComponentInChildren<TMP_Text>().text = "Hay algo mal clasificado o te faltan elementos por clasificar";
        }
    }

    void GenerateGroceryListV2()
    {
        FoodResourcesManager fm = Instantiate(prefabFoodManager).GetComponent<FoodResourcesManager>();
        SetAsNotTakenFood(fm.bakeryFoods);
        SetAsNotTakenFood(fm.fruitsFoods);
        SetAsNotTakenFood(fm.legumeFoods);
        SetAsNotTakenFood(fm.fridgeFoods);
        SetAsNotTakenFood(fm.fishFoods);
        SetAsNotTakenFood(fm.perfumeryFoods);
        List<Food> allFoods = fm.bakeryFoods;
        allFoods.AddRange(fm.fruitsFoods);
        allFoods.AddRange(fm.legumeFoods);
        allFoods.AddRange(fm.fridgeFoods);
        allFoods.AddRange(fm.fishFoods);
        allFoods.AddRange(fm.perfumeryFoods);

        int randIndex = 0;
        for(int i=0; i<numItems; i++)
        {
            randIndex = Random.Range(0, allFoods.Count);
            //Meter ese alimento en la lista
            foodPendings.Add(allFoods[randIndex].GetComponent<Food>());
            //pendings.Add(allFoods[randIndex].GetComponent<Food>().foodName);
            //clasificarlo en las listas de tipos de GM
            switch (allFoods[randIndex].GetComponent<Food>().category)
            {
                case Food.Category.bakery:
                    GameManager.GetInstance().bakeryFoodList.Add(allFoods[randIndex]);
                    break;
                case Food.Category.fruit:
                    GameManager.GetInstance().fruitFoodList.Add(allFoods[randIndex]);
                    break;
                case Food.Category.legume:
                    GameManager.GetInstance().legumeFoodList.Add(allFoods[randIndex]);
                    break;
                case Food.Category.fridge:
                    GameManager.GetInstance().fridgeFoodList.Add(allFoods[randIndex]);
                    break;
                case Food.Category.fish:
                    GameManager.GetInstance().fishFoodList.Add(allFoods[randIndex]);
                    break;
                case Food.Category.perfumery:
                    GameManager.GetInstance().perfumeryFoodList.Add(allFoods[randIndex]);
                    break;
                default:
                    break;
            }
            //Eliminarlo del conjunto de todos
            allFoods.RemoveAt(randIndex);
        }
        GenerateClasificationList();
    }

    void GenerateClasificationList()
    {
        for(int i=0; i<foodPendings.Count; i++)
        {
            GameObject g = Instantiate(prefabFoodItemList);
            //set parent
            g.transform.SetParent(parentList.transform);
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            g.GetComponent<DragAndDropGroceryList>().canvas = canvas;
            g.GetComponent<DragAndDropGroceryList>().upperParent = canvas;
            //set index
            int rand = Random.Range(0, numItems);
            g.transform.SetSiblingIndex(rand);
            g.GetComponentInChildren<TMP_Text>().text = foodPendings[i].GetComponent<Food>().foodName;
            g.GetComponent<Food>().category = foodPendings[i].category;
            switch (g.GetComponent<Food>().category)
            {
                case Food.Category.bakery:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = bakeryIcons;
                    break;
                case Food.Category.fruit:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = fruitsIcons;
                    break;
                case Food.Category.legume:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = legumesIcons;
                    break;
                case Food.Category.fridge:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = fridgeIcons;
                    break;
                case Food.Category.fish:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = fishIcons;
                    break;
                case Food.Category.perfumery:
                    g.transform.Find("Icons").GetComponent<Image>().sprite = perfumeryIcons;
                    break;
                default:
                    break;
            }
        }
    }

    void SetAsNotTakenFood(List<Food> list)
    {
        foreach (Food f in list){
            f.alreadyTaken = false;
        }
    }
}
