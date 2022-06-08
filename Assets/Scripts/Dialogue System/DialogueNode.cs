using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> childNodes = new List<string>();
        [SerializeField]
        private Rect nodeRect = new Rect(30, 30, 200, 100);

        public Rect GetRect()
        {
            return nodeRect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return childNodes;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            nodeRect.position = newPos;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            childNodes.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            childNodes.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}






