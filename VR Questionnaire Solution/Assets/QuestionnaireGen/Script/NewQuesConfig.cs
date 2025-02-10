using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestionnaireConfiguration1", menuName = "Questionnaire/Configuration")]
public class NewQuesConfig : ScriptableObject
{
    [Tooltip("File path where the exported CSV will be saved.")]
     public string ExportedCsvPath;

    [Tooltip("List of questions.")]
     public List<QuestionnaireQuestion> questions = new List<QuestionnaireQuestion>();
}


public enum QuestionType
{
    Likert,
    SingleSelect,
    MultiSelect,
    Slider_LeftStart,
    Slider_MidStart,
    InputField,
    DropDown,
    Consent
}

[System.Serializable]
public class QuestionnaireQuestion
{
    [Tooltip("Select the type of question.")]
    public QuestionType questionType;

   
    [Header("Common Field")]
    [Tooltip("The title or question text.")]
    public string questionTitle;

    [Header("Options (For Likert, SingleChoice, MultiChoice, Dropdown)")]
    [Tooltip("List of option titles. Only used for Likert, SingleChoice, MultiChoice, and Dropdown types.")]
    public List<string> options = new List<string>();

    [Header("Consent Form (For ConsentForm type)")]
    [Tooltip("Consent content text. Only used for the ConsentForm type.")]
    [TextArea(3, 10)]
    public string consentContent;

    [Header("Slider (For Slider types)")]
    [Tooltip("The label for the leftmost value of the slider. Only used for Slider types.")]
    public string leftLabel;
    [Tooltip("The label for the rightmost value of the slider. Only used for Slider types.")]
    public string rightLabel;
}