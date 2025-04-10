using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SectionGenerator : MonoBehaviour
{
    [SerializeField] private LevelLoader lvlLoader;
    [SerializeField] SectionFigureMiniGame minigameFigures;
    [SerializeField] SectionColorsMiniGame minigameColors;
    [SerializeField] private ExplanationCanvas explanationCanvas;

    [SerializeField] private GameObject sectionPanel;
    [SerializeField] private GameObject foodElement;
    [SerializeField] private GameObject foodResourcesPrefab;
    [SerializeField] private Image backgroungSection;
    [SerializeField] private Sprite[] backgrounds = new Sprite[6];

    [SerializeField] List<Food> listElements = new List<Food>();

    [SerializeField] Canvas groceryListCanvas;

    // Start is called before the first frame update
    void Start()
    {
        lvlLoader = FindObjectOfType<LevelLoader>();
        minigameFigures = FindObjectOfType<SectionFigureMiniGame>();
        minigameColors = FindObjectOfType<SectionColorsMiniGame>();
        //explanationCanvas = FindObjectOfType<ExplanationCanvas>();
        
        PopulateSection();
        groceryListCanvas.gameObject.SetActive(false);
        ChooseMiniGame();
    }

    void ChooseMiniGame()
    {
        if(GameManager.GetInstance().actualSection == Food.Category.fruit && !GameManager.GetInstance().daltonicUser)
        {
            explanationCanvas.SetTextChecking(3, "¡Vaya! Parece que aquí utilizan un sistema de camuflaje. Tendrás que equiparte gafas especiales para poder ver los alimentos. " +
            "\nPincha en la barra de abajo que color de gafas quieres usar y date prisa en coger lo que necesites, solo duran unos pocos segundos. \nSaldrás de aquí " +
            "cuando hayas cogido todo lo que necesitas.");            

            minigameColors.StartMiniGame();
            minigameFigures.gameObject.SetActive(false);
        }
        else
        {
            explanationCanvas.SetTextChecking(4, "En esta zona del supermercado utilizan unos sensores para evitar que cojamos lo que queremos. Tendrás que aprovechar para coger el " +
            "alimento que necesitas justo cuando su silueta se encuentre en el recuadro resaltado de la izquierda. ¡Actúa rápido o perderás la oportunidad! \nSaldrás de " +
            "aquí cuando hayas cogido todo lo que necesitas.");
            

            minigameFigures.StartMiniGame();
            minigameColors.gameObject.SetActive(false);
        }
    }

    void PopulateSectionWithoutRepetition()
    {
        GameObject foodResources = Instantiate(foodResourcesPrefab);
        //Get actual section del GM
        //Get lista de alimentos de la seccion
        //List<String> elements = new List<String>();
        List<Food> allSectionFoods = new List<Food>();
        //List<Sprite> foodSprites = new List<Sprite>();
        Debug.Log("Creamdo seccion de " + GameManager.GetInstance().actualSection);

        switch (GameManager.GetInstance().actualSection)
        //switch (Food.Category.bakery)
        {
            case Food.Category.bakery:
                backgroungSection.sprite = backgrounds[0];
                listElements.AddRange(GameManager.GetInstance().bakeryFoodList);
                allSectionFoods.AddRange(foodResources.GetComponent<FoodResourcesManager>().bakeryFoods);
                //foodSprites.AddRange(foodResources.GetComponent<FoodResourcesManager>().bakery);
                break;
            case Food.Category.fruit:
                backgroungSection.sprite = backgrounds[1];
                listElements = GameManager.GetInstance().fruitFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fruitsFoods;
                break;
            case Food.Category.legume:
                backgroungSection.sprite = backgrounds[2];
                listElements = GameManager.GetInstance().legumeFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().legumeFoods;
                break;
            case Food.Category.fridge:
                backgroungSection.sprite = backgrounds[3];
                listElements = GameManager.GetInstance().fridgeFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fridgeFoods;
                break;
            case Food.Category.fish:
                backgroungSection.sprite = backgrounds[4];
                listElements = GameManager.GetInstance().fishFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fishFoods;
                break;
            case Food.Category.perfumery:
                backgroungSection.sprite = backgrounds[5];
                listElements = GameManager.GetInstance().perfumeryFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().perfumeryFoods;
                break;
            default:
                listElements = new List<Food>();
                //foodSprites = new List<Sprite>();
                allSectionFoods = new List<Food>();
                break;
        }

        //NUmero aleatorio de elementos (entre 3 y size list)
        var numElements = Random.Range(listElements.Count, allSectionFoods.Count);
        Debug.Log("numelements: " + numElements);
        //Crear toggles
        //Instanciar los elemntos de la lista de la compra y guardar su referencia
        GameObject[] toBuyElements = new GameObject[listElements.Count];
        Debug.Log("Num obligatorios: " + listElements.Count);
        Debug.Log("Num sprites: " + allSectionFoods.Count);
        for (int i = 0; i < listElements.Count; i++)
        {
            //VIGILAR SI ESTA BIEN CON NAME O TIENE QUE SER FOOD NAME
            int index = allSectionFoods.FindIndex(s => s.foodName == listElements[i].GetComponent<Food>().foodName);
            //int index = foodSprites.FindIndex(s => s.name == elements[i].ToString());
            //TO DO
            GameObject element = Instantiate(foodElement);
            element.transform.SetParent(sectionPanel.transform,false);
            Debug.Log(index);
            Sprite s = allSectionFoods[index].sprite;
            element.transform.Find("Background").GetComponent<Image>().sprite = s;
            element.transform.Find("Background").Find("Checkmark").GetComponent<Image>().sprite = s;
            element.GetComponent<Food>().CopyFood(listElements[i].GetComponent<Food>());
            allSectionFoods.RemoveAt(index);
            toBuyElements[i] = element;
        }
        //Instanciar elementos de relleno
        for (int i = 0; i < numElements - listElements.Count; i++)
        {
            GameObject element = Instantiate(foodElement);
            element.transform.SetParent(sectionPanel.transform,false);
            //Asignar imagenes aleatorias de la seccion a los toogles
            var rand = Random.Range(0, allSectionFoods.Count);
            Sprite s = allSectionFoods[rand].sprite;
            element.transform.Find("Background").GetComponent<Image>().sprite = s;
            element.transform.Find("Background").Find("Checkmark").GetComponent<Image>().sprite = s;
            element.GetComponent<Food>().CopyFood(allSectionFoods[rand].GetComponent<Food>());
            allSectionFoods.RemoveAt(rand);
            //Debug.Log("randFoodIndex: " + rand + "-- count food remaining " + allSectionFoods.Count);
        }
        // Dar una posicion aleatoria entre los hijos a los elemtos iniciales
        for (int i = 0; i < listElements.Count; i++)
        {
            var rand = Random.Range(0, numElements);
            toBuyElements[i].transform.SetSiblingIndex(rand);
        }
    }

    void PopulateSection()
    {
        GameObject foodResources = Instantiate(foodResourcesPrefab);
        //Get actual section del GM
        //Get lista de alimentos de la seccion
        //List<String> elements = new List<String>();
        List<Food> allSectionFoods = new List<Food>();
        //List<Sprite> foodSprites = new List<Sprite>();
        Debug.Log("Creamdo seccion de " + GameManager.GetInstance().actualSection);

        switch (GameManager.GetInstance().actualSection)
        //switch (Food.Category.bakery)
        {
            case Food.Category.bakery:
                backgroungSection.sprite = backgrounds[0];
                listElements.AddRange(GameManager.GetInstance().bakeryFoodList);
                allSectionFoods.AddRange(foodResources.GetComponent<FoodResourcesManager>().bakeryFoods);
                //foodSprites.AddRange(foodResources.GetComponent<FoodResourcesManager>().bakery);
                break;
            case Food.Category.fruit:
                backgroungSection.sprite = backgrounds[1];
                listElements = GameManager.GetInstance().fruitFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fruitsFoods;
                break;
            case Food.Category.legume:
                backgroungSection.sprite = backgrounds[2];
                listElements = GameManager.GetInstance().legumeFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().legumeFoods;
                break;
            case Food.Category.fridge:
                backgroungSection.sprite = backgrounds[3];
                listElements = GameManager.GetInstance().fridgeFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fridgeFoods;
                break;
            case Food.Category.fish:
                backgroungSection.sprite = backgrounds[4];
                listElements = GameManager.GetInstance().fishFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().fishFoods;
                break;
            case Food.Category.perfumery:
                backgroungSection.sprite = backgrounds[5];
                listElements = GameManager.GetInstance().perfumeryFoodList;
                allSectionFoods = foodResources.GetComponent<FoodResourcesManager>().perfumeryFoods;
                break;
            default:
                listElements = new List<Food>();
                //foodSprites = new List<Sprite>();
                allSectionFoods = new List<Food>();
                break;
        }

        //NUmero aleatorio de elementos (entre 3 y size list)
        var numElements = Random.Range(listElements.Count, 12);
        //var numElements = Random.Range(listElements.Count, allSectionFoods.Count);
        Debug.Log("numelements: " + numElements);
        //Crear toggles
        //Instanciar los elemntos de la lista de la compra y guardar su referencia
        GameObject[] toBuyElements = new GameObject[listElements.Count];
        Debug.Log("Num obligatorios: " + listElements.Count);
        Debug.Log("Num sprites: " + allSectionFoods.Count);
        for (int i = 0; i < listElements.Count; i++)
        {
            int index = allSectionFoods.FindIndex(s => s.foodName == listElements[i].GetComponent<Food>().foodName);
            GameObject element = Instantiate(foodElement);
            element.transform.SetParent(sectionPanel.transform, false);
            Debug.Log(index);
            Sprite s = allSectionFoods[index].sprite;
            element.transform.Find("Background").GetComponent<Image>().sprite = s;
            element.transform.Find("Background").Find("Checkmark").GetComponent<Image>().sprite = s;
            element.GetComponent<Food>().CopyFood(listElements[i].GetComponent<Food>());
            allSectionFoods.RemoveAt(index);
            toBuyElements[i] = element;
        }
        //Instanciar elementos de relleno
        for (int i = 0; i < numElements - listElements.Count; i++)
        {
            GameObject element = Instantiate(foodElement);
            element.transform.SetParent(sectionPanel.transform,false);
            //Asignar imagenes aleatorias de la seccion a los toogles
            var rand = Random.Range(0, allSectionFoods.Count);
            Sprite s = allSectionFoods[rand].sprite;
            element.transform.Find("Background").GetComponent<Image>().sprite = s;
            element.transform.Find("Background").Find("Checkmark").GetComponent<Image>().sprite = s;
            element.GetComponent<Food>().CopyFood(allSectionFoods[rand].GetComponent<Food>());
            //allSectionFoods.RemoveAt(rand);
            //Debug.Log("randFoodIndex: " + rand + "-- count food remaining " + allSectionFoods.Count);
        }
        // Dar una posicion aleatoria entre los hijos a los elemtos iniciales
        for (int i = 0; i < listElements.Count; i++)
        {
            var rand = Random.Range(0, numElements);
            toBuyElements[i].transform.SetSiblingIndex(rand);
        }
    }

    public void OnClickGroceryList()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        groceryListCanvas.gameObject.SetActive(!groceryListCanvas.gameObject.activeSelf);
        groceryListCanvas.gameObject.GetComponent<GroceryListDisplay>().RefreshSection();
    }
}
