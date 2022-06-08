using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AStarPathfinding : MonoBehaviour
{
    public NodeGrid2D nodeGrid; //reference to node grid 
    public bool drawPath; //draws node path for debugging

    private List<Node2D> nodesToDraw = new List<Node2D>();

    public List<Node2D> CalculatePath(Vector3 startPos, Vector3 endPos)
    {
        Node2D startNode = nodeGrid.GetNodeFromWorldPoint(startPos);
        Node2D endNode = nodeGrid.GetNodeFromWorldPoint(endPos);

        List<Node2D> openList = new List<Node2D>();
        HashSet<Node2D> closedList = new HashSet<Node2D>();

        openList.Add(startNode);
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();

        while (openList.Count > 0)
        {
            Node2D currentNode = openList[openList.Count - 1];

            openList.RemoveAt(openList.Count - 1);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                st.Stop();
                Debug.Log("Pathfinding Complete : " + st.ElapsedMilliseconds.ToString("F3"));
                return RetracePath(startNode, endNode);              
            }

            List<Node2D> adjacentNodes = new List<Node2D>();

            foreach (Node2D adjacentNode in nodeGrid.GetAdjacentNodes(currentNode))
            {
                if (!adjacentNode.isWalkable || closedList.Contains(adjacentNode))
                {
                    continue;
                }

                if (!openList.Contains(adjacentNode))
                {
                    adjacentNode.nodeParent = currentNode;
                    adjacentNode.gCost = currentNode.gCost + GetManDist(adjacentNode, currentNode);
                    adjacentNode.hCost = GetManDist(adjacentNode, endNode);
                    adjacentNodes.Add(adjacentNode);
                }
            }

            adjacentNodes.Sort((x, y) => y.fCost - x.fCost);

            openList = MergeLists(openList, adjacentNodes);
        }

        return new List<Node2D>();
    }
    public int GetManDist(Node2D start, Node2D end)
    {
        //calculate heuristic using Manhattan distance 
        return Mathf.Abs(start.gridPos.x - end.gridPos.x) + Mathf.Abs(start.gridPos.y - end.gridPos.y);
    }

    public List<Node2D> MergeLists(List<Node2D> openList, List<Node2D> adjacentNodes)
    {
        List<Node2D> result = new List<Node2D>();

        int i = 0, j = 0;

        while (i < openList.Count || j < adjacentNodes.Count)
        {
            if (i >= openList.Count)
            {
                result.Add(adjacentNodes[j]);
                j++;
                continue;
            }
            if (j >= adjacentNodes.Count)
            {
                result.Add(openList[i]);
                i++;
                continue;
            }

            if (openList[i].fCost > adjacentNodes[j].fCost)
            {
                result.Add(openList[i]);
                i++;
            }
            else
            {
                result.Add(adjacentNodes[j]);
                j++;
            }
        }

        return result;
    }

    public List<Node2D> RetracePath(Node2D start, Node2D end)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D current = end;

        //retrace path by iterating through node parents
        while (current != start)
        {
            path.Add(current);
            current = current.nodeParent;
        }

        //reverse list to put nodes in correct order
        path.Reverse();

        if (drawPath)
        {
            nodesToDraw = new List<Node2D>(path);
        }

        return path;
    }

    private void OnDrawGizmos()
    {
        //Draw the path in yellow if debugging
        if (drawPath)
        {
            Gizmos.color = Color.yellow;
            if (nodesToDraw.Count > 0)
            {
                foreach (Node2D n in nodesToDraw)
                {
                    Gizmos.DrawWireCube(n.worldPos, new Vector3(1, 1, 1) * nodeGrid.nodeSize);
                }
            }
        }
    }
}

