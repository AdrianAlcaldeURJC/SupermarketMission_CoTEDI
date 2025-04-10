using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{

    [SerializeField] private TMP_Text text;

    private float timeToDisplay = 0.0f;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            EventManager.OnTimerStop();
            return;
        }
        else
        {
            timeToDisplay += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
            text.text = timeSpan.ToString(@"mm\:ss\:ff");
            //Debug.Log(timeSpan);
        }
    }

    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.SaveTimer += EventManagerSaveTimer;
    }    
    
    private void OnDisable()
    {
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.SaveTimer -= EventManagerSaveTimer;
    }

    private void EventManagerOnTimerStart()
    {
        isRunning = true;
    }

    private void EventManagerOnTimerStop()
    {
        isRunning = false;
    }

    private void EventManagerSaveTimer()
    {
        isRunning = false;
        GameManager.GetInstance().currentSpentTime += timeToDisplay;
        switch (SceneManager.GetActiveScene().name)
        {
            case "GroceryList":
                GameManager.GetInstance().groceryListSpentTime += timeToDisplay;
                break;
            case "SupermarketMap":
                GameManager.GetInstance().SupermarketMapSpentTime += timeToDisplay;
                break;
            case "TrolleyScene 1":
                GameManager.GetInstance().trolleySpentTime += timeToDisplay;
                break;
            case "SupermarketSection":
                ClassifyMinigameTime();
                break;
        }
    }

    private void ClassifyMinigameTime()
    {
        switch (GameManager.GetInstance().actualSection)
        {
            case Food.Category.bakery:
                GameManager.GetInstance().minigamesSpentTime[0] += timeToDisplay;
                break;
            case Food.Category.fruit:
                GameManager.GetInstance().minigamesSpentTime[1] += timeToDisplay;
                break;
            case Food.Category.legume:
                GameManager.GetInstance().minigamesSpentTime[2] += timeToDisplay;
                break;
            case Food.Category.fridge:
                GameManager.GetInstance().minigamesSpentTime[3] += timeToDisplay;
                break;
            case Food.Category.fish:
                GameManager.GetInstance().minigamesSpentTime[4] += timeToDisplay;
                break;
            case Food.Category.perfumery:
                GameManager.GetInstance().minigamesSpentTime[5] += timeToDisplay;
                break;
            default:
                break;
        }
    }
}
