using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> mazes3Obstacles;
    [SerializeField] List<GameObject> mazes5Obstacles;
    [SerializeField] int numObstacles = 3;

    private void Awake()
    {
        List<GameObject> mazesList;
        switch (numObstacles)
        {
            case 3:
                mazesList = mazes3Obstacles;
                break;
            case 5:
                mazesList = mazes5Obstacles;
                break;
            default:
                mazesList = mazes3Obstacles;
                break;
        }

        int randomMazeID = Random.Range(0, mazes3Obstacles.Count);

        // Instantiate maze and trolley
        Instantiate(mazes3Obstacles[randomMazeID], new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}
