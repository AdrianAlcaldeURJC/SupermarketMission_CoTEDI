using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupermarketMap : MonoBehaviour
{
    [SerializeField]
    private LevelLoader lvlLoader;
    private ExplanationCanvas explanationCanvas;

    [SerializeField]
    private GameObject groceryListCanvas;

    [SerializeField] GameObject[] sections = new GameObject[7];
    [SerializeField] private Sprite[] sectionImages = new Sprite[6];
    [SerializeField] MinigameListener minigameListener;
    float StartTime = 0f;

    private MapSelectorListener mapSelectorListener;

    // Start is called before the first frame update
    void Start()
    {
        lvlLoader = FindObjectOfType<LevelLoader>();
        explanationCanvas = FindObjectOfType<ExplanationCanvas>();

        explanationCanvas.SetTextChecking("ExplicationCanvas", "SupermarketMapSelection_1", 2);

        groceryListCanvas.SetActive(false);
        MapGeneration();

        mapSelectorListener = FindObjectOfType<MapSelectorListener>();
    }

    public void OnClickSection(Food.Category category)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        GameManager.GetInstance().actualSection = category;
        if (category == Food.Category.cashier)
            lvlLoader.LoadNextLevel("ObstaclesGame");
        else
            lvlLoader.LoadNextLevel("SupermarketSection");

        // Save map picked order
        mapSelectorListener.AddPickedMap((int)category);
        GameManager.GetInstance().CurrentMinigame = category;
    }

    public void ShowAndHideList()
    {   
        if (groceryListCanvas.gameObject.activeSelf && minigameListener != null)
        {
            minigameListener.AddListOpened(
                minigameListener.GetListOpenedIndex(),
                StartTime,
                minigameListener.GetElapsedTime());
        }

        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        groceryListCanvas.SetActive(!groceryListCanvas.activeSelf);
    }

    void MapGeneration()
    {
        //Recorrer las sections
        //Asignar un aaleatoria de la lista de secciones y quitarla de la lista
        //Ponerle la imagen correspondiente
        //Ponerle el valor correspodiente al dropfield
        Food.Category[] order = GameManager.GetInstance().sectionDistribution;

        for (int i = 0; i < order.Length; i++)
        {            
            switch (order[i])
            {
                case Food.Category.bakery:
                    sections[i].GetComponent<Image>().sprite = sectionImages[0];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.bakery); });
                    break;
                case Food.Category.fruit:
                    sections[i].GetComponent<Image>().sprite = sectionImages[1];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.fruit); });
                    break;
                case Food.Category.legume:
                    sections[i].GetComponent<Image>().sprite = sectionImages[2];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.legume); });
                    break;
                case Food.Category.fridge:
                    sections[i].GetComponent<Image>().sprite = sectionImages[3];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.fridge); });
                    break;
                case Food.Category.fish:
                    sections[i].GetComponent<Image>().sprite = sectionImages[4];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.fish); });
                    break;
                case Food.Category.perfumery:
                    sections[i].GetComponent<Image>().sprite = sectionImages[5];
                    sections[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.perfumery); });
                    break;
                default:
                    Debug.Log("No existe esa categoria: " + order[i]);
                    break;
            }
            
        }
        sections[6].GetComponent<Button>().onClick.AddListener(delegate { OnClickSection(Food.Category.cashier); });

    }
}
