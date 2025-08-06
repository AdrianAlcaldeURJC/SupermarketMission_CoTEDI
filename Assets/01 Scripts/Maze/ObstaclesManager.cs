
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;


[System.Serializable]
public class MazeConditionsClass
{
    public List<MazeNode> list;
}

[ExecuteInEditMode]
public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] List<MazeNode> obstaclesNodes;
    [SerializeField] List<MazeNode> sectionsNodes;
    [SerializeField] int obstaclesNumber;
    [SerializeField] List<MazeConditionsClass> obstaclesConditions;
    [SerializeField] GameObject differentFloorTile;
    private List<MazeNode> SelectedObstaclesNodes = new List<MazeNode>();

    private bool CheckConditions(MazeNode node)
    {
        bool conditionMet = false;
        foreach (MazeConditionsClass list in obstaclesConditions)
        {
            if (list.list.Contains(node))
            {
                bool localConditionMet = true;
                foreach (var conditionNode in list.list)
                {
                    if (!conditionNode.GetObstacle() && conditionNode != node)
                        localConditionMet = false;
                }
                if (localConditionMet == true)
                {
                    conditionMet = true;
                    break;
                }
            }
        }

        if (conditionMet)
            return false;
        else
            return true;
    }

    private void ResetObstacles()
    {
        foreach (MazeNode obstacleNode in obstaclesNodes)
        {
            obstacleNode.ResetToObstacle();
        }
    }

    private void FillFloorColors()
    {
        List<MazeNode> nodes = transform.GetComponentsInChildren<MazeNode>().ToList();
        int colorID = 0;
        foreach (MazeNode node in nodes)
        {
            if (colorID == 0)
            {
                // Do not touch the floor
                colorID = 1;
            }
            else if (colorID == 1)
            {
                // Set the floor to the new floor color
                GameObject floorInstantiated = Instantiate(differentFloorTile);
                floorInstantiated.transform.parent = node.transform;
                floorInstantiated.name = "Floor_1";
                floorInstantiated.transform.localPosition = new UnityEngine.Vector3(0.55f, -0.5f, -0.55f);
                node.SetFloor(floorInstantiated);
                colorID = 0;
            }
        }
    }

    public void GenerateObstacles()
    {
        int remainingObstacles = obstaclesNumber;
        while (remainingObstacles != 0)
        {
            int obstacleNodeIndex = UnityEngine.Random.Range(0, obstaclesNodes.Count - 1);
            if (!obstaclesNodes[obstacleNodeIndex].GetObstacle() && CheckConditions(obstaclesNodes[obstacleNodeIndex]))
            {
                obstaclesNodes[obstacleNodeIndex].SetObstacle();
                SelectedObstaclesNodes.Add(obstaclesNodes[obstacleNodeIndex]);
                remainingObstacles--;
            }
        }

        //FillFloorColors();

    }

    public List<MazeNode> GetSelectedObstaclesNodes()
    {
        return SelectedObstaclesNodes;
    }

}
