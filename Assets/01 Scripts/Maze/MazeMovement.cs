using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{

    private class MazeStep
    {
        public int Direction;
        public bool IsCorrect;
        public float Time;

        public MazeStep(int dir, bool isCorrect, float time)
        {
            Direction = dir;
            IsCorrect = isCorrect;
            Time = time;
        }

        public override string ToString()
        {
            return $"({Direction}, {IsCorrect}, {Time})";
        }
    }

    [SerializeField] List<GameObject> arrows;
    [SerializeField] Vector2Int mazeSize; // x z
    [SerializeField] GameObject trolleyModel;
    [SerializeField] int spawnIndex;
    private static MazeMovement Instance;
    private List<MazeNode> mazeNodes;
    private List<string> mazeStepsString;
    private int currentIndex;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        mazeStepsString = new List<string>();
    }

    void Start()
    {
        mazeNodes = new List<MazeNode>();
        var obj = FindObjectOfType<ObstaclesManagerSingleton>();
        for (int i = 0; i < mazeSize.x * mazeSize.y; ++i)
        {
            mazeNodes.Add(obj.transform.GetChild(i).GetComponent<MazeNode>());
        }

        currentIndex = spawnIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(NodeDir.Top);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(NodeDir.Down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(NodeDir.Right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(NodeDir.Left);
        }
    }

    void Move(NodeDir direction)
    {
        int nextNodeIndex = -1;
        // Move to correct node
        switch (direction)
        {
            case NodeDir.Top:
                nextNodeIndex = currentIndex + mazeSize[0];
                trolleyModel.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                break;

            case NodeDir.Down:
                nextNodeIndex = currentIndex - mazeSize[0];
                trolleyModel.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;

            case NodeDir.Right:
                nextNodeIndex = currentIndex + 1;
                trolleyModel.transform.eulerAngles = new Vector3(0f, 270f, 0f);
                break;

            case NodeDir.Left:
                nextNodeIndex = currentIndex - 1;
                trolleyModel.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                break;
        }

        // Check boundaries
        if (nextNodeIndex < 0 || nextNodeIndex > mazeSize.x * mazeSize.y)
            return;

        // Check if direction is blocked
        if (CheckIfDirectionIsAvailable(nextNodeIndex, direction))
        {
            currentIndex = nextNodeIndex;
            Vector3 destination = mazeNodes[nextNodeIndex].transform.position;
            destination = new Vector3(destination.x, transform.position.y, destination.z);
            transform.position = destination;
        }

        SaveMovementData(direction, CheckIfDirectionIsAvailable(nextNodeIndex, direction));
    }

    bool CheckIfDirectionIsAvailable(int nextNodeIndex, NodeDir direction)
    {
        return !mazeNodes[nextNodeIndex].GetWallStatus(direction.GetOppositeDirection());
    }

    void SaveMovementData(NodeDir direction, bool directionAvailable)
    {
        DataStorage.Instance.mazeMapData.MazeStepsCount++;

        float time = ObstaclesManagerSingleton.Instance.timerAux.elapsedTime[ObstaclesManagerSingleton.Instance.timerID];
        MazeStep step = new MazeStep((int)direction, directionAvailable, time);
        mazeStepsString.Add(step.ToString());
        DataStorage.Instance.mazeMapData.MazeSteps = string.Join(",", mazeStepsString);
    }

    public void MoveTop()
    {
        Move(NodeDir.Top);
    }

    public void MoveDown()
    {
        Move(NodeDir.Down);
    }

    public void MoveRight()
    {
        Move(NodeDir.Right);
    }

    public void MoveLeft()
    {
        Move(NodeDir.Left);
    }

}
