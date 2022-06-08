using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingTest))]
public class TopDownAI_Editor : Editor
{
    private static bool setEndPosition = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathfindingTest entity = (PathfindingTest)target;

        if (GUILayout.Button("Set Target Position"))
        {
            if(!setEndPosition)
            {
                setEndPosition = true;
            }
            else
            {
                setEndPosition = false;
            }
        }
    }

    public void OnSceneGUI()
    {
        PathfindingTest entity = (PathfindingTest)target;

        if(setEndPosition)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));

            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = Camera.current.pixelHeight - mousePos.y;
            Vector3 position = Camera.current.ScreenPointToRay(mousePos).origin;
            position.x = Mathf.RoundToInt(position.x + 0.5f) - 0.5f;
            position.y = Mathf.RoundToInt(position.y + 0.5f) - 0.5f;
            position.z = 0;
            Debug.Log(position);
            var color = new Color(1, 0.8f, 0.4f, 1);
            Handles.color = color;
            Handles.DrawWireCube(position, new Vector3(1, 1, 0));
            SceneView.RepaintAll();
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {               
                entity.endPos = position;
                setEndPosition = false;
            }
        }
    }
}
