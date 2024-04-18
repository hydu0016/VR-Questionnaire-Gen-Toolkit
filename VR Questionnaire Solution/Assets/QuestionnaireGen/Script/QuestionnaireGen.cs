using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class QuestionnaireGen : MonoBehaviour
{

    public QuestionnaireConfiguration questionnaireConfiguration; // Assigned from the editor

    private GameObject questionnaire_Completion_Instruction;

    public GameObject questionPanelPrefab_likert;
    public GameObject questionPanelPrefab_singleSelect;
    public GameObject questionPanelPrefab_multiSelect;

    public GameObject optionPrefab_likert;
    public GameObject optionPrefab_singleSelect;
    public GameObject optionPrefab_multiSelect;

    private Transform questionnairePanel; // Parent panel for question panels
    private Button nextButton;
    private Button previousButton;
    private Button submitButton;
    int questionNumber = 1;
    int currentPage=1;

   float  flashDuration=0.15f;
   public Color flashColorForUnansweredQuestion=Color.red;

    List<GameObject> questionPanels = new List<GameObject>();

    void Start()
    {       

        questionnaire_Completion_Instruction=transform.Find("Base/Head/Head Title").gameObject;
        nextButton=transform.Find("Base/Button_Next").gameObject.GetComponent<Button>();
        previousButton=transform.Find("Base/Button_Previous").gameObject.GetComponent<Button>();
        submitButton=transform.Find("Base/Button_Submit").gameObject.GetComponent<Button>();
        questionnairePanel=transform.Find("Base/QuestionDisplayArea").gameObject.transform;

        LoadQuestionPrefabIntoQuestionList();
        AssignPageNumberToQuestionPrefab();
       // test();
        DisplayFirstPageQuestions();

        nextButton.onClick.AddListener(NextButtonPressed);
        previousButton.onClick.AddListener(PreviousButtonPressed);
        submitButton.onClick.AddListener(SubmitButtonPressed);
        submitButton.gameObject.SetActive(false); // Initially hide submit button
        previousButton.gameObject.SetActive(true); // Initially hide previous button
    }

    private void test()
    {
        foreach (var item in questionPanels)
        {
            Debug.Log("总Index" + item.GetComponent<QuestionPanel>().QuestionIndex + "所属问卷编号" + item.GetComponent<QuestionPanel>().QuestionnaireIndex + "所属问卷类型" + item.GetComponent<QuestionPanel>().questionnaireType + "所属页码" + item.GetComponent<QuestionPanel>().PageIndex);
        }

    }


    //Iterate questionnaires list
    private void LoadQuestionPrefabIntoQuestionList()
    {
        //Iterate each questionnaire
        for (int i = 0; i < questionnaireConfiguration.questionnaires.Count; i++)
        {
            //iterate each question in this questionnaire
            AssignValueToQuestionPrefab(questionnaireConfiguration.questionnaires[i]);
        }
    }


    //Add info to each questions in each questionnaires
    private void AssignValueToQuestionPrefab(Questionnaire questionnaire)
    {
        
        foreach (var question in questionnaire.questions)
        {

            GameObject questionPrefab = Instantiate(GetQuestionPrefabForQuestionnaireType(questionnaire.type), questionnairePanel);
            QuestionPanel qp = questionPrefab.GetComponent<QuestionPanel>();
            qp.QuestionIndex = questionNumber++;
            qp.QuestionnaireIndex = questionnaireConfiguration.questionnaires.IndexOf(questionnaire)+1;
            qp.questionnaireType = questionnaire.type;
            qp.QuestionTitle = question.questionTitle;
            qp.optionPrefab = GetOptionPrefabForQuestionnaireType(questionnaire.type);
            qp.options = question.options;

            
            questionPrefab.GetComponent<QuestionPanel>().SetupPanel(qp.QuestionTitle, qp.optionPrefab,qp.options,qp.questionnaireType);
            questionPrefab.SetActive(false);
            questionPanels.Add(questionPrefab);
        }
    }



    private GameObject GetQuestionPrefabForQuestionnaireType(QuestionnaireType type)
    {
        switch (type)
        {
            case QuestionnaireType.Likert:
                return questionPanelPrefab_likert;
            case QuestionnaireType.SingleSelect:
                return questionPanelPrefab_singleSelect;
            case QuestionnaireType.MultiSelect:
                return questionPanelPrefab_multiSelect;
            default:
                return null; 
        }
    }

    private GameObject GetOptionPrefabForQuestionnaireType(QuestionnaireType type)
    {
        switch (type)
        {
            case QuestionnaireType.Likert:
                return optionPrefab_likert;
            case QuestionnaireType.SingleSelect:
                return optionPrefab_singleSelect;
            case QuestionnaireType.MultiSelect:
                return optionPrefab_multiSelect;
            default:
                return null;
        }
    }



    private int currentQuestionIndex = 0;
    private int pageNumber = 1;

    private int tmp = 0;
    private void AssignPageNumberToQuestionPrefab()
    {
        for (int i = 0; i < questionnaireConfiguration.questionnaires.Count; i++)
        {
            int questionsPerPage = questionnaireConfiguration.questionnaires[i].type == QuestionnaireType.Likert ? 3 : 1;


            for (int j = 0; j < questionnaireConfiguration.questionnaires[i].questions.Count; j++)
            {
                questionPanels[currentQuestionIndex].GetComponent<QuestionPanel>().PageIndex = pageNumber;

                tmp++;

                if (tmp == questionsPerPage)
                {
                    pageNumber++;
                    tmp = 0;
                }

                currentQuestionIndex++;
            }


            if (questionnaireConfiguration.questionnaires[i].type == QuestionnaireType.Likert && tmp==0)
            {}
            else if (questionnaireConfiguration.questionnaires[i].type == QuestionnaireType.MultiSelect && tmp == 0)
            {}
            else if (questionnaireConfiguration.questionnaires[i].type == QuestionnaireType.SingleSelect && tmp == 0)
            {}
            else
            {
                pageNumber++;
            }
            tmp = 0;

            
        }
        pageNumber = questionPanels.Last().GetComponent<QuestionPanel>().PageIndex;
    }

    void ClearExistingQuestions(List<GameObject> currentPageQuestions)
    {
        foreach (var item in currentPageQuestions)
        {
            item.SetActive(false);
        }
    }

    List<GameObject>GetExistingQuestions()
    {
        List<GameObject> currentPageQuestions = new List<GameObject>();
        foreach (Transform child in questionnairePanel)
        {
            if (child.gameObject.activeSelf)
            {
                // If active, add it to the list
                currentPageQuestions.Add(child.gameObject);
            }
        }

        return currentPageQuestions;
    }

    void  NextButtonPressed()
    {
        List<GameObject> currentPageQuestions = GetExistingQuestions();

        //Check if all questions is completed
        if (!areAllQuestionsCompleted(currentPageQuestions))
        {
            WarnUnansweredQuestion(currentPageQuestions);
            return;
        }
        

        //Clear questions in current page
        ClearExistingQuestions(currentPageQuestions);

        //Check if next page is final page
        int nextPageNumber= ++currentPage;
        if (nextPageNumber==pageNumber)
        {
            //变 submissionbutton
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
           // return;
        }

        //Display questions in next page
        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == nextPageNumber)
            {
                item.SetActive(true);
            }
        }

        DisplayQuestionnaireCompletionInstruction(currentPage);
        //Debug.Log(currentPage);

    }

    bool areAllQuestionsCompleted(List<GameObject> currentPageQuestions){

        foreach (var item in currentPageQuestions)
        {
            if (item.GetComponent<QuestionPanel>().OptionDictionairy.All(kvp => kvp.Value == 0))
            {
                return false;
            }
        }

        return true;
    }

    void WarnUnansweredQuestion(List<GameObject> currentPageQuestions){
        
        List<GameObject> unAnsweredQuestions=new List<GameObject>();

        foreach (var item in currentPageQuestions)
        {
            if (item.GetComponent<QuestionPanel>().OptionDictionairy.All(kvp => kvp.Value == 0))
            {
                unAnsweredQuestions.Add(item);
            }
        }
        
        foreach (var item in unAnsweredQuestions)
        {
            var questionTitle=item.transform.Find("Title/QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>();
            var defaultColor=item.transform.Find("Title/QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().color;
            StartCoroutine(FlashTitleColor(questionTitle, defaultColor));
        }

    }

    IEnumerator FlashTitleColor(TextMeshProUGUI questionTitle, Color defaultColor)
    {
        // Change color to red
        yield return StartCoroutine(TransitionColor(questionTitle,defaultColor, flashColorForUnansweredQuestion, flashDuration));
        // Transition back to default color
        yield return StartCoroutine(TransitionColor(questionTitle,flashColorForUnansweredQuestion, defaultColor, flashDuration));
        // Transition to red again
        yield return StartCoroutine(TransitionColor(questionTitle,defaultColor, flashColorForUnansweredQuestion, flashDuration));
        // Transition back to default color
        yield return StartCoroutine(TransitionColor(questionTitle,flashColorForUnansweredQuestion, defaultColor, flashDuration));
    }

    IEnumerator TransitionColor(TextMeshProUGUI questionTitle, Color startColor, Color endColor, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            questionTitle.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null; // Wait until next frame
        }
        questionTitle.color = endColor; // Ensure final color is set
    }

    void PreviousButtonPressed()
    {
        //return if current page is already first page
        if (currentPage == 1)
        {
            return;
        }

        nextButton.gameObject.SetActive(true);
        //Hide submissuib button if current page is last page
        if (currentPage==pageNumber)
        {
            submitButton.gameObject.SetActive(false);
        }

        
        ClearExistingQuestions(GetExistingQuestions());
        currentPage--;

        //Display questions in previouse page
        foreach (var item in questionPanels)
            {
                if (item.GetComponent<QuestionPanel>().PageIndex == currentPage)
                {
                    item.SetActive(true);
                }
            }
        
        DisplayQuestionnaireCompletionInstruction(currentPage);
        Debug.Log(currentPage);

    }

    void SubmitButtonPressed()
    {
        List<GameObject> currentPageQuestions = GetExistingQuestions();
       if (!areAllQuestionsCompleted(currentPageQuestions))
        {
            WarnUnansweredQuestion(currentPageQuestions);
            return;
        }       
        //Export Data
        DataExporter.ExportData(questionPanels);
        //Display goodbye UI
        DisplayCloseUI();
    }

    void DisplayCloseUI(){

        //Hide All Buttons
        nextButton.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        gameObject.transform.Find("Base/QuestionDisplayArea").gameObject.SetActive(false);
        //Clear header
        questionnaire_Completion_Instruction.gameObject.GetComponent<TextMeshProUGUI>().text="";

        //Display closing UI
        gameObject.transform.Find("GoodByePanel").gameObject.SetActive(true);
        StartCoroutine(CountDown());
        
    }

    IEnumerator CountDown(float time=1){
        int count=3;
        while (count>0)
        {
            gameObject.transform.Find("GoodByePanel/Closing").gameObject.GetComponent<TextMeshProUGUI>().text=string.Format("Closing in {0} seconds...", count);
            yield return new WaitForSeconds(time);
            count--;
        }
        gameObject.transform.Find("GoodByePanel/Closing").GetComponent<TextMeshProUGUI>().text = string.Format("Closing in {0} seconds...", count);
        gameObject.SetActive(false);
    }


    void DisplayFirstPageQuestions()
    {       
        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == 1)
            {
                item.SetActive(true);

            }
        }

        QuestionnaireType type=questionnaireConfiguration.questionnaires[0].type;
        DisplayQuestionnaireCompletionInstruction(1);
    }

    void DisplayQuestionnaireCompletionInstruction(int pageNumber){
        
        string instruction="";
        QuestionnaireType type=questionnaireConfiguration.questionnaires[0].type;

        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == pageNumber)
            {
                type=item.GetComponent<QuestionPanel>().questionnaireType;
                break;
            }
        }

            switch (type)
        {
            case QuestionnaireType.Likert:
                instruction="Please rate each statement by selecting one option.";
                //return "Please rate each statement by selecting one option.";
                break;
            case QuestionnaireType.SingleSelect:
                instruction="Please choose one option for each statement.";
                //return "Please choose one option for each statement.";
                break;
            case QuestionnaireType.MultiSelect:
                instruction="Please select all options that apply to you for each statement.";
                //return "Please select all options that apply to you for each statement.";
                break;
            default:
                instruction="Welcome!";
                break;
                //return null;
        }

        questionnaire_Completion_Instruction.gameObject.GetComponent<TextMeshProUGUI>().text=instruction;
    }
}
