using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CSVManager
{

    public static void Export(List<Data> dataList, string filePath)
    {
        StringBuilder csvContent = new StringBuilder();
        
        // Writing the CSV header
        csvContent.AppendLine("QuestionnaireType,Question,Answer");
        
        // Adding data rows
        foreach (Data data in dataList)
        {
          //  var selectedAnswers = data.Answers.Where(a => a.Value == 1).Select(a => a.Key.ToString()).ToList();
           // string answers = string.Join(";", selectedAnswers);  // Combine selected answers with semicolon

            csvContent.AppendLine($"{Escape(data.QuestionnaireType)},{Escape(data.Question)},{Escape(data.Answers)}");
        }
        
        // Writing to file
        File.WriteAllText(filePath, csvContent.ToString());
    }

    private static string Escape(string field)
    {
        if (field.Contains("\"") || field.Contains(",") || field.Contains("\n") || field.Contains(";"))
        {
            // Escape double quotes and wrap the field in double quotes
            field = $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

}

public class Data
{
    public string QuestionnaireType;
    public string Question;
    public string Answers;  
}

public class DataExporter:MonoBehaviour{

    public static void ExportData(List<GameObject> questions, string ExportedPath){

        List<Data> dataList=new List<Data>();
        foreach (GameObject questionObject in questions)
        {
            QuestionPanel qp=questionObject.GetComponent<QuestionPanel>();

            if(qp.questionType==QuestionType.Likert){
                var selectedAnswers = qp.OptionDictionairy.Where(a => a.Value == 1).Select(a => a.Key.ToString()).ToList();
                string answers = string.Join(";", selectedAnswers);  // Combine selected answers with semicolon
                qp.Answer=answers;
            }

            if(qp.questionType==QuestionType.SingleSelect||qp.questionType==QuestionType.MultiSelect){
                var selectedAnswers = qp.OptionDictionairy.Where(a => a.Value == 1).Select(a => a.Key).ToList();
                List<string> selectedOptions=new List<string>();
                foreach (var item in selectedAnswers)
                {
                    selectedOptions.Add(qp.options[item-1]);
                }
                
                string answers = string.Join(";", selectedOptions);  // Combine selected answers with semicolon
                qp.Answer=answers;
            }

            Data newData=new Data(){QuestionnaireType=qp.questionType.ToString(),Question=qp.QuestionTitle,Answers=qp.Answer};
            dataList.Add(newData);
        }
            //string folderPath= "/Users/haoyangdu/untitled folder";
            // ✅ Step 1: Check if ExportedPath is valid
            string folderPath;
            if (string.IsNullOrEmpty(ExportedPath))
            {
                // Use default folder if ExportedPath is not provided
                folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "untitled folder");
            }
            else
            {
                folderPath = ExportedPath;
            }

            // ✅ Step 2: Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log("Create");
            }

            // ✅ Step 3: Create file name with timestamp
            string baseFileName = "questionnaire_results";
            string dateTimeFormat = "yyyyMMdd_HHmmss";
            string dateTimeString = DateTime.Now.ToString(dateTimeFormat);
            string fileName = $"{baseFileName}_{dateTimeString}.csv";
            string fullPath = Path.Combine(folderPath, fileName);

            // ✅ Step 4: Export CSV
            CSVManager.Export(dataList, fullPath);
            Debug.Log("The data is stored at: " + fullPath);
    
    }

}
