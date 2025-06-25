using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesListener : MonoBehaviour
{

    public struct Slide
    {
        public int direction;
        public int isCorrect;
        public float time;

        public override readonly string ToString()
        {
            return $"({direction}, {isCorrect}, {time})";
        }
    }

    public struct Impact
    {
        public int ObstacleIndex;
        public int ObstacleType;
        public float PosX;
        public int RemainingLives;
        public float Time;

        public override readonly string ToString()
        {
            return $"({ObstacleIndex}, {ObstacleType}, {PosX}, {RemainingLives}, {Time})";
        }
    }

    [SerializeField] private TimerAux timerAux;
    private int timerIndex;
    private List<Slide> slides = new List<Slide>();
    private List<Impact> impacts = new List<Impact>();


    void Awake()
    {
        timerIndex = timerAux.InitTimer();
        timerAux.StartTimer(timerIndex);
    }

    void OnDestroy()
    {
        timerAux.StopTimer(timerIndex);
        DataStorage.Instance.trolleyDodgeData.TrolleyDodgeDuration = timerAux.elapsedTime[timerIndex];
        DataStorage.Instance.trolleyDodgeData.ScenesFinished = PlayedMinigamesToString();
        DataStorage.Instance.trolleyDodgeData.TrolleyDodgeSlides = ListToString(slides);
        DataStorage.Instance.trolleyDodgeData.TrolleyImpacts = ListToString(impacts);
        
    }

    public float GetElapsedTime()
    {
        return timerAux.elapsedTime[timerIndex];
    }

    public void AddSlide(int direction, int isCorrect)
    {
        Slide slide = new Slide
        {
            direction = direction,
            isCorrect = isCorrect,
            time = timerAux.elapsedTime[timerIndex]
        };

        slides.Add(slide);
    }

    public void AddImpact(int obstacleIndex, int obstacleType, float posX, int remainingLives)
    {
        Impact impact = new Impact
        {
            ObstacleIndex = obstacleIndex,
            ObstacleType = obstacleType,
            PosX = posX,
            RemainingLives = remainingLives,
            Time = timerAux.elapsedTime[timerIndex]
        };

        impacts.Add(impact);
    }

    private string ListToString<T>(List<T> ts, string brackets = "[]")
    where T : struct
    {
        List<string> list = new List<string>();
        foreach (var item in ts)
        {
            list.Add(item.ToString());
        }

        return brackets[0] + string.Join(", ", list) + brackets[1];
    }

    private string PlayedMinigamesToString()
    {
        List<int> playedMinigames = new List<int>();

        for (int i = 0; i < GameManager.GetInstance().minigamesSpentTime.Length; ++i)
        {
            if (GameManager.GetInstance().minigamesSpentTime[i] > 0)
            {
                playedMinigames.Add(i);
            }
        }
        
        return ListToString(playedMinigames, "[]");
    }
}
