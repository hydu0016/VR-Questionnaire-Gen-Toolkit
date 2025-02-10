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

    public QuestionType questionType { get; set; }

    public Dictionary<int, int> OptionDictionairy { get; set; }

    public string Answer { get; set; }

    public bool IsAnswered { get; set; }



    //instantiate option!
    //add event to option, notify and modify OptionDictionairy when it is select
    public virtual void SetupPanel(string qustionTitle, GameObject optionPrefab, List<string> options, QuestionType questionType,QuestionnaireQuestion item) {
        if(questionType==QuestionType.Likert || questionType==QuestionType.SingleSelect || questionType==QuestionType.MultiSelect){
            OptionDictionairy = new Dictionary<int, int>();

            //Find Question prefab's title in unity inspector and modify it
            transform.Find("Title/QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
            //Find Questuin prefab's option child object
            Transform OptionsPanel = transform.Find("Options");
            //get its toggle group attached on option child object
            ToggleGroup toggleGroup = OptionsPanel.gameObject.GetComponent<ToggleGroup>();
            //iterate options of this question
            for (int i = 0; i < options.Count; i++)
            {
                int j = i + 1;
                OptionDictionairy.Add(j, 0);

                var optionInstance = Instantiate(optionPrefab, OptionsPanel);

                //Set toggle to a toggle group so that only one of them can switched on at a time
                Toggle optionToggle = optionInstance.GetComponent<Toggle>();
                if (questionType != QuestionType.MultiSelect)
                {
                    optionInstance.GetComponent<Toggle>().group = toggleGroup;
                }

                //Set option description
                optionInstance.transform.Find("Label").GetComponent<Text>().text = options[i];

                int optionNumber = options.IndexOf(options[i]) + 1;

                optionToggle.onValueChanged.AddListener((isSelected) =>
                {
                    if (isSelected)
                    {
                        OptionDictionairy[j] = 1;
                        IsAnswered=true;
                    }
                    else
                    {
                        OptionDictionairy[j] = 0;
                    }
                });
            }
        }

        Answer="null"; //initialize answer is "null", if it still be null, so it means this question is not answered by user.

        if(questionType==QuestionType.Slider_LeftStart||questionType==QuestionType.Slider_MidStart ){

            
            IsAnswered=true;
            transform.Find("QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
            transform.Find("Left").gameObject.GetComponent<TextMeshProUGUI>().text=item.leftLabel;
            transform.Find("Right").gameObject.GetComponent<TextMeshProUGUI>().text=item.rightLabel;

            transform.Find("Slider").gameObject.GetComponent<Slider>().onValueChanged.AddListener(value => Answer = value.ToString("F6"));
            Answer="0.5"; //Set default value is 0.5, user sometimes do not move slider as they think 0.5 is fine.
        }

        if(questionType==QuestionType.InputField){
            transform.Find("QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
            transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().onValueChanged.AddListener(newText =>
            {
                Answer = newText;
                IsAnswered = true; // Mark as answered when the event is triggered
            });
        }

        if(questionType==QuestionType.DropDown){
            IsAnswered = true;
            transform.Find("QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
            TMP_Dropdown myDropdown=transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>();

            Answer=options[0]; //Set Default value for dropdown as first option, otherwise it will considered as uncompleted if user select first option.
            myDropdown.onValueChanged.AddListener(index => Answer = myDropdown.options[index].text);

            myDropdown.ClearOptions();
            myDropdown.AddOptions(options);

        }

        if(questionType==QuestionType.Consent){
            transform.Find("QuestionTitle").gameObject.GetComponent<TextMeshProUGUI>().text = qustionTitle;
            transform.Find("Scroll View/Viewport/Content").gameObject.GetComponent<TextMeshProUGUI>().text = item.consentContent;
            transform.Find("ToggleGroup/Agree").GetComponent<Toggle>().onValueChanged.AddListener((isSelected) =>
                {
                    if (isSelected) // Update only when the toggle is selected
                    {
                        Answer = "Agree";
                        Debug.Log("Selected: " + Answer);
                        IsAnswered = true;
                    }else{
                        IsAnswered = false;
                    }
                });

            transform.Find("ToggleGroup/Disagree").GetComponent<Toggle>().onValueChanged.AddListener((isSelected) =>
                {
                    if (isSelected) // Update only when the toggle is selected
                    {
                        Answer = "Disagree";
                        Debug.Log("Selected: " + Answer);
                        IsAnswered = true;
                    }else{
                        IsAnswered=false;
                    }
                });
        }

    }
}



