using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGrid2D))]
public class NodeGrid2D_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeGrid2D aStarGrid = (NodeGrid2D)target;

        if(GUILayout.Button("Draw Grid"))
        {
            aStarGrid.ConstructGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            aStarGrid.ClearGrid();
        }
    }
}
