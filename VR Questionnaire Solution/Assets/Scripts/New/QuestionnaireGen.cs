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

    public GameObject questionnaire_Completion_Instruction;

    public GameObject questionPanelPrefab_likert;
    public GameObject questionPanelPrefab_singleSelect;
    public GameObject questionPanelPrefab_multiSelect;

    public GameObject optionPrefab_likert;
    public GameObject optionPrefab_singleSelect;
    public GameObject optionPrefab_multiSelect;

    public Transform questionnairePanel; // Parent panel for question panels
    public Button nextButton;
    public Button previousButton;
    public Button submitButton;
    int questionNumber = 1;
    int currentPage=1;

   float  flashDuration=0.15f;
   public Color flashColorForUnansweredQuestion=Color.red;

    List<GameObject> questionPanels = new List<GameObject>();

    void Start()
    {       
        LoadQuestionPrefabIntoQuestionList();
        AssignPageNumberToQuestionPrefab();
        test();
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

    // private void InitializeDataStorageDictionary()
    // {
    //     int i = 0;
    //     foreach (var questionnaire in questionnaireConfiguration.questionnaires)
    //     {
    //         foreach (var item in questionnaire.questions)
    //         {
    //             DataStorageManager.Instance.questionnaireData.Add(i, 0);
    //             i++;
    //         }
    //     }
    // }

    //先看有几个问卷 每个问卷的类型
    //按每个问卷的类型
    //生成questionPrefab,使其全部挂在questionnairepannel下面， 并给其打标签 QuestionIndex, 并录入prefab数组
    private void LoadQuestionPrefabIntoQuestionList()
    {
        //Iterate each questionnaire
        for (int i = 0; i < questionnaireConfiguration.questionnaires.Count; i++)
        {
            //iterate each question in this questionnaire
            AssignValueToQuestionPrefab(questionnaireConfiguration.questionnaires[i]);
        }
    }
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

    //打标签之后 给其分页  打分页标签 page index
    //第一页  第二页  第三页   第四页   第五页   第六页   第七页    第八页 ...


    //先看第一个questionnaire
    //如果是likert  那么
    //
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

        //查看是否完成所有问题
        if (!areAllQuestionsCompleted(currentPageQuestions))
        {
            WarnUnansweredQuestion(currentPageQuestions);
            return;
        }
        

        //清除当前页面显示的问题
        ClearExistingQuestions(currentPageQuestions);

        //看下一页是否为最后一页，如果是最后一页那么则可以提交
        int nextPageNumber= ++currentPage;
        if (nextPageNumber==pageNumber)
        {
            //变 submissionbutton
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
           // return;
        }

        //把下一页的问题激活
        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == nextPageNumber)
            {
                item.SetActive(true);
            }
        }

        DisplayQuestionnaireCompletionInstruction(currentPage);
        Debug.Log(currentPage);

    }

    bool areAllQuestionsCompleted(List<GameObject> currentPageQuestions){
                ////查看当前页面问题是否全部勾选
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
        //如果当前已经是第一页了，那么currentPage还保持在第一页，返回此方法。
        if (currentPage == 1)
        {
            return;
        }

        nextButton.gameObject.SetActive(true);
        //如果当前页面是最后一页，那么隐藏提交按钮
        if (currentPage==pageNumber)
        {
            submitButton.gameObject.SetActive(false);
        }

        //清除当前页面显示的问题
        ClearExistingQuestions(GetExistingQuestions());
        currentPage--;

        //把上一页的问题激活
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

       // DataStorageManager dataStorageManager = DataStorageManager.Instance;
        //dataStorageManager.PrintAllQuestionnaireResults(questionPanels);
        
        //转化数据
        DataExporter.ExportData(questionPanels);
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
