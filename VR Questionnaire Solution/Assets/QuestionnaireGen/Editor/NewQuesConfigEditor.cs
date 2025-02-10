#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(NewQuesConfig))]
public class NewQuesConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector interface.
        DrawDefaultInspector();

        // Reference to the target ScriptableObject.
        NewQuesConfig config = (NewQuesConfig)target;

        // Add a space and a Validate button.
        EditorGUILayout.Space();
        if (GUILayout.Button("Validate"))
        {
            List<string> errors = config.Validate();

            if (errors.Count > 0)
            {
                // Concatenate all error messages into a single string.
                string errorMessage = string.Join("\n", errors.ToArray());
                EditorUtility.DisplayDialog("Validation Errors", errorMessage, "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation", "All inputs are valid.", "OK");
            }
        }
    }
}
#endif
