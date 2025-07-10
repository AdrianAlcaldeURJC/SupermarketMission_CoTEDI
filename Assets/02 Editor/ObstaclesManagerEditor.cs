using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstaclesManager))]
public class ObstaclesManagerEditor : EditorWindow
{
    GameObject prefabToSpawn;

    [MenuItem("ObstaclesManager/PrefabSpawner")]
    public static void ShowWindow()
    {
        GetWindow<ObstaclesManagerEditor>("Prefab spawner");
    }

    void OnGUI()
    {
        GUILayout.Label("Spawn prefab settings", EditorStyles.boldLabel);

        prefabToSpawn = EditorGUILayout.ObjectField("Prefab", prefabToSpawn, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn prefab with random Obstacles"))
        {
            //prefabToSpawn.GetComponent<ObstaclesManager>().GenerateObstacles();
            InstantiateObstaclesMaze();

        }
    }

    void InstantiateObstaclesMaze()
    {
        if (prefabToSpawn == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a prefab to spawn", "Ok");
            return;
        }

        GameObject spawnedPrefab = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
        Undo.RegisterCreatedObjectUndo(spawnedPrefab, "Spawn prefab");

        spawnedPrefab.GetComponent<ObstaclesManager>().GenerateObstacles();
    }
}