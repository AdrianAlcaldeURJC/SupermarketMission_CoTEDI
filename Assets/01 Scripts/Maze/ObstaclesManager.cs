
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] int obstaclesNumber;
    [SerializeField] List<MazeConditionsClass> obstaclesConditions;

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

    public void GenerateObstacles()
    {
        //ResetObstacles();
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
    }

    public List<MazeNode> GetSelectedObstaclesNodes()
    {
        return SelectedObstaclesNodes;
    }
}