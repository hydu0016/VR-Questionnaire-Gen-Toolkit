using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuesConfig", menuName = "Questionnaire/Configuration")]
public class NewQuesConfig : ScriptableObject
{
    [Tooltip("File path where the exported CSV will be saved.")]
     public string ExportedCsvPath;

    [Tooltip("List of questions.")]
     public List<QuestionnaireQuestion> questions = new List<QuestionnaireQuestion>();

//////
    /// <summary>
    /// Validates the configuration and returns a list of error messages, if any.
    /// </summary>
    public List<string> Validate()
    {
        List<string> errors = new List<string>();

        // Check if the CSV export path is set.
        if (string.IsNullOrEmpty(ExportedCsvPath))
        {
            errors.Add("Exported CSV Path is empty.");
        }

        // Validate each question in the list.
        for (int i = 0; i < questions.Count; i++)
        {
            var question = questions[i];
            string questionIndex = $"Question {i + 1}";

            // Validate common field: question title must not be empty.
            if (string.IsNullOrEmpty(question.questionTitle))
            {
                errors.Add($"{questionIndex}: Question title is empty.");
            }

            // Perform type-specific validations.
            switch (question.questionType)
            {
                case QuestionType.Likert:
                case QuestionType.SingleSelect:
                case QuestionType.MultiSelect:
                case QuestionType.DropDown:
                    // For these types, options must be provided.
                    if (question.options == null || question.options.Count == 0)
                    {
                        errors.Add($"{questionIndex}: {question.questionType} requires at least one option.");
                    }
                    else
                    {
                        // Check each option string.
                        for (int j = 0; j < question.options.Count; j++)
                        {
                            if (string.IsNullOrEmpty(question.options[j]))
                            {
                                errors.Add($"{questionIndex}: Option {j + 1} is empty.");
                            }
                        }
                    }
                    break;

                case QuestionType.Consent:
                    // Consent form must have consent content.
                    if (string.IsNullOrEmpty(question.consentContent))
                    {
                        errors.Add($"{questionIndex}: Consent form requires consent content.");
                    }
                    break;

                case QuestionType.Slider_LeftStart:
                case QuestionType.Slider_MidStart:
                    // Sliders must have both left and right labels.
                    if (string.IsNullOrEmpty(question.leftLabel))
                    {
                        errors.Add($"{questionIndex}: {question.questionType} requires a left label.");
                    }
                    if (string.IsNullOrEmpty(question.rightLabel))
                    {
                        errors.Add($"{questionIndex}: {question.questionType} requires a right label.");
                    }
                    break;

                case QuestionType.InputField:
                    // No additional fields required beyond the question title.
                    break;

                default:
                    break;
            }
        }

        return errors;
    }
/////
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