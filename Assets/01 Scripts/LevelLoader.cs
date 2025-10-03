using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1;
    

    public void LoadNextLevel(string nameScene)
    {
        StartCoroutine(LoadLevel(nameScene));
    }

    IEnumerator LoadLevel(string scene)
    {

        if (scene == "NextPlayerScene")
        {
            string currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "StartingCinematic":
                    GameManager.GetInstance().nextSceneName = "GroceryList";
                    break;
                case "GroceryList":
                    GameManager.GetInstance().nextSceneName = "SupermarketMap";
                    break;
                case "SupermarketMap":
                    GameManager.GetInstance().nextSceneName = "Maze_Test2";
                    break;
                case "TrolleyScene 1":
                    GameManager.GetInstance().nextSceneName = "Maze_Test2";
                    break;
            }
        }

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(scene);

        // Special case for obstaclesGame
        if (scene == "ObstaclesGame")
        {
            GameObject maze = FindObjectOfType<ObstaclesManagerSingleton>().gameObject;
            GameObject trolley = FindObjectOfType<MazeMovement>().gameObject;

            maze.SetActive(false);
            trolley.SetActive(false);
        }
    }

    public void StartFakeTransition()
    {
        StartCoroutine(FakeLoading());
    }
    IEnumerator FakeLoading()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
    }

    public void LoadNextSceneFromNextPlayerScene()
    {
        LoadNextLevel(GameManager.GetInstance().nextSceneName);
    }

}
