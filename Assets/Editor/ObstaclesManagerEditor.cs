using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstaclesManager))]
public class ObstaclesManagerEditor : EditorWindow
{
    GameObject prefabToSpawn;
    GameObject obstacleModelOne;
    GameObject obstacleModelTwo;
    GameObject obstacleModelThree;

    List<GameObject> obstaclesModels = new List<GameObject>(3);

    [MenuItem("ObstaclesManager/PrefabSpawner")]
    public static void ShowWindow()
    {
        GetWindow<ObstaclesManagerEditor>("Prefab spawner");
    }

    void OnGUI()
    {
        GUILayout.Label("Spawn prefab settings", EditorStyles.boldLabel);

        prefabToSpawn = EditorGUILayout.ObjectField("Prefab", prefabToSpawn, typeof(GameObject), false) as GameObject;

        // Load all obstacles
        obstaclesModels.Clear();
        obstacleModelOne = EditorGUILayout.ObjectField("Obstacle Model 1", obstacleModelOne, typeof(GameObject), false) as GameObject;
        obstacleModelTwo = EditorGUILayout.ObjectField("Obstacle Model 2", obstacleModelTwo, typeof(GameObject), false) as GameObject;
        obstacleModelThree = EditorGUILayout.ObjectField("Obstacle Model 3", obstacleModelThree, typeof(GameObject), false) as GameObject;

        obstaclesModels.Add(obstacleModelOne);
        obstaclesModels.Add(obstacleModelTwo);
        obstaclesModels.Add(obstacleModelThree);

        if (GUILayout.Button("Spawn prefab with random Obstacles"))
        {
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

        // Generate the list of obstacles
        spawnedPrefab.GetComponent<ObstaclesManager>().GenerateObstacles();

        // Add the obstacles to the GameObject
        List<MazeNode> selectedObstaclesNodes = spawnedPrefab.GetComponent<ObstaclesManager>().GetSelectedObstaclesNodes();

        foreach (MazeNode node in selectedObstaclesNodes)
        {
            int randomObstacleIndex = UnityEngine.Random.Range(0, obstaclesModels.Count);
            GameObject spawnedObstacle = PrefabUtility.InstantiatePrefab(obstaclesModels[randomObstacleIndex]) as GameObject;
            Undo.RegisterCreatedObjectUndo(spawnedObstacle, "Obstacle prefab");

            spawnedObstacle.transform.SetParent(node.transform);
            spawnedObstacle.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        }


        // Destroy this code component, so the build does not give any problem   
        DestroyImmediate(spawnedPrefab.GetComponent<ObstaclesManager>());
    }
}