using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaclesManagerSingleton : MonoBehaviour
{
    [SerializeField] public int mazeID;
    [SerializeField] public TimerAux timerAux;
    public static ObstaclesManagerSingleton Instance;
    private DataStorage dataStorage;
    public int timerID;
    public bool destroyOnLoad = false;

    private void Awake()
    {
        if (!destroyOnLoad)
        {
            if (Instance != null)
            {
                Debug.Log("Removing myself");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }    
    }

    private void Start()
    {
        dataStorage = DataStorage.Instance;
        dataStorage.mazeMapData.MazeID = mazeID;
        timerID = timerAux.InitTimer();
        timerAux.StartTimer(timerID);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        if (scene.name == "Maze_Test2")
        {
            timerAux.RestartAllTimers();
        }
        else
        {
            timerAux.StopAllTimers();
        }

        if (scene.name == "ObstaclesGame")
        {
            DataStorage.Instance.mazeMapData.MazeDuration = timerAux.elapsedTime[timerID];
        }
    }
}   
