using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorageManager : MonoBehaviour
{

    private static DataStorageManager _instance;

    public static DataStorageManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("DatastorageManager is null");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }


    public Dictionary<int, int> questionnaireData = new Dictionary<int, int>();
    public int questionIndex = 0;



    public  void PrintAllQuestionnaireResults(List<GameObject> questions)
    {
        foreach (GameObject questionObject in questions)
        {
            QuestionPanel questionPanel = questionObject.GetComponent<QuestionPanel>();
            if (questionPanel != null)
            {
                Debug.Log($"Questionnaire Index: {questionPanel.QuestionnaireIndex}");
                Debug.Log($"Question Index: {questionPanel.QuestionIndex}");

                // Assuming QuestionIndex is accessible and iterating through each question
                foreach (KeyValuePair<int, int> option in questionPanel.OptionDictionairy)
                {
                    if (option.Value==1)
                    {
                        Debug.Log($"Option Selected: {option.Key}");
                    }
                }
            }
        }
    }

}
