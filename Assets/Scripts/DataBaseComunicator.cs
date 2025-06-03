using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DataBaseComunicator : MonoBehaviour
{
    public void SendGetRequest(string data)
    {
        //string data = @"{
        //  ""username"":""TFMMGP2024"", ""password"":""2024TFMSupermercadoPC"",
        //  ""table"":""test"",
        //  ""filter"":{""name"": ""name1"" }
        //}";

        StartCoroutine(SendGetPostRequest(data));
    }

    IEnumerator SendGetPostRequest(string data)
    {
        //Construye JSON para la petición REST


        //Construye UnityWebRequest para enviar solicitud 
        UnityWebRequest request = UnityWebRequest.Post("https://tfvj.etsii.urjc.es/get", data, "application/json");

        // Configurar la solicitud (headers, etc.) si es necesario
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Verificar si hay errores
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            // La solicitud fue exitosa, puedes acceder a la respuesta
            Debug.Log("Respuesta: " + request.downloadHandler.text);
        }
    }

    public void SendInsertRequest(string scores)
    {
        string data = @"{
          ""username"":""TFMMGP2024"", ""password"":""2024TFMSupermercadoPC"",
          ""table"":""SupermarketMissionFull"",
          ""data"": " + scores + " " +
        "}";

        StartCoroutine(SendInsertPostRequest(data));
    }

    IEnumerator SendInsertPostRequest(string data)
    {

        //Construye UnityWebRequest para enviar solicitud 
        UnityWebRequest request = UnityWebRequest.Post("https://tfvj.etsii.urjc.es/insert", data, "application/json");

        // Configurar la solicitud (headers, etc.) si es necesario
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Verificar si hay errores
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            // La solicitud fue exitosa, puedes acceder a la respuesta
            Debug.Log("Respuesta: " + request.downloadHandler.text);
        }
    }



    public void ProcessSessionGamesCount()
    {
        string data = @"{
        ""username"":""TFMMGP2024"",
        ""password"":""2024TFMSupermercadoPC"",
        ""table"":""SupermarketMissionFull"",
        ""filter"":{""userID"": """ + DataStorage.Instance.userData.UserID + @"""}
        }";

        StartCoroutine(SendCountPostRequest(data));
    }

    IEnumerator SendCountPostRequest(string data)
    {
        UnityWebRequest request = UnityWebRequest.Post("https://tfvj.etsii.urjc.es/get", data, "application/json");
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Session Count Response: " + request.downloadHandler.text);
            // Process the response to extract the session count
            ProcessSessionCountResponse(request.downloadHandler.text);
        }
    }

    private void ProcessSessionCountResponse(string response)
    {
        // Parse the JSON response to extract the count
        try
        {
            // Assuming the response is a JSON object with a "count" field


            Debug.Log("Number of sessions: " + response); // Process the response
            GetPostCombinedData combinedData = JsonConvert.DeserializeObject<GetPostCombinedData>(response);

            int sessionID = -1;
            int gameID = -1;
            foreach (CombinedData data in combinedData.data)
            {
                if (data.SessionID > sessionID && data.UserID == DataStorage.Instance.userData.UserID)
                {
                    sessionID = data.SessionID;
                    gameID = data.GameID;
                }
            }
            DataStorage.Instance.sessionData.SessionID = sessionID + 1;
            DataStorage.Instance.gameData.GameID = gameID + 1;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing session count response: " + e.Message);
        }
    }



}
