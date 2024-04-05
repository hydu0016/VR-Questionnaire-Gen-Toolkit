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

        //设置question title  设置生成几个option 以及每个option title
        transform.Find("Title/QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
        //找到需要生成toggle的父物体Transform
        Transform OptionsPanel = transform.Find("Options");
        //父物体上的toogle group找到统一设置成一个group
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

            //给此option添加事件，所有后面点击时可以存储数据
            //数据格式：  问卷Index，问题index, 选择index
            //数据格式：问题index,选择index.
            int optionNumber = options.IndexOf(options[i]) + 1;
            //  Debug.Log("问题index"+ questionNumber+"答案index" + optionNumber);
            Debug.Log($"Adding listener to option {j}" + $"此option所属问卷{questionnaireType}");
            optionToggle.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    // Update the responses dictionary using questionNumber as the key.
                    OptionDictionairy[j] = 1;
                    Debug.Log("我被选中了");

                }
                else
                {
                    OptionDictionairy[j] = 0;
                    Debug.Log("我被取消了了");
                }
            });
        }
    }
}



