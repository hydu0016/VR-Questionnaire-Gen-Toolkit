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
            var selectedAnswers = data.Answers.Where(a => a.Value == 1).Select(a => a.Key.ToString()).ToList();
            string answers = string.Join(";", selectedAnswers);  // Combine selected answers with semicolon

            csvContent.AppendLine($"{Escape(data.QuestionnaireType)},{Escape(data.Question)},{Escape(answers)}");
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
    public Dictionary<int, int> Answers;  
}

public class DataExporter:MonoBehaviour{

    public static void ExportData(List<GameObject> questions){

        List<Data> dataList=new List<Data>();
        foreach (GameObject questionObject in questions)
        {
            QuestionPanel qp=questionObject.GetComponent<QuestionPanel>();
            Data newData=new Data(){QuestionnaireType=qp.questionnaireType.ToString(),Question=qp.QuestionTitle,Answers=qp.OptionDictionairy};
            dataList.Add(newData);
        }

        CSVManager.Export(dataList,@"D:\try\questionnaire_results.csv");
    }

}
