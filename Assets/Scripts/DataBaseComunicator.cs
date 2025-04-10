using System.Collections;
using System.Collections.Generic;
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
          ""table"":""PlayerScores"",
          ""data"":{ " +scores+ "}"+
        "}";

        //string data = @"{
        //  ""username"":""TFMMGP2024"", ""password"":""2024TFMSupermercadoPC"",
        //  ""table"":""test"",
        //  ""data"":{""name"": ""name1"", ""start"": ""2023-12-01 00:01:00"", ""end"": ""2023-11-10 00:01:00""}
        //}";


        StartCoroutine(SendInsertPostRequest(data));
    }


    IEnumerator SendInsertPostRequest(string data)
    {
        //Construye JSON para la petición REST
        
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

    //"name": "FR", 
    //                    "age": "12",
    //                    "gender": "Masculino",
    //                    "totalTime": "351,1779",
    //                    "clasifyListTime": "66,65614",
    //                    "identifyMapTime": "25,46741",
    //                    "organizeTrolleyTime": "58,87215",
    //                    "bakeryMGTime": "24,00925",
    //                    "fruitsMGTime": "13,56796",
    //                    "legumesMGTime": "48,55979",
    //                    "fridgeMGTime": "18,00876",
    //                    "fishMGTime": "26,01155",
    //                    "perfumeryMGTime": "70,02486",
    //                    "correctPickedItems": "20",
    //                    "wrongPickedItems": "1",
    //                    "correctPositionTroley": "69",
    //                    "moderatePositionTrolley": "1",
    //                    "wrongPositionTrolley": "0",
    //                    "date": "2024-07-08 20:31:54"
}
