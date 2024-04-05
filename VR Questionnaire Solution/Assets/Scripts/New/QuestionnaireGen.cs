using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestionnaireGen : MonoBehaviour
{

    public QuestionnaireConfiguration questionnaireConfiguration; // Assigned from the editor

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

    //GameObject[] questionPanel=new GameObject();

    List<GameObject> questionPanels = new List<GameObject>();

    void Start()
    {
        //InitializeDataStorageDictionary();

        /*
        LoadQuestionnaire();
        nextButton.onClick.AddListener(NextButtonPressed);
        previousButton.onClick.AddListener(PreviousButtonPressed);
        submitButton.onClick.AddListener(SubmitButtonPressed);
        submitButton.gameObject.SetActive(false); // Initially hide submit button
        previousButton.gameObject.SetActive(false); // Initially hide previous button
        */
        
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
            Debug.Log("��Index" + item.GetComponent<QuestionPanel>().QuestionIndex + "�����ʾ���" + item.GetComponent<QuestionPanel>().QuestionnaireIndex + "�����ʾ�����" + item.GetComponent<QuestionPanel>().questionnaireType + "����ҳ��" + item.GetComponent<QuestionPanel>().PageIndex);
        }

    }

    private void InitializeDataStorageDictionary()
    {
        int i = 0;
        foreach (var questionnaire in questionnaireConfiguration.questionnaires)
        {
            foreach (var item in questionnaire.questions)
            {
                DataStorageManager.Instance.questionnaireData.Add(i, 0);
                i++;
            }
        }
    }

    //�ȿ��м����ʾ� ÿ���ʾ������
    //��ÿ���ʾ������
    //����questionPrefab,ʹ��ȫ������questionnairepannel���棬 ��������ǩ QuestionIndex, ��¼��prefab����
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

    //���ǩ֮�� �����ҳ  ���ҳ��ǩ page index
    //��һҳ  �ڶ�ҳ  ����ҳ   ����ҳ   ����ҳ   ����ҳ   ����ҳ    �ڰ�ҳ ...


    //�ȿ���һ��questionnaire
    //�����likert  ��ô
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
        //List<GameObject> currentPageQuestions = new List<GameObject>();
        //foreach (Transform child in questionnairePanel)
        //{
        //    if (child.gameObject.activeSelf)
        //    {
        //        // If active, add it to the list
        //        currentPageQuestions.Add(child.gameObject);

        //        child.gameObject.SetActive(false);
        //    }
        //}

        //return currentPageQuestions;

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
        ////�鿴��ǰҳ�������Ƿ�ȫ����ѡ
        foreach (var item in currentPageQuestions)
        {
            if (item.GetComponent<QuestionPanel>().OptionDictionairy.All(kvp => kvp.Value == 0))
            {
                // If not, log a message indicating an action needs to be taken
                Debug.Log("Action needs to be done");
                // Return from the function if you need to terminate its execution here
                return;
            }
        }

        //�����ǰҳ����ʾ������
        ClearExistingQuestions(currentPageQuestions);

        //����һҳ�Ƿ�Ϊ���һҳ����������һҳ��ô������ύ
        int nextPageNumber= ++currentPage;
        if (nextPageNumber==pageNumber)
        {
            //�� submissionbutton
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
           // return;
        }

        //����һҳ�����⼤��
        foreach (var item in questionPanels)
        {
            if (item.GetComponent<QuestionPanel>().PageIndex == nextPageNumber)
            {
                item.SetActive(true);
            }
        }


        Debug.Log(currentPage);

    }

    void PreviousButtonPressed()
    {
        //�����ǰ�Ѿ��ǵ�һҳ�ˣ���ôcurrentPage�������ڵ�һҳ�����ش˷�����
        if (currentPage == 1)
        {
            return;
        }

        nextButton.gameObject.SetActive(true);
        //�����ǰҳ�������һҳ����ô�����ύ��ť
        if (currentPage==pageNumber)
        {
            submitButton.gameObject.SetActive(false);
        }

        //�����ǰҳ����ʾ������
        ClearExistingQuestions(GetExistingQuestions());
        currentPage--;

        //����һҳ�����⼤��
        foreach (var item in questionPanels)
            {
                if (item.GetComponent<QuestionPanel>().PageIndex == currentPage)
                {
                    item.SetActive(true);
                }
            }
        Debug.Log(currentPage);

    }

    void SubmitButtonPressed()
    {
        DataStorageManager dataStorageManager = DataStorageManager.Instance;
        dataStorageManager.PrintAllQuestionnaireResults(questionPanels);
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

    }
}
