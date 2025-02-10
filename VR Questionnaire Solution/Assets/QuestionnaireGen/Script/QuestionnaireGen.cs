using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class QuestionnaireGen : MonoBehaviour
{

   // public QuestionnaireConfiguration questionnaireConfiguration; // Assigned from the editor
    //newnew
    public NewQuesConfig newQuesConfig;

    private GameObject questionnaire_Completion_Instruction;

    public GameObject questionPanelPrefab_likert;    //need option gen when instantiate this item
    public GameObject questionPanelPrefab_singleSelect;  //need option gen when instantiate this item
    public GameObject questionPanelPrefab_multiSelect;         //need option gen when instantiate this item
    public GameObject questionPanelPrefab_Slider_LeftStart;   //No need option gen when instantiate this item
    public GameObject questionPanelPrefab_Slider_MidStart;    //No need option gen when instantiate this item
    public GameObject questionPanelPrefab_InputField;         //No need option gen when instantiate this item
    public GameObject questionPanelPrefab_DropDown;         //No need option gen when instantiate this item
    public GameObject questionPanelPrefab_Consent;          //No need option gen when instantiate this item


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
        //string username = Environment.UserName;
        //Debug.Log("Current User: " + username);

        questionnaire_Completion_Instruction=transform.Find("Base/Head/Head Title").gameObject;
        nextButton=transform.Find("Base/Button_Next").gameObject.GetComponent<Button>();
        previousButton=transform.Find("Base/Button_Previous").gameObject.GetComponent<Button>();
        submitButton=transform.Find("Base/Button_Submit").gameObject.GetComponent<Button>();
        questionnairePanel=transform.Find("Base/QuestionDisplayArea").gameObject.transform;

        //LoadQuestionPrefabIntoQuestionList();  //GENERATE all questions
        //AssignPageNumberToQuestionPrefab();   // Assign page numbers to questions
       // test();

       AssignValue2QuestionPrefabs(newQuesConfig);
        AssignPageNumbers();

        DisplayFirstPageQuestions();    //show first pages

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
            Debug.Log("总Index" + item.GetComponent<QuestionPanel>().QuestionIndex + "所属问卷编号" + item.GetComponent<QuestionPanel>().QuestionnaireIndex + "所属问卷类型" + item.GetComponent<QuestionPanel>().questionType + "所属页码" + item.GetComponent<QuestionPanel>().PageIndex);
        }

    }


    //Iterate questionnaires list
    // private void LoadQuestionPrefabIntoQuestionList()
    // {
    //     //Iterate each questionnaire
    //     for (int i = 0; i < questionnaireConfiguration.questionnaires.Count; i++)
    //     {
    //         //iterate each question in this questionnaire
    //         AssignValueToQuestionPrefab(questionnaireConfiguration.questionnaires[i]);
    //     }
    // }


    // //Add info to each questions in each questionnaires
    // private void AssignValueToQuestionPrefab(Questionnaire questionnaire)
    // {
        
    //     foreach (var question in questionnaire.questions)
    //     {

    //         GameObject questionPrefab = Instantiate(GetQuestionPrefabForQuestionnaireType(questionnaire.type), questionnairePanel);////Create a question prefab object under questionnairePanel
    //         QuestionPanel qp = questionPrefab.GetComponent<QuestionPanel>(); //Get component of QuestionPrefab
    //         qp.QuestionIndex = questionNumber++;
    //         qp.QuestionnaireIndex = questionnaireConfiguration.questionnaires.IndexOf(questionnaire)+1;
    //         qp.questionnaireType = questionnaire.type;
    //         qp.QuestionTitle = question.questionTitle;
    //         qp.optionPrefab = GetOptionPrefabForQuestionnaireType(questionnaire.type);
    //         qp.options = question.options;

            
    //         questionPrefab.GetComponent<QuestionPanel>().SetupPanel(qp.QuestionTitle, qp.optionPrefab,qp.options,qp.questionnaireType);
    //         questionPrefab.SetActive(false);
    //         questionPanels.Add(questionPrefab);
    //     }
    // }

//newnew
    private void AssignValue2QuestionPrefabs(NewQuesConfig newQuesConfig){
        List<QuestionnaireQuestion> questions = new List<QuestionnaireQuestion>();
        foreach (var item in newQuesConfig.questions)
        {
            GameObject questionPrefab=Instantiate(GetQuestionPrefabForQuestionnaireType(item.questionType), questionnairePanel);
            QuestionPanel qp = questionPrefab.GetComponent<QuestionPanel>();
            qp.QuestionIndex = questionNumber++;
            qp.questionType = item.questionType;
            qp.QuestionTitle = item.questionTitle;
            qp.optionPrefab = GetOptionPrefabForQuestionnaireType(item.questionType);
            qp.options= item.options;

            questionPrefab.GetComponent<QuestionPanel>().SetupPanel(qp.QuestionTitle, qp.optionPrefab,qp.options,qp.questionType,item);
            questionPrefab.SetActive(false);
            questionPanels.Add(questionPrefab);            
        }
    }


    private GameObject GetQuestionPrefabForQuestionnaireType(QuestionType type)
    {
        switch (type)
        {
            case QuestionType.Likert:
                return questionPanelPrefab_likert;
            case QuestionType.SingleSelect:
                return questionPanelPrefab_singleSelect;
            case QuestionType.MultiSelect:
                return questionPanelPrefab_multiSelect;
            case QuestionType.Slider_LeftStart:
                return questionPanelPrefab_Slider_LeftStart;
            case QuestionType.Slider_MidStart:
                return questionPanelPrefab_Slider_MidStart;
            case QuestionType.InputField:
                return questionPanelPrefab_InputField;
            case QuestionType.DropDown:
                return questionPanelPrefab_DropDown;
            case QuestionType.Consent:
                return questionPanelPrefab_Consent;
            default:
                return null; 
        }
    }

    private GameObject GetOptionPrefabForQuestionnaireType(QuestionType type)
    {
        switch (type)
        {
            case QuestionType.Likert:
                return optionPrefab_likert;
            case QuestionType.SingleSelect:
                return optionPrefab_singleSelect;
            case QuestionType.MultiSelect:
                return optionPrefab_multiSelect;
            case QuestionType.Slider_LeftStart:
                return null;
            case QuestionType.Slider_MidStart:
                return null;
            case QuestionType.InputField:
                return null;
            case QuestionType.DropDown:
                return null;
            case QuestionType.Consent:
                return null;
            default:
                return null; 
        }
    }



    int maxAssignedPage=0;
    public void AssignPageNumbers()
    {
        var questions=questionPanels;

        if (questions == null || questions.Count == 0)
            return;

        int currentPage = 1;
        // Start with the type of the first question.
        QuestionType currentGroupType = questions[0].GetComponent<QuestionPanel>().questionType;
        int countOnPage = 0;

        foreach (var question in questions)
        {
            // If the question type changes compared to the current group,
            // we start a new page even if the current page hasn't reached its capacity.
            if (question.GetComponent<QuestionPanel>().questionType != currentGroupType)
            {
                currentPage++;
                currentGroupType = question.GetComponent<QuestionPanel>().questionType;
                countOnPage = 0;
            }

            // Get the maximum number of questions allowed on a page for this type.
            int capacity = GetCapacityForQuestionType(question.GetComponent<QuestionPanel>().questionType);

            // If we've reached the capacity for the current page, start a new page.
            if (countOnPage >= capacity)
            {
                currentPage++;
                countOnPage = 0;
            }

            // Assign the current page number to the question.
            question.GetComponent<QuestionPanel>().PageIndex=currentPage;
            countOnPage++;
        }
        
        foreach (var item in questions)
        {
            if(item.GetComponent<QuestionPanel>().PageIndex>maxAssignedPage){
                maxAssignedPage=item.GetComponent<QuestionPanel>().PageIndex;
            }
            
        }
    }

    private int GetCapacityForQuestionType(QuestionType type)
    {
        switch (type)
        {
            case QuestionType.Likert:
            case QuestionType.InputField:
            case QuestionType.DropDown:
                return 3;
            case QuestionType.Slider_LeftStart:
            case QuestionType.Slider_MidStart:
                return 2;
            case QuestionType.Consent:
            case QuestionType.SingleSelect:
            case QuestionType.MultiSelect:
                return 1;
            default:
                return 1;
        }
    }


    // private int currentQuestionIndex = 0;
    // private int pageNumber = 1;

    // private int tmp = 0;
    // private int maxAssignedPage = 1; // Stores the highest page index that actually contains questions

    // private void AssignPageNumberToQuestionPrefab()
    // {
    //     maxAssignedPage = 1; // Reset before assigning new page numbers

    //     for (int i = 0; i < questionnaireConfiguration.questionnaires.Count; i++)
    //     {
    //         int questionsPerPage;

    //         switch (questionnaireConfiguration.questionnaires[i].type)
    //         {
    //             case QuestionType.Likert:
    //             case QuestionType.InputField:
    //             case QuestionType.DropDown:
    //                 questionsPerPage = 3;
    //                 break;
    //             case QuestionType.Slider_LeftStart:
    //             case QuestionType.Slider_MidStart:
    //                 questionsPerPage = 2;
    //                 break;
    //             case QuestionType.Consent:
    //                 questionsPerPage = 1;
    //                 break;
    //             default:
    //                 questionsPerPage = 1;
    //                 break;
    //         }

    //         for (int j = 0; j < questionnaireConfiguration.questionnaires[i].questions.Count; j++)
    //         {
    //             if (currentQuestionIndex < questionPanels.Count)
    //             {
    //                 questionPanels[currentQuestionIndex].GetComponent<QuestionPanel>().PageIndex = pageNumber;
    //                 maxAssignedPage = Math.Max(maxAssignedPage, pageNumber); // Track highest used page
    //             }

    //             tmp++;

    //             if (tmp == questionsPerPage)
    //             {
    //                 pageNumber++;
    //                 tmp = 0;
    //             }

    //             currentQuestionIndex++;
    //         }

    //         if (tmp > 0) 
    //         {
    //             pageNumber++;
    //             maxAssignedPage = Math.Max(maxAssignedPage, pageNumber); // Update maxAssignedPage
    //         }

    //         tmp = 0;
    //     }

    //     if (questionPanels.Count > 0)
    //     {
    //         maxAssignedPage = Math.Max(maxAssignedPage, questionPanels.Last().GetComponent<QuestionPanel>().PageIndex);
    //     }
    // }






    
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

    void NextButtonPressed()
    {
        List<GameObject> currentPageQuestions = GetExistingQuestions();

        // Check if all questions are completed
        if (!areAllQuestionsCompleted(currentPageQuestions))
        {
            WarnUnansweredQuestion(currentPageQuestions);
            return;
        }

        // Clear questions on the current page
        ClearExistingQuestions(currentPageQuestions);

        // Move to the next page
        int nextPageNumber = ++currentPage;

        // Ensure last page is the page containing the last question
        if (nextPageNumber== maxAssignedPage)
        {
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
            //return; // Prevent displaying an empty page
        }

        // Display questions on the next page
        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == nextPageNumber)
            {
                item.SetActive(true);
            }
        }

        DisplayQuestionnaireCompletionInstruction(currentPage);
    }

    bool areAllQuestionsCompleted(List<GameObject> currentPageQuestions){

        foreach (var item in currentPageQuestions)
        {
            if(!item.GetComponent<QuestionPanel>().IsAnswered){return false;}

        }

        return true;
    }

    void WarnUnansweredQuestion(List<GameObject> currentPageQuestions){
        
        List<GameObject> unAnsweredQuestions=new List<GameObject>();

        foreach (var item in currentPageQuestions)
        {
            if(!item.GetComponent<QuestionPanel>().IsAnswered){unAnsweredQuestions.Add(item);}
        }
        
        foreach (var item in unAnsweredQuestions)
        {
            // Try to find "Title/QuestionTitle" first
            Transform titleTransform = item.transform.Find("Title/QuestionTitle");

            // If not found, try "QuestionTitle"
            if (titleTransform == null)
            {
                titleTransform = item.transform.Find("QuestionTitle");
            }

            // Proceed only if we found a valid titleTransform
            if (titleTransform != null)
            {
                var questionTitle = titleTransform.GetComponent<TextMeshProUGUI>();
                var defaultColor = questionTitle.color;

                StartCoroutine(FlashTitleColor(questionTitle, defaultColor));
            }
            else
            {
                Debug.LogError($"Question title not found in {item.name}");
            }
        }        

    }
    //tool function used for warning uncompleted questions
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

    //tool function used for warning uncompleted questions
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


        
        ClearExistingQuestions(GetExistingQuestions());
        currentPage--;

        if (currentPage!=maxAssignedPage)
        {
            submitButton.gameObject.SetActive(false);
        }

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

        QuestionType type=newQuesConfig.questions[0].questionType;
        DisplayQuestionnaireCompletionInstruction(1);
    }

    void DisplayQuestionnaireCompletionInstruction(int pageNumber){
        
        string instruction="";
        QuestionType type=newQuesConfig.questions[0].questionType;

        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == pageNumber)
            {
                type=item.GetComponent<QuestionPanel>().questionType;
                break;
            }
        }

        switch (type)
        {
            case QuestionType.Likert:
                instruction = "Please rate each statement by selecting one option.";
                break;
            case QuestionType.SingleSelect:
                instruction = "Please choose one option for each statement.";
                break;
            case QuestionType.MultiSelect:
                instruction = "Please select all options that apply to you for each statement.";
                break;
            case QuestionType.Consent:
                instruction = "Please read the consent form below and indicate your agreement.";
                break;
            case QuestionType.Slider_LeftStart:
                instruction = "Please adjust the slider starting from the left to indicate your response.";
                break;
            case QuestionType.Slider_MidStart:
                instruction = "Please adjust the slider starting from the middle to indicate your response.";
                break;
            case QuestionType.DropDown:
                instruction = "Please select an option from the dropdown menu.";
                break;
            case QuestionType.InputField:
                instruction = "Please enter your response in the provided input field.";
                break;
            default:
                instruction = "Welcome!";
                break;
        }


        questionnaire_Completion_Instruction.gameObject.GetComponent<TextMeshProUGUI>().text=instruction;
    }
}
