using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    ObstaclesMinigame,
    RandomMinigame,
    Blocked
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
    [SerializeField] bool obstacle = false;

    // !Kinda deprecated
    public void SetState(NodeState state)
    {   
        /*
        switch (state)
        {
            case NodeState.Available:
                floor.sharedMaterial.color = Color.white;
                break;
            case NodeState.Current:
                floor.sharedMaterial.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.sharedMaterial.color = Color.blue;
                break;
            case NodeState.ObstaclesMinigame:
                floor.sharedMaterial.color = Color.green;
                break;
            case NodeState.RandomMinigame:
                floor.sharedMaterial.color = Color.magenta;
                break;
            case NodeState.Blocked:
                floor.sharedMaterial.color = Color.red;
                break;
        }
        */
        floor.transform.position = new Vector3(floor.transform.position.x, -0.5f, floor.transform.position.z);
    }

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].SetActive(false);
    }

    public bool GetWallStatus(NodeDir wallDirection)
    {
        return walls[(int)wallDirection].activeSelf;
    }

    public void SetObstacle()
    {
        obstacle = true;
        foreach (var wall in walls)
        {
            if (wall.activeSelf == false)
            {
                wall.SetActive(true);
                wall.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        SetState(NodeState.Blocked);
    }

    public bool GetObstacle()
    {
        return obstacle;
    }

    public void ResetToObstacle()
    {
        // Remove walls
        RemoveWall(0);
        RemoveWall(1);
        RemoveWall(2);
        RemoveWall(3);
        floor.transform.position = new Vector3(floor.transform.position.x, -0.5f, floor.transform.position.z);
        SetState(NodeState.ObstaclesMinigame);
    }

    public void SetFloorColor(Material material)
    {
        floor.material = material;
    }
}

static class NodeDirMethods
{
    public static NodeDir GetOppositeDirection(this NodeDir direction)
    {
        switch (direction)
        {
            case NodeDir.Top:
                return NodeDir.Down;
            case NodeDir.Down:
                return NodeDir.Top;
            case NodeDir.Right:
                return NodeDir.Left;
            case NodeDir.Left:
                return NodeDir.Right;
            default:
                return NodeDir.Top;
        }
    }
}
