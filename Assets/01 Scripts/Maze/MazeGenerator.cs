using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;

    private void Start()
    {
        GenerateMazeInstant(mazeSize);
        StartCoroutine(InstantiateAllZones(mazeSize));
    }

    void GenerateMazeInstant(Vector2Int size)
    {
        // Depth First Search Algorithm 
        List<MazeNode> nodes = new List<MazeNode>();

        // Create nodes
        for (int x = 0; x < size.x; ++x)
        {
            for (int y = 0; y < size.y; ++y)
            {
                // Centers the maze around (0, 0, 0) worldPos
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        // currentPath.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)]);
        currentPath.Add(nodes[0]);
        currentPath[0].SetState(NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node right of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node left of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = UnityEngine.Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
            }
            // Backtrack
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
    }

    IEnumerator InstantiateAllZones(Vector2Int size)
    {
        List<MazeNode> nodes = GetComponentsInChildren<MazeNode>().ToList();

        // Find longest path
        uint maxLength = 0;
        uint currentLength = 0;

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();
        List<MazeNode> longestPath = new List<MazeNode>();

        currentPath.Add(nodes[0]);
        currentPath[0].SetState(NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node right of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]) &&
                    !nodes[currentNodeIndex].GetWallStatus(NodeDir.Right))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node left of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]) &&
                    !nodes[currentNodeIndex].GetWallStatus(NodeDir.Left))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]) &&
                    !nodes[currentNodeIndex].GetWallStatus(NodeDir.Top))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below of current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]) &&
                    !nodes[currentNodeIndex].GetWallStatus(NodeDir.Down))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = UnityEngine.Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                currentLength++;
                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Completed);
            }
            // Backtrack
            else
            {
                if (currentLength > maxLength)
                {
                    Debug.Log(currentLength);
                    maxLength = currentLength;
                    currentLength = 0;
                    longestPath = new List<MazeNode>(currentPath);
                }
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Current);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
            //yield return new WaitForSeconds(0.02f);
        }

        // Draw longest path
        foreach (var node in longestPath)
        {
            node.SetState(NodeState.Completed);
            yield return new WaitForSeconds(0.1f);
        }

        // Set obstacles minigame and random minigames positions
        longestPath[longestPath.Count - 1].SetState(NodeState.ObstaclesMinigame);

        List<MazeNode> minigameNode = new List<MazeNode>();
        for (int i = 0; i < 6; ++i)
        {
            int randomNodeIndex = 0;
            while (longestPath.Contains(nodes[randomNodeIndex]) || minigameNode.Contains(nodes[randomNodeIndex]))
            {
                randomNodeIndex = UnityEngine.Random.Range(0, nodes.Count - 1);
            }
            minigameNode.Add(nodes[randomNodeIndex]);
            nodes[randomNodeIndex].SetState(NodeState.RandomMinigame);
            Debug.Log(randomNodeIndex);
            yield return new WaitForSeconds(0.1f);
        }

    }

}

