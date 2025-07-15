using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private DataBaseComunicator dataBaseCommunicator;
    void Start()
    {
        // Load the JSON from the database
        List<string> jsons = new List<string>();
        DataStorage.Instance.gameData.GameEndTime = DateTime.Now.ToString("M/d/yyyy/hh:mm:ss"); // Set the end time
        DataStorage.Instance.SaveCombinedJsonToFile();
        for (int i = 0; i < DataStorage.Instance.minigamesData.Count; i++)
        {
            jsons.Add(DataStorage.Instance.GetCombinedJson(i));    // Get JSON data
            dataBaseCommunicator.SendInsertRequest(jsons[i]);        // Send JSON data
        }

        DataStorage.Instance.OnEndData();                   // Clear data
    
    }
}
