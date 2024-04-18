using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestionnaireConfiguration))]
public class Validate : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QuestionnaireConfiguration config = (QuestionnaireConfiguration)target;

        if (GUILayout.Button("Validate Questionnaires"))
        {
            config.ValidateAll();
        }
    }
}
