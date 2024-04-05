using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum QuestionnaireType
{
    Likert,
    SingleSelect,
    MultiSelect
}

[Serializable]
public class Question
{
    public string questionTitle;
    public List<string> options = new List<string>(); // For Likert, this could be predefined scale options
}

[Serializable]
public class Questionnaire
{
    public string QuestionnaireTitle;
    public QuestionnaireType type;
    //public int scale; // Used for Likert scale
    public List<Question> questions = new List<Question>();
}

[CreateAssetMenu(fileName = "NewQuestionnaireConfiguration", menuName = "Questionnaire/Configuration")]
public class QuestionnaireConfiguration : ScriptableObject
{
    public List<Questionnaire> questionnaires = new List<Questionnaire>();

    // Methods here to help configure or validate the questionnaires as needed.

    public bool ValidateLikertScaleOptions()
    {
        foreach (var questionnaire in questionnaires)
        {
            if (questionnaire.type == QuestionnaireType.Likert)
            {
                if (!AllQuestionsHaveSameOptions(questionnaire.questions))
                {
                    Debug.LogError($"Questionnaire '{questionnaire.QuestionnaireTitle}' has questions with differing options. Please make sure the number of options in a Likert questionnaire are equal across all questions.");
                    return false;
                }
            }
        }
        return true;
    }

    private bool AllQuestionsHaveSameOptions(List<Question> questions)
    {
        if (questions == null || questions.Count == 0)
        {
            return true;
        }

        var firstQuestionOptions = questions[0].options;
        return questions.All(q => q.options.SequenceEqual(firstQuestionOptions));
    }


}


