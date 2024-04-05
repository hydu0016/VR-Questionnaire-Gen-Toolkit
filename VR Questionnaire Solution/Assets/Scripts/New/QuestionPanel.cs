using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class QuestionPanel : MonoBehaviour
{
    public GameObject optionPrefab { get; set; }
    public int PageIndex { get; set; }
    public int QuestionIndex { get; set; }

    public List<string> options { get; set; }
    public int QuestionnaireIndex { get; set; }

    public string QuestionTitle { get; set; }

    public QuestionnaireType questionnaireType { get; set; }

    public Dictionary<int, int> OptionDictionairy { get; set; }



    //instantiate option!
    //add event to option, notify and modify OptionDictionairy when it is select
    public virtual void SetupPanel(string qustionTitle, GameObject optionPrefab, List<string> options, QuestionnaireType questionnaireType) {

        OptionDictionairy = new Dictionary<int, int>();

        //����question title  �������ɼ���option �Լ�ÿ��option title
        transform.Find("Title/QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
        //�ҵ���Ҫ����toggle�ĸ�����Transform
        Transform OptionsPanel = transform.Find("Options");
        //�������ϵ�toogle group�ҵ�ͳһ���ó�һ��group
        ToggleGroup toggleGroup = OptionsPanel.gameObject.GetComponent<ToggleGroup>();


        //iterate options of this question
        for (int i = 0; i < options.Count; i++)
        {
            int j = i + 1;
            OptionDictionairy.Add(j, 0);

            var optionInstance = Instantiate(optionPrefab, OptionsPanel);

            //Set toggle to a toggle group so that only one of them can switched on at a time
            Toggle optionToggle = optionInstance.GetComponent<Toggle>();
            if (questionnaireType != QuestionnaireType.MultiSelect)
            {
                optionInstance.GetComponent<Toggle>().group = toggleGroup;
            }

            //Set option description
            optionInstance.transform.Find("Label").GetComponent<Text>().text = options[i];

            //����option����¼������к�����ʱ���Դ洢����
            //���ݸ�ʽ��  �ʾ�Index������index, ѡ��index
            //���ݸ�ʽ������index,ѡ��index.
            int optionNumber = options.IndexOf(options[i]) + 1;
            //  Debug.Log("����index"+ questionNumber+"��index" + optionNumber);
            Debug.Log($"Adding listener to option {j}" + $"��option�����ʾ�{questionnaireType}");
            optionToggle.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    // Update the responses dictionary using questionNumber as the key.
                    OptionDictionairy[j] = 1;
                    Debug.Log("�ұ�ѡ����");

                }
                else
                {
                    OptionDictionairy[j] = 0;
                    Debug.Log("�ұ�ȡ������");
                }
            });
        }
    }
}



