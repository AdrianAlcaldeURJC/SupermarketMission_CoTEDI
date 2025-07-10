using System.Collections.Generic;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{
    [SerializeField] List<MazeNode> mazeNodes;
    [SerializeField] List<GameObject> arrows;
    [SerializeField] Vector2Int mazeSize; // x z
    [SerializeField] int spawnIndex;
    private int currentIndex;

    private static MazeMovement Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
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
                break;

            case NodeDir.Down:
                nextNodeIndex = currentIndex - mazeSize[0];
                break;

            case NodeDir.Right:
                nextNodeIndex = currentIndex + 1;
                break;

            case NodeDir.Left:
                nextNodeIndex = currentIndex - 1;
                break;
        }

        // Check boundaries
        if (nextNodeIndex < 0 || nextNodeIndex > mazeSize.x * mazeSize.y)
            return;

        // Check if direction is blocked
        if (!mazeNodes[nextNodeIndex].GetWallStatus(direction.GetOppositeDirection()))
        {
            currentIndex = nextNodeIndex;
            Vector3 destination = mazeNodes[nextNodeIndex].transform.position;
            destination = new Vector3(destination.x, transform.position.y, destination.z);
            transform.position = destination;
        }

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
