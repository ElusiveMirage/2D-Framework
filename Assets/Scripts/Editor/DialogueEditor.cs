using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        DialogueNode currentNode = null;
        [NonSerialized]
        Vector2 mouseOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkParentNode = null;
        [NonSerialized]
        bool dragCanvas = false;
        [NonSerialized]
        Vector2 dragCanvasOffset;

        Vector2 scrollPos;

        const float canvasSize = 6000;
        const float backgroundSize = 50;

        [MenuItem("Window/DialogueEditor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnAssetOpen(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if(dialogue != null)
            {
                ShowEditorWindow();          
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }
                                                                                                                 
        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNodelinks(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void DrawNodelinks(DialogueNode node)
        {
            Vector3 startPos = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
 
                Vector3 endPos = new Vector2(childNode.GetRect().center.x, childNode.GetRect().center.y);
                Vector3 offset = endPos - startPos;
                offset.y = 0;
                offset.x *= 0.8f;
                Handles.DrawBezier(startPos, endPos, startPos + offset, endPos - offset, Color.white, null, 4f);
            }
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && currentNode == null)
            {
                currentNode = GetNodeAtPoint(Event.current.mousePosition + scrollPos);
                if(currentNode != null)
                {
                    mouseOffset = currentNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = currentNode;
                }
                else
                {
                    dragCanvas = true;
                    dragCanvasOffset = Event.current.mousePosition + scrollPos;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && currentNode != null)
            {
                currentNode.SetPosition(Event.current.mousePosition + mouseOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && dragCanvas)
            {
                scrollPos = dragCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && dragCanvas)
            {
                dragCanvas = false;
            }
            else if(Event.current.type == EventType.MouseUp && currentNode != null)
            {
                currentNode = null;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode returnNode = null;
            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(point))
                {
                    returnNode = node;
                }
            }
            return returnNode;
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.GetRect(), nodeStyle);

            string newText = EditorGUILayout.TextField(node.GetText());

            node.SetText(newText);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                deletingNode = node;
            }
            if(linkParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkParentNode = node;
                }
            }
            else if(linkParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkParentNode = null;
                }
            }
            else if (linkParentNode.GetChildren().Contains(node.name))
            {
                if(GUILayout.Button("Unlink"))
                {
                    linkParentNode.RemoveChild(node.name);
                    linkParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkParentNode.AddChild(node.name);
                    linkParentNode = null;
                }
            }         
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}


