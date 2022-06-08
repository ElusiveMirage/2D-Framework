using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Add additional properties for use in differentating different node levels
public class Node2D
{
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public int gCost; //distance from start point
    public int hCost; //distance from end point
    //=========================================================//
    public Vector3 worldPos; //position in world space
    public Vector2Int gridPos; //position in grid array
    //public int nodeLayer; 
    public bool isWalkable = true;
    public bool isPlaceable = false;
    public bool unitPlaced = false;
    public Transform deploymentZone;
    //=========================================================//
    public List<Node2D> adjacentNodes;
    public Node2D nodeParent;

    public Node2D(bool walkable, Vector3 _worldPos, Vector2Int _gridPos)
    {
        isWalkable = walkable;
        worldPos = _worldPos;
        gridPos = _gridPos;
    }
}

