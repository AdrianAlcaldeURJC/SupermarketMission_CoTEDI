using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameObjects
{
    public List<GameObject> list;
}

[System.Serializable]
public class Categories
{
    public List<Food.Category> list;
}

public class DragAndDropManager : MonoBehaviour
{
    private DropField[] dropFields = new DropField[6];
    [SerializeField] private LevelLoader lvlLoader;
    [SerializeField] private ExplanationCanvas explanationCanvas;
    [SerializeField] private Canvas notificationCanvas;
    [SerializeField] private GameObject[] sections = new GameObject[6];
    [SerializeField] private Sprite[] sectionImages = new Sprite[6];
    [SerializeField] private List<GameObjects> mazeMaps = new List<GameObjects>(3);
    [SerializeField] private List<Categories> sectionCategories = new List<Categories>(3);
    private List<string> sectionsAvailability = new List<string> { "bakery", "fruits", "legumes", "fridge", "fish", "perfumery" };


    void Start()
    {
        notificationCanvas.gameObject.SetActive(false);

        explanationCanvas.SetTextChecking("ExplicationCanvas", "SupermarketMap_1", 1);
        InitMaze();
        InitSections();
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
                if (dropFields[i].transform.GetChild(0).GetComponent<DragAndDrop>().getValue() != this.dropFields[i].GetValue())
                {
                    correct = false;
                    notificationCanvas.gameObject.SetActive(true);
                }
            }
            else
            {
                correct = false;
                notificationCanvas.gameObject.SetActive(true);
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
            EventManager.OnSaveTimer();
            lvlLoader.LoadNextLevel("Maze_Test2");
        }
    }

    void MapGeneration()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            int rand = Random.Range(0, sectionsAvailability.Count);
            switch (sectionsAvailability[rand])
            {
                case "bakery":
                    sections[i].GetComponent<Image>().sprite = sectionImages[0];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.bakery);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.bakery;
                    MapListener.Instance.mapLayout.Add(1);
                    break;
                case "fruits":
                    sections[i].GetComponent<Image>().sprite = sectionImages[1];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.fruit);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fruit;
                    MapListener.Instance.mapLayout.Add(0);
                    break;
                case "legumes":
                    sections[i].GetComponent<Image>().sprite = sectionImages[2];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.legume);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.legume;
                    MapListener.Instance.mapLayout.Add(2);
                    break;
                case "fridge":
                    sections[i].GetComponent<Image>().sprite = sectionImages[3];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.fridge);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fridge;
                    MapListener.Instance.mapLayout.Add(3);
                    break;
                case "fish":
                    sections[i].GetComponent<Image>().sprite = sectionImages[4];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.fish);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.fish;
                    MapListener.Instance.mapLayout.Add(4);
                    break;
                case "perfumery":
                    sections[i].GetComponent<Image>().sprite = sectionImages[5];
                    sections[i].GetComponent<DropField>().SetValue(Food.Category.perfumery);
                    GameManager.GetInstance().sectionDistribution[i] = Food.Category.perfumery;
                    MapListener.Instance.mapLayout.Add(5);
                    break;
            }
            sectionsAvailability.RemoveAt(rand);
        }
        MapListener.Instance.mapLayout.Add(6);
    }

    void InitSections()
    {
        Categories categories = sectionCategories[GameManager.GetInstance().mazeID / 3];
        int i = 0;
        foreach (Food.Category category in categories.list)
        {
            sections[i].GetComponent<Image>().sprite = sectionImages[(int)category];
            sections[i].GetComponent<DropField>().SetValue(category);
            GameManager.GetInstance().sectionDistribution[i] = category;
            MapListener.Instance.mapLayout.Add((int)category);

            ++i;
        }
        MapListener.Instance.mapLayout.Add(6);
    }

    void InitMaze()
    {
        int rand1 = UnityEngine.Random.Range(0, mazeMaps.Count);
        int rand2 = UnityEngine.Random.Range(0, mazeMaps[rand1].list.Count);

        GameObject maze = mazeMaps[rand1].list[rand2];
        GameManager.GetInstance().mazeID = maze.GetComponent<ObstaclesManagerSingleton>().mazeID;

        GameObject mazeSpawned = Instantiate(maze);
        mazeSpawned.GetComponent<ObstaclesManagerSingleton>().destroyOnLoad = true;
        mazeSpawned.transform.Find("Sections").gameObject.SetActive(false);
        mazeSpawned.transform.position = new Vector3(20, 0, -20);
        mazeSpawned.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

}
