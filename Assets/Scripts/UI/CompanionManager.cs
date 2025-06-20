using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;


public class CompanionManager : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent stringEventLocalization;
    [SerializeField] private LocalizedString[] localizedStrings;
    private List<bool> minigamesToPlay;
    private List<int> minigamesPlayed;
    private int remainingMinigames;
    private int maxMinigames;

    private void Awake()
    {
        InitMinigamesToPlay();
        InitMinigamesPlayed();
        InitRemainingMinigames();
        InitMaxMinigames();
    }

    private void Start()
    {
        SetNextText();    
    }

    private void InitMinigamesToPlay()
    {
        GameManager gm = GameManager.GetInstance();

        minigamesToPlay = new List<bool>
        {
            gm.bakeryFoodList?.Count != 0,
            gm.fruitFoodList?.Count != 0,
            gm.legumeFoodList?.Count != 0,
            gm.fridgeFoodList?.Count != 0,
            gm.fishFoodList?.Count != 0,
            gm.perfumeryFoodList?.Count != 0,
        };
    }

    private void InitMinigamesPlayed()
    {
        GameManager gm = GameManager.GetInstance();
        minigamesPlayed = new List<int>();
        for (int i = 0; i < 6; ++i)
        {
            if (minigamesToPlay[i] == true)
            {
                minigamesPlayed.Add(gm.minigamesSpentTime[i] > 0.0f ? 1 : 0);
            }
            else
            {
                minigamesPlayed.Add(-1);
            }
        }
    }

    private void InitRemainingMinigames()
    {
        int count = 0;
        foreach (int played in minigamesPlayed)
        {
            count += played == 0 ? 1 : 0;
        }
        remainingMinigames = count;
    }

    private void InitMaxMinigames()
    {
        int count = 0;
        foreach (bool playable in minigamesToPlay)
        {
            count += playable == true ? 1 : 0;
        }
        maxMinigames = count;
    }

    private void SetNextText()
    {
        int stringIndex = -1;

        if (remainingMinigames == maxMinigames)
        {
            stringIndex = 0;
        }
        else if (remainingMinigames != 1)
        {
            var dict1 = new Dictionary<string, int> { { "remainingMinigames", remainingMinigames } };
            var dict2 = new Dictionary<string, int> { { "maxMinigames", maxMinigames } };

            localizedStrings[1].Arguments = new object[] { dict1, dict2 };
            stringIndex = 1;
        }
        else
        {
            stringIndex = 2;
        }

        stringEventLocalization.StringReference = localizedStrings[stringIndex];

    }
    


}
