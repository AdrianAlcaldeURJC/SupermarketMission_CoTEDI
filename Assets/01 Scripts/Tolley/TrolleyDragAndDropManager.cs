using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrolleyDragAndDropManager : MonoBehaviour
{
    [SerializeField]
    private GameObject layer2;

    [SerializeField]
    private Sprite lightFrame, midFrame, grossFrame;

    [SerializeField]
    private Sprite lightWeight, midWeight, heavyWeight;

    [SerializeField]
    private GameObject newFoodParent;

    [SerializeField]
    private GameObject TrolleyElementPrefab;

    [SerializeField] private GameObject foodResourcesPrefab;

    [SerializeField]
    LevelLoader lvlLoader;
    private ExplanationCanvas explanationCanvas;

    public GameObject[,] trolley = new GameObject[8, 3];

    GameObject foodManager;

    // Start is called before the first frame update
    void Start()
    {
        explanationCanvas = FindObjectOfType<ExplanationCanvas>();

        explanationCanvas.SetTextChecking("ExplicationCanvas", "Trolley_1", 5);
        SetTrolley();
        SetItemsToOrganize();
        lvlLoader = FindObjectOfType<LevelLoader>();
        foodManager = Instantiate(foodResourcesPrefab);
        EvaluateTrolley();
    }

    void SetItemsToOrganize()
    {
        //Mostrar los alimentos que se traen nuevos de la seccion
        //Ver cual es la seccion actual
        //Listar los alimentos obligatorios de esa seccion (y que esten marcados por el isTaken)
        //Comprobar que isTaken este definido como marcado cuando se cogen en la seccion que no lo se
        //Craer el prefab con esos alimentos
        //Ponerles como padre el canvas
        List<Food> newFoods = new List<Food>();
        Debug.Log("Actual section: " + GameManager.GetInstance().actualSection);

        newFoods = GameManager.GetInstance().pickedItems;
        Debug.Log(newFoods.Count);

        foreach (Food foodItem in newFoods)
        {
            //if (foodItem.alreadyTaken)
            //{
            GameObject element = Instantiate(TrolleyElementPrefab);
            element.transform.SetParent(newFoodParent.transform, true);
            element.transform.localScale = new Vector3(1f, 1f, 1f);
            element.GetComponent<TrolleyDragAndDrop>().upperParent = this.gameObject.GetComponent<Canvas>();
            element.GetComponent<TrolleyDragAndDrop>().canvas = this.gameObject.GetComponent<Canvas>();
            element.GetComponent<Food>().CopyFood(foodItem);
            //element.GetComponentInChildren<Image>().sprite = element.GetComponent<Food>().sprite;
            element.transform.Find("FoodImage").GetComponent<Image>().sprite = element.GetComponent<Food>().sprite;
            //Hardness
            if (element.GetComponent<Food>().hardness == Food.hardnessLevel.hard)
                element.transform.Find("FrameImage").GetComponent<Image>().sprite = grossFrame;
            else if (element.GetComponent<Food>().hardness == Food.hardnessLevel.mid)
                element.transform.Find("FrameImage").GetComponent<Image>().sprite = midFrame;
            else if (element.GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                element.transform.Find("FrameImage").GetComponent<Image>().sprite = lightFrame;
            //Weight
            if (element.GetComponent<Food>().weight == Food.weightLevel.heavy)
                element.transform.Find("WeightImage").GetComponent<Image>().sprite = heavyWeight;
            else if (element.GetComponent<Food>().weight == Food.weightLevel.mid)
                element.transform.Find("WeightImage").GetComponent<Image>().sprite = midWeight;
            else if (element.GetComponent<Food>().weight == Food.weightLevel.light)
                element.transform.Find("WeightImage").GetComponent<Image>().sprite = lightWeight;
            //}
        }
    }

    void SetTrolley()
    {
        //Colocar cada alimento dnd esta en e status
        //Para los tres pisos
        //Recorrer los hijos y comprobar si en sus indices hay un aimento
        //Si hay alimento, instanciar el elemento como hijo de ese
        //Poner el sprite del food
        int[] index;
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            index = layer2.transform.GetChild(i).GetComponent<TrolleyDropField>().GetIndexes();
            //Debug.Log(trolleyStatus[index[1], index[0], 0]);
            if (GameManager.GetInstance().trolleyStatus[index[1], index[0]])
            {
                GameObject element = Instantiate(TrolleyElementPrefab);
                element.GetComponent<RectTransform>().position = layer2.transform.GetChild(i).GetComponent<RectTransform>().position;
                //element.transform.localScale = new Vector3(1.08f, 1.08f, 1.08f);
                element.transform.SetParent(layer2.transform.GetChild(i), true);
                element.transform.localScale = new Vector3(1f, 1f, 1f);
                element.GetComponent<TrolleyDragAndDrop>().upperParent = this.gameObject.GetComponent<Canvas>();
                element.GetComponent<TrolleyDragAndDrop>().canvas = this.gameObject.GetComponent<Canvas>();
                element.GetComponent<Food>().CopyFood(GameManager.GetInstance().trolleyStatus[index[1], index[0]]);
                element.transform.Find("FoodImage").GetComponent<Image>().sprite = element.GetComponent<Food>().sprite;
                //Hardness
                if (element.GetComponent<Food>().hardness == Food.hardnessLevel.hard)
                    element.transform.Find("FrameImage").GetComponent<Image>().sprite = grossFrame;
                else if (element.GetComponent<Food>().hardness == Food.hardnessLevel.mid)
                    element.transform.Find("FrameImage").GetComponent<Image>().sprite = midFrame;
                else if (element.GetComponent<Food>().hardness == Food.hardnessLevel.fragile)
                    element.transform.Find("FrameImage").GetComponent<Image>().sprite = lightFrame;
                //Weight
                if (element.GetComponent<Food>().weight == Food.weightLevel.heavy)
                    element.transform.Find("WeightImage").GetComponent<Image>().sprite = heavyWeight;
                else if (element.GetComponent<Food>().weight == Food.weightLevel.mid)
                    element.transform.Find("WeightImage").GetComponent<Image>().sprite = midWeight;
                else if (element.GetComponent<Food>().weight == Food.weightLevel.light)
                    element.transform.Find("WeightImage").GetComponent<Image>().sprite = lightWeight;
                trolley[index[1], index[0]] = element;
            }
        }
    }

    void SaveTrolley()
    {
        //Para cada capa
        //Recorrer los hijos y ver si tienen hijo (aka hay alimento)
        //Si hay hijo, coger sus indices y guardar su compeonet food en el sitio correspondiente de array

        int[] index;
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            index = layer2.transform.GetChild(i).GetComponent<TrolleyDropField>().GetIndexes();
            if (layer2.transform.GetChild(i).childCount > 0)
            {
                Debug.Log(layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName);

                switch (layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().category)
                {
                    case Food.Category.bakery:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().bakeryFoods[foodManager.GetComponent<FoodResourcesManager>().bakeryFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    case Food.Category.fruit:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().fruitsFoods[foodManager.GetComponent<FoodResourcesManager>().fruitsFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    case Food.Category.legume:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().legumeFoods[foodManager.GetComponent<FoodResourcesManager>().legumeFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    case Food.Category.fridge:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().fridgeFoods[foodManager.GetComponent<FoodResourcesManager>().fridgeFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    case Food.Category.fish:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().fishFoods[foodManager.GetComponent<FoodResourcesManager>().fishFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    case Food.Category.perfumery:
                        GameManager.GetInstance().trolleyStatus[index[1], index[0]] = foodManager.GetComponent<FoodResourcesManager>().perfumeryFoods[foodManager.GetComponent<FoodResourcesManager>().perfumeryFoods.FindIndex(s => s.GetComponent<Food>().foodName == layer2.transform.GetChild(i).transform.GetChild(0).GetComponent<Food>().foodName)];
                        break;
                    default:
                        break;
                }
                //Comparar color para guardar su status en GM
                //switch (trolley[index[1], index[0]].GetComponent<Food>().trolleyStatus)
                //{
                //    case Food.positionStatus.good:
                //        GameManager.GetInstance().numElementsCorrectPositionTrolley++;
                //        break;
                //    case Food.positionStatus.moderate:
                //        GameManager.GetInstance().numElementsModeratePositionTrolley++;
                //        break;
                //    case Food.positionStatus.wrong:
                //        GameManager.GetInstance().numElementsWrongPositionTrolley++;
                //        break;
                //}

            }
            else
            {
                GameManager.GetInstance().trolleyStatus[index[1], index[0]] = null;
            }
        }
    }

    public void OnClickNext()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        if (newFoodParent.transform.childCount == 0)
        {
            SaveTrolley();
            GameManager.GetInstance().pickedItems = new List<Food>();
            EventManager.OnSaveTimer();
            lvlLoader.LoadNextLevel("Maze_Test2");
        }

    }

    private void EvaluateTrolley()
    {
        for (int col = 0; col < trolley.GetLength(0); ++col)
        {
            EvaluateColumn(col);
        }
    }

    /// <summary>
    /// Evaluate the column with the given indexJ.
    /// If the top food exists, check if the middle food is fragile and the above food is mid or heavy weight.
    /// If the middle food exists, check if the bottom food is fragile and the middle food is mid or heavy weight.
    /// </summary>
    /// <param name="indexJ">Index of the column to evaluate.</param>
    /// 
    public void EvaluateColumn(int indexJ)
    {
        int topWeight = 0;
        for (int row = 2; row >= 0; --row)
        {
            if (!trolley[indexJ, row]) break;
            topWeight = EvaluateWeight(indexJ, row);
            Food.positionStatus itemStatus = EvaluateRules(topWeight, (int)trolley[indexJ, row].GetComponent<Food>().hardness);
            Color newColor = EvaluateColor(itemStatus);
            newColor.a = 0.65f;
            trolley[indexJ, row].GetComponent<TrolleyDragAndDrop>().statusImage.color = newColor;
            trolley[indexJ, row].GetComponent<Food>().trolleyStatus = itemStatus;
        }
    }

    private int EvaluateWeight(int column, int row)
    {
        int accumulate = 0;
        for (int r = row - 1; r >= 0; --r)
        {
            if (!trolley[column, r]) break;
            accumulate += (int)trolley[column, r].GetComponent<Food>().weight + 1;
        }
        return accumulate;
    }

    private Food.positionStatus EvaluateRules(int topWeight, int hardness)
    {
        if (hardness == (int)Food.hardnessLevel.fragile)
        {
            if (topWeight == 2)
            {
                return Food.positionStatus.moderate;
            }
            else if (topWeight > 2)
            {
                return Food.positionStatus.wrong;
            }
            Debug.Log($"Fragile TopWeight: {topWeight}");
        }
        if (hardness == (int)Food.hardnessLevel.mid)
        {
            if (topWeight == 3)
            {
                return Food.positionStatus.moderate;
            }
            else if (topWeight > 3)
            {
                return Food.positionStatus.wrong;
            }
        }
        if (hardness == (int)Food.hardnessLevel.hard)
        {
            if (topWeight == 4)
            {
                return Food.positionStatus.moderate;
            }
            else if (topWeight > 4)
            {
                return Food.positionStatus.wrong;
            }
        }
        return Food.positionStatus.good;
    }

    private Color EvaluateColor(Food.positionStatus status)
    {
        return status switch
        {
            Food.positionStatus.good => Color.green,
            Food.positionStatus.moderate => Color.yellow,
            Food.positionStatus.wrong => Color.red,
            _ => Color.white,
        };
    }
}
