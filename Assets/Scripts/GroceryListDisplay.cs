using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroceryListDisplay : MonoBehaviour
{
    [SerializeField] private GameObject prefabFoodItemList;
    [SerializeField] private GameObject bakeryScroll;
    [SerializeField] private GameObject fruitsScroll;
    [SerializeField] private GameObject legumesScroll;
    [SerializeField] private GameObject fridgeScroll;
    [SerializeField] private GameObject fishScroll;
    [SerializeField] private GameObject perfumeryScroll;

    // Start is called before the first frame update
    void Awake()
    {
        DisplayGroceyList();
    }

    void DisplayGroceyList()
    {
        this.FillSectionList(GameManager.GetInstance().bakeryFoodList, bakeryScroll);
        this.FillSectionList(GameManager.GetInstance().fruitFoodList, fruitsScroll);
        this.FillSectionList(GameManager.GetInstance().legumeFoodList, legumesScroll);
        this.FillSectionList(GameManager.GetInstance().fridgeFoodList, fridgeScroll);
        this.FillSectionList(GameManager.GetInstance().fishFoodList, fishScroll);
        this.FillSectionList(GameManager.GetInstance().perfumeryFoodList, perfumeryScroll);
    }

    void FillSectionList(List<Food> foodList, GameObject sectionScroll)
    {
        //Por cada lista, ir creando los eleemntos de la lista
        for (int i = 0; i < foodList.Count; i++)
        {
            GameObject g = Instantiate(prefabFoodItemList);
            //Poner como padre la lista correspondiente
            g.transform.SetParent(sectionScroll.transform);
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            //Comprobar si el elemento ya ha sido cogido para tachar el texto
            g.GetComponentInChildren<TMP_Text>().text = foodList[i].foodName;
            if (foodList[i].alreadyTaken)
                g.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Strikethrough;
            //Quitar drag and drop script
            g.GetComponent<DragAndDropGroceryList>().enabled = false;
        }
    }

    void DeleteChildsScrollSection(GameObject sectionScroll)
    {
        int childCount = sectionScroll.transform.childCount;
        for (int i=0; i<childCount; i++)
        {
            Debug.Log("Estoy borradno "+ sectionScroll.transform.GetChild(0).gameObject);
            GameObject go = sectionScroll.transform.GetChild(0).gameObject;
            go.transform.parent = null;
            Destroy(go);
        }
    }

    public void RefreshSection()
    {
        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                this.DeleteChildsScrollSection(bakeryScroll);
                this.FillSectionList(GameManager.GetInstance().bakeryFoodList, bakeryScroll);
                break;
            case Food.Category.fruit:
                this.DeleteChildsScrollSection(fruitsScroll);
                this.FillSectionList(GameManager.GetInstance().fruitFoodList, fruitsScroll);
                break;
            case Food.Category.legume:
                this.DeleteChildsScrollSection(legumesScroll);
                this.FillSectionList(GameManager.GetInstance().legumeFoodList, legumesScroll);
                break;
            case Food.Category.fridge:
                this.DeleteChildsScrollSection(fridgeScroll);
                this.FillSectionList(GameManager.GetInstance().fridgeFoodList, fridgeScroll);
                break;
            case Food.Category.fish:
                this.DeleteChildsScrollSection(fishScroll);
                this.FillSectionList(GameManager.GetInstance().fishFoodList, fishScroll);
                break;
            case Food.Category.perfumery:
                this.DeleteChildsScrollSection(perfumeryScroll);
                this.FillSectionList(GameManager.GetInstance().perfumeryFoodList, perfumeryScroll);
                break;
            default:
                
                break;
        }
    }
}
