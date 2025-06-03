using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TestingDB : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    DataBaseComunicator db;
    void Start()
    {
        //DataStorage.Instance.userData.setData("name", 14, 1);

        DataStorage.Instance.groceryMapData.MapDrops = "[(1, 2, 3), (4, 5, 6)]";

        string JSON = DataStorage.Instance.GetCombinedJsons(0);

/*         string excapedJSON = JSON.Replace("\r\n", "\\r\\n");
        excapedJSON = excapedJSON.Replace(@"""", @"\""");
        excapedJSON = excapedJSON.Replace("\n", "\\n"); */



        db.SendInsertRequest(JSON);
    }

}
