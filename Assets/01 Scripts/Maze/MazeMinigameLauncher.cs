using Unity.VisualScripting;
using UnityEngine;

public class MazeMinigameLauncher : MonoBehaviour
{
    [SerializeField] Food.Category foodCategory;
    private LevelLoader lvlLoader;

    public void Start()
    {
        if (lvlLoader == null)
        {
            lvlLoader = FindObjectOfType<LevelLoader>();
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
        GetComponent<Collider>().enabled = false;
        LoadMinigame();
    }

}
