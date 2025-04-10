using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour
{
    private DropField[] dropFields = new DropField[6];
    [SerializeField]
    private LevelLoader lvlLoader;
    [SerializeField] private ExplanationCanvas explanationCanvas;
    [SerializeField] private Canvas notificationCanvas;

    [SerializeField] private GameObject[] sections = new GameObject[6];
    [SerializeField] private Sprite[] sectionImages = new Sprite[6];
    private List<string> sectionsAvaliability = new List<string>{"bakery","fruits","legumes","fridge","fish","perfumery"};

    void Start()
    {
        notificationCanvas.gameObject.SetActive(false);
        explanationCanvas.SetTextChecking(1, "El segundo paso, es el reconocimiento del terreno. Identifiquemos en este mapa dónde se encuentra cada sección del supermercado. \n" +
                    "Arrastra el nombre de cada sección encima del icono que la representa.");

        MapGeneration();
        var num = FindObjectsOfType<DragAndDrop>().Length;
        dropFields = FindObjectsOfType<DropField>();
    }

    private bool CheckResults()
    {
        bool correct = true;
        for (int i = 0; correct && i < dropFields.Length; i++)
        {
            if (dropFields[i].transform.childCount > 0)
            {
                if (dropFields[i].transform.GetChild(0).GetComponent<DragAndDrop>().getValue() != this.dropFields[i].getValue()){
                    correct = false;
                    Debug.Log("Algo mal con " + this.dropFields[i].getValue());
                    notificationCanvas.gameObject.SetActive(true);
                    notificationCanvas.GetComponentInChildren<TMP_Text>().text = "Hay alguna sección mal identificada.";
                }
                else
                {
                    Debug.Log("Todo correcto con " + this.dropFields[i].getValue());
                }
            }
            else
            {
                Debug.Log("Algo mal con " + this.dropFields[i].getValue());
                correct = false;
                notificationCanvas.gameObject.SetActive(true);
                notificationCanvas.GetComponentInChildren<TMP_Text>().text = "Faltan secciones por identificar.";
            }
        }
        return correct;
    }

    public void OnClickCheck()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        var correct = this.CheckResults();
        if (correct)
        {
            Debug.Log("Muy bien");
            EventManager.OnSaveTimer();
            lvlLoader.LoadNextLevel("SupermarketMapSelection");
        }
        else
        {
            Debug.Log("Hay algo mal");
        }
    }

    void MapGeneration()
    {
        //Recorrer las sections
        //Asignar un aaleatoria de la lista de secciones y quitarla de la lista
        //Ponerle la imagen correspondiente
        //Ponerle el valor correspodiente al dropfield

        for(int i=0; i < sections.Length; i++)
        {
            int rand = Random.Range(0,sectionsAvaliability.Count);
            switch (sectionsAvaliability[rand])
            {
                case "bakery":
                    sections[i].GetComponent<Image>().sprite = sectionImages[0];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.bakery);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.bakery;
                    break;
                case "fruits":
                    sections[i].GetComponent<Image>().sprite = sectionImages[1];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.fruit);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fruit;
                    break;
                case "legumes":
                    sections[i].GetComponent<Image>().sprite = sectionImages[2];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.legume);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.legume;
                    break;
                case "fridge":
                    sections[i].GetComponent<Image>().sprite = sectionImages[3];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.fridge);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fridge;
                    break;
                case "fish":
                    sections[i].GetComponent<Image>().sprite = sectionImages[4];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.fish);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fish;
                    break;
                case "perfumery":
                    sections[i].GetComponent<Image>().sprite = sectionImages[5];
                    sections[i].GetComponent<DropField>().setValue(Food.Category.perfumery);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.perfumery;
                    break;
            }
            sectionsAvaliability.RemoveAt(rand);
        }
    }

}
