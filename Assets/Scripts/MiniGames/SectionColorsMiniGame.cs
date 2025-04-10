using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionColorsMiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject colorButtonPrefab;
    [SerializeField] private GameObject buttonsScrollPanel;
    [SerializeField] private Image filterImage;

    [SerializeField] private FoodResourcesManager foodManager;
    [SerializeField] private LevelLoader lvlLoader;

    private List<Food> sectionFood = new List<Food>();
    private List<Food> groceryList = new List<Food>();
    private List<Food> correctItems = new List<Food>();
    private int correctSelected;
    private int wrongSelected;
    private Toggle[] toggles;

    //private bool stopMiniGame = false;

    public void StartMiniGame()
    {
        PrepareMiniGame();
        PrepareToggles();
    }

    private void PrepareMiniGame()
    {
        filterImage.color = new Color32(0,0,0,0);
        //Coger los alimentos de section de food resource manager
        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                sectionFood = foodManager.bakeryFoods;
                groceryList = GameManager.GetInstance().bakeryFoodList;
                break;
            case Food.Category.fruit:
                sectionFood = foodManager.fruitsFoods;
                groceryList = GameManager.GetInstance().fruitFoodList;
                break;
            case Food.Category.legume:
                sectionFood = foodManager.legumeFoods;
                groceryList = GameManager.GetInstance().legumeFoodList;
                break;
            case Food.Category.fridge:
                sectionFood = foodManager.fridgeFoods;
                groceryList = GameManager.GetInstance().fridgeFoodList;
                break;
            case Food.Category.fish:
                sectionFood = foodManager.fishFoods;
                groceryList = GameManager.GetInstance().fishFoodList;
                break;
            case Food.Category.perfumery:
                sectionFood = foodManager.perfumeryFoods;
                groceryList = GameManager.GetInstance().perfumeryFoodList;
                break;
            default:
                sectionFood = new List<Food>();
                groceryList = new List<Food>();
                break;
        }
        CountAlreadyTakenItems(groceryList);
        //Instancia los botones de los colores que tenga los alimentos
        List<Food.colors> sectionColors = new List<Food.colors>();
        foreach(Food f in sectionFood)
        {
            foreach(Food.colors color in f.color)
            {
                if (!sectionColors.Contains(color))
                {
                    sectionColors.Add(color);
                    GameObject button = Instantiate(colorButtonPrefab);
                    button.transform.SetParent(buttonsScrollPanel.transform, false);
                    button.GetComponent<Image>().color = SetColorButton(color);
                    button.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        SetColorVisibility(color);
                    });
                }
            }
        }
        Debug.Log("Numero de colores en esta seccion: " + sectionColors.Count);
    }

    Color SetColorButton(Food.colors color)
    {
        switch (color)
        {
            case Food.colors.red:
                return new Color32(255,0,0,255);
            case Food.colors.orange:
                return new Color32(255, 159, 0, 255);
            case Food.colors.yellow:
                return new Color32(255, 255, 0, 255);
            case Food.colors.green:
                return new Color32(21, 171, 0, 255);
            case Food.colors.blue:
                return new Color32(0, 0, 255, 255);
            case Food.colors.purple:
                return new Color32(84, 22, 180, 255);
            case Food.colors.brown:
                return new Color32(88, 57, 39, 255);
            case Food.colors.black:
                return new Color32(0, 0, 0, 255);
            case Food.colors.white:
                return new Color32(255, 255, 255, 255);
            default:
                return new Color(0, 0, 0, 255);
        }
    }

    void PrepareToggles()
    {
        toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener(delegate {
                ObjectSelected(t.gameObject);
            });
            t.GetComponent<CanvasGroup>().alpha = 0;
            t.GetComponent<CanvasGroup>().interactable = false;
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
            //stopMiniGame = true;
            SaveCorrectItems();
        correctSelected = countAlreadytaken;
        Debug.Log("alimento cogidos anteriormente " + countAlreadytaken + " alimentos en la lista " + list.Count);
    }

    void SetColorVisibility(Food.colors color)
    {
        if (filterImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadedFilter"))
        {
            //Visualizar solo los alimentos que tengas ese color
            //Poner el filtro a ese color
            Color32 temp = SetColorButton(color);
            filterImage.GetComponent<Animator>().SetTrigger("Appear");
            filterImage.color = new Color32(temp.r, temp.g, temp.b, 125);
            //Recorrer toogles y porner alfa a 0 de los que lo tengan
            Debug.Log("Pulsado para " + color);
            bool hasColor;
            foreach (Toggle toggle in toggles)
            {
                hasColor = false;
                foreach (Food.colors foodcolor in toggle.GetComponent<Food>().color)
                {
                    if (foodcolor == color)
                    {
                        hasColor = true;
                    }
                }
                if (hasColor)
                {
                    toggle.GetComponent<CanvasGroup>().alpha = 1;
                    toggle.GetComponent<CanvasGroup>().interactable = true;
                }
            }
            filterImage.GetComponent<Animator>().SetTrigger("Fade");
            StartCoroutine(MakeItemsDisappear());
        }
    }

    IEnumerator MakeItemsDisappear()
    {
        //Debug.Log("Animation duration: "+filterImage.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        yield return new WaitForSeconds(2f);

        //yield return new WaitForSeconds(filterImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        foreach (Toggle t in toggles)
        {
            t.GetComponent<CanvasGroup>().alpha = 0;
            t.GetComponent<CanvasGroup>().interactable = false;
        }
    }

    void ObjectSelected(GameObject foodSelected)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
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
            Debug.Log("valor alreadyTaken: " + alreadyTaken);
            if (!alreadyTaken)
            {
                //Si no habia sido aun cogido, se añade a la lista de correctos
                correctItems.Add(groceryList[index]);
                correctSelected++;
                GameManager.GetInstance().pickedListItems++;
            }
            else
            {
                wrongSelected++;
            }
            //groceryList.RemoveAt(index);
            Debug.Log("se han cogido correctos " + correctItems.Count + " y en la lista hay " + groceryList.Count);
            //if (correctItems.Count == groceryList.Count)
                //stopMiniGame = true;
            if(correctSelected== groceryList.Count)
                SaveCorrectItems();
        }
        else
        {
            //No esta en la lista de la compra
            wrongSelected++;
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
