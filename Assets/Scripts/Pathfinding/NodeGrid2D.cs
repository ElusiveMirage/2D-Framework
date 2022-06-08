using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class NodeGrid2D : MonoBehaviour
{
    //grid of aStarNodes created based on all Tilemaps in list
    public List<Tilemap> floorLayers; //all layers with walkable tiles
    public List<Tilemap> obstacleLayers; //all layers that contain objects to navigate around
    //=========================================================//     
    public float nodeSize; //size of nodes relative to world units
    public Vector2 gridSize = new Vector2(0, 0); //size of grid in Unity units, both axis has to be a even number
    public Node2D[,] nodes; //2D array representation of all nodes in scene
    public bool diagonalMovement; //allow diagonal movement on grid
    public bool calculateGrid = true;
    public bool showGrid = false;
    //=========================================================//
    private int gridX, gridY; //length and width of grid relative to size of grid nodes      
    private Vector3 gridStartPos; //origin point of the grid, calculated based on the grid size
    private float gridOffset;

    private void Start()
    {
        ConstructGrid();
    }

    public void ConstructGrid()
    {
        if(calculateGrid)
        {
           CalculateGrid();
        }
        else
        {
            gridStartPos = new Vector3(gridSize.x / 2, gridSize.y / 2, 0) * -1;
        }
        
        gridOffset = nodeSize / 2;

        gridX = Mathf.Abs(Mathf.RoundToInt(gridSize.x / nodeSize));
        gridY = Mathf.Abs(Mathf.RoundToInt(gridSize.y / nodeSize));

        gridStartPos = new Vector3(gridStartPos.x + gridOffset, gridStartPos.y + gridOffset, 0);

        nodes = new Node2D[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 nextPos = gridStartPos + new Vector3(x * nodeSize, y * nodeSize, 0);

                bool walkable = true;

                foreach (Tilemap tm in obstacleLayers)
                {
                    if (tm.HasTile(tm.WorldToCell(nextPos)))
                    {
                        walkable = false;
                    }
                }

                Vector2Int gridPos = new Vector2Int(x, y);

                Node2D node = new Node2D(walkable, nextPos, gridPos);

                nodes[x, y] = node;
            }
        }
    }

    public List<Node2D> GetAdjacentNodes(Node2D node)
    {
        List<Node2D> adjacentNodes = new List<Node2D>();

        int x = node.gridPos.x;
        int y = node.gridPos.y;

        //checking for edge cases
        bool left = x != 0;
        bool right = x != gridX - 1;
        bool down = y != 0;
        bool up = y != gridY - 1;

        //add adjacent nodes to list
        if (left)
        {
            adjacentNodes.Add(nodes[x - 1, y]);
        }
        if (right)
        {
            adjacentNodes.Add(nodes[x + 1, y]);
        }
        if (down)
        {
            adjacentNodes.Add(nodes[x, y - 1]);
        }
        if (up)
        {
            adjacentNodes.Add(nodes[x, y + 1]);
        }

        if (diagonalMovement)
        {
            if (left)
            {
                if (down)
                {
                    if (nodes[x - 1, y].isWalkable && nodes[x, y - 1].isWalkable)
                    {
                        adjacentNodes.Add(nodes[x - 1, y - 1]);
                    }
                }
                if (up)
                {
                    if (nodes[x - 1, y].isWalkable && nodes[x, y + 1].isWalkable)
                    {
                        adjacentNodes.Add(nodes[x - 1, y + 1]);
                    }
                }
            }
            if (right)
            {
                if (down)
                {
                    if (nodes[x + 1, y].isWalkable && nodes[x, y - 1].isWalkable)
                    {
                        adjacentNodes.Add(nodes[x + 1, y - 1]);
                    }
                }
                if (up)
                {
                    if (nodes[x + 1, y].isWalkable && nodes[x, y + 1].isWalkable)
                    {
                        adjacentNodes.Add(nodes[x + 1, y + 1]);
                    }
                }
            }
        }

        return adjacentNodes;
    }

    public Node2D GetNodeFromWorldPoint(Vector3 worldPos)
    {
        //returns node in grid array from world point
        int x = Mathf.RoundToInt(worldPos.x * (1f / nodeSize) + Mathf.Abs(gridStartPos.x / nodeSize));
        int y = Mathf.RoundToInt(worldPos.y * (1f / nodeSize) + Mathf.Abs(gridStartPos.y / nodeSize));

        return nodes[x, y];
    }

    public void ClearGrid()
    {
        gridSize = Vector2.zero;
        nodes = null;
    }

    private void CalculateGrid()
    {
        //calculates length and width of grid based on tilemaps in list
        gridSize = Vector2.zero;

        float xMax = 0, xMin = 0, yMin = 0, yMax = 0;
      
        foreach (Tilemap tm in floorLayers)
        {
            tm.CompressBounds();

            if (xMax < tm.localBounds.max.x)
            {
                xMax = tm.localBounds.max.x;
            }
            if (xMin > tm.localBounds.min.x)
            {
                xMin = tm.localBounds.min.x;
            }

            if (yMax < tm.localBounds.max.y)
            {
                yMax = tm.localBounds.max.y;
            }
            if (yMin > tm.localBounds.min.y)
            {
                yMin = tm.localBounds.min.y;
            }
        }

        foreach (Tilemap tm in obstacleLayers)
        {
            tm.CompressBounds();

            if (xMax < tm.localBounds.max.x)
            {
                xMax = tm.localBounds.max.x;
            }
            if (xMin > tm.localBounds.min.x)
            {
                xMin = tm.localBounds.min.x;
            }

            if (yMax < tm.localBounds.max.y)
            {
                yMax = tm.localBounds.max.y;
            }
            if (yMin > tm.localBounds.min.y)
            {
                yMin = tm.localBounds.min.y;
            }
        }

        gridSize.x = (xMax - xMin);
        gridSize.y = (yMax - yMin);

        if (gridSize.x % 2 == 1)
        {
            gridSize.x++;
        }

        if (gridSize.y % 2 == 1)
        {
            gridSize.y++;
        }

        gridStartPos = new Vector3(xMin, yMin, 0);
    }

    public void OnValidate()
    {
        //input validation for gridsize to ensure it is a even number
        gridSize.x = Mathf.Abs(Mathf.RoundToInt(gridSize.x / 2) * 2);
        gridSize.y = Mathf.Abs(Mathf.RoundToInt(gridSize.y / 2) * 2);
    }

    private void OnDrawGizmos()
    {
        if (showGrid && nodes != null)
        {
            //outlines unwalkable tiles in red, walkable tiles in white
            foreach (Node2D n in nodes)
            {
                if (n.isWalkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireCube(n.worldPos, new Vector3(1, 1, 1) * nodeSize);
            }
        }
    }
}

