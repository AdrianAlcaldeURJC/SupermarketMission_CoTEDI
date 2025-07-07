using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    ObstaclesMinigame,
    RandomMinigame
}

public enum NodeDir
{
    Right,
    Left,
    Top,
    Down
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;

    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.blue;
                break;
            case NodeState.ObstaclesMinigame:
                floor.material.color = Color.red;
                break;
            case NodeState.RandomMinigame:
                floor.material.color = Color.magenta;
                break;
        }
    }

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].SetActive(false);
    }

    public bool GetWallStatus(NodeDir wallDirection)
    {
        return walls[(int)wallDirection].activeSelf;
    }
}
