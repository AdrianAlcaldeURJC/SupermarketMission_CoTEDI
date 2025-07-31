using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeMinigameLauncher : MonoBehaviour
{
    [SerializeField] Food.Category foodCategory;
    private LevelLoader lvlLoader;
    private bool isColliding = false;

    public void Start()
    {
        if (lvlLoader == null)
        {
            lvlLoader = FindObjectOfType<LevelLoader>();
        }

        var foodCount = foodCategory switch
        {
            Food.Category.bakery => GameManager.GetInstance().bakeryFoodList.Count,
            Food.Category.fish => GameManager.GetInstance().fishFoodList.Count,
            Food.Category.fridge => GameManager.GetInstance().fridgeFoodList.Count,
            Food.Category.fruit => GameManager.GetInstance().fruitFoodList.Count,
            Food.Category.legume => GameManager.GetInstance().legumeFoodList.Count,
            Food.Category.perfumery => GameManager.GetInstance().perfumeryFoodList.Count,
            _ => 1,
        };

        if (foodCount == 0)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            SetCompleted();
            // gameObject.SetActive(false);
        }

    }

    public void LoadMinigame()
    {
        if (lvlLoader == null)
        {
            lvlLoader = FindObjectOfType<LevelLoader>();
        }

        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        GameManager.GetInstance().actualSection = foodCategory;
        if (foodCategory == Food.Category.cashier)
            lvlLoader.LoadNextLevel("ObstaclesGame");
        else
            lvlLoader.LoadNextLevel("SupermarketSection");

        // Save map picked
        GameManager.GetInstance().CurrentMinigame = foodCategory;

        // Save map pick order in database
        string pickOrder = DataStorage.Instance.mazeMapData.MazeMinigamePickOrder;
        if (pickOrder == "" || pickOrder == null)
        {
            pickOrder = $"({foodCategory})";
        }
        else
        {
            pickOrder = pickOrder.TrimEnd(")");
            pickOrder += $", {foodCategory})";
        }
        DataStorage.Instance.mazeMapData.MazeMinigamePickOrder = pickOrder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;

        StartCoroutine(Reset());

        if (!CheckAllSectionsVisited() && foodCategory == Food.Category.cashier)
        {
            GameObject.Find("Trolley").GetComponent<MazeMovement>().ReverseLastDirection();
            FindObjectsOfType<ExplanationCanvas>(true)[0].gameObject.SetActive(true);
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            SetCompleted();
            LoadMinigame();
        }

    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }

    private bool CheckAllSectionsVisited()
    {
        // I am so over
        List<BoxCollider> sections = transform.parent.transform.parent.transform.parent.gameObject.GetComponentsInChildren<BoxCollider>().ToList();

        int enabled = 0;
        foreach (var section in sections)
        {
            if (section.enabled == true)
            {
                enabled++;
            }
        }

        if (enabled > 1)
        {
            return false;
        }
        return true;
    }

    private void SetCompleted()
    {
        // Get parent (color highlight)
        transform.parent.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color = transform.parent.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color * 0.7f;

        // Get children's  
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>().color * 0.7f;
        }

        if (transform.childCount == 0)
        {
            transform.GetComponent<UnityEngine.UI.Image>().color = transform.gameObject.GetComponent<UnityEngine.UI.Image>().color * 0.7f;
        }
    }
    
}
