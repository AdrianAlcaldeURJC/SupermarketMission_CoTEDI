using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Localization.Settings;

[Serializable]
public class CombinedData
{
    // UserData properties
    public string UserID;
    public string Name;
    public string CreationDate;
    public int Age;
    public string Gender;
    public string Country;
    public string UserAux1;
    public string UserAux2;

    // SessionData properties
    public int SessionID;
    public int NumGames;
    public string SessionStartTime;
    public string SessionEndTime;
    public int Platform;
    public string SessionAux1;
    public string SessionAux2;

    // GameData properties
    public int GameID;
    public string GameStartTime;
    public string GameEndTime;
    public string GameAux1;
    public string GameAux2;

    // GroceryMapData properties
    public string ListDuration;
    public string MapDuration;
    public string GroceryListItems;
    public string GroceryDrops;
    public string GroceryClicks;
    public string MapOrder;
    public string MapDrops;
    public string MapClicks;
    public string MapPickOrder;
    public string DecisionTime;
    public string GroceryAux1;
    public string GroceryAux2;
    public string MapAux1;
    public string MapAux2;

    // MinigamesData properties
    public int Minigame;
    public float MinigameDuration;
    public string ColorsOpened;
    public string ColorsPicks;
    public string ShadowSkipped;
    public string ShadowPicks;
    public string ListOpened;
    public string AddedItems;
    public string TrolleyResult;
    public string TrolleyDrops;
    public string ColorsAux1;
    public string ColorsAux2;
    public string ShadowsAux1;
    public string ShadowsAux2;
    public string TrolleyAux1;
    public string TrolleyAux2;

    // TrolleyDodgeData properties
    public float TrolleyDodgeDuration;
    public string ScenesFinished;
    public string TrolleyDodgeSlides;
    public string TrolleyImpacts;
    public string TrolleyDodgeAux1;
    public string TrolleyDodgeAux2;
    public string TrolleyDodgeAux3;

}

[Serializable]
public class GetPostCombinedData
{
    public string result;
    public List<CombinedData> data;
}