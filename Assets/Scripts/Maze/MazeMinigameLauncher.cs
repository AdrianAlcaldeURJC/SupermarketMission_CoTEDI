using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMinigameLauncher : MonoBehaviour
{
    [SerializeField] Food.Category foodCategory;
    [SerializeField] LevelLoader lvlLoader;


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


}
