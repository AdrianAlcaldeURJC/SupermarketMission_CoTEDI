using UnityEngine;

public class MazeMinigameLauncher : MonoBehaviour
{
    [SerializeField] Food.Category foodCategory;
    [SerializeField] LevelLoader lvlLoader;

    public void Start()
    {
        if (lvlLoader == null)
        {
            lvlLoader = FindObjectOfType<LevelLoader>();
        }
    }

    public void LoadMinigame()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        GameManager.GetInstance().actualSection = foodCategory;
        if (foodCategory == Food.Category.cashier)
            lvlLoader.LoadNextLevel("ObstaclesGame");
        else
            lvlLoader.LoadNextLevel("SupermarketSection");

        // Save map picked order
        GameManager.GetInstance().CurrentMinigame = foodCategory;
    }

    private void OnTriggerEnter(Collider other)
    {
        LoadMinigame();
        GetComponent<Collider>().enabled = false;
    }

}
