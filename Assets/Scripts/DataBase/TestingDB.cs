using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestingDB : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    DataBaseComunicator db;
    void Start()
    {
        DataStorage.Instance.userData.UserID = 1;
        DataStorage.Instance.userData.Name = "TestName";
        DataStorage.Instance.userData.CreationData = "01/01/0001";
        DataStorage.Instance.userData.Age = 7;
        DataStorage.Instance.userData.Gender = 0;
        DataStorage.Instance.userData.Country = "España";
        DataStorage.Instance.userData.UserAux1 = "Aux1";
        DataStorage.Instance.userData.UserAux2 = "Aux2";

        string JSON = DataStorage.Instance.GetCombinedJsons(0);
        string excapedJSON = JSON.Replace("\r\n", "\\r\\n");
        excapedJSON = excapedJSON.Replace(@"""", @"\""");

        //excapedJSON = excapedJSON.Replace("\n", "\\n");

        db.SendInsertRequest(excapedJSON);
    }
       
}
