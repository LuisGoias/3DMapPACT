using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelperSerialize
{
    public List<string> questions;
    public List<string> answers;


    public void setQuestions(List<string> newQuestions)
    {
        questions = new List<string>(newQuestions);
    }


    public void setAnswers(List<string> newAnwsers)
    {
        answers = new List<string>(newAnwsers);
    }

    public void addQuestion(string newQuestion)
    {
        if (!questions.Contains(newQuestion)) {
        
            questions.Add(newQuestion);
        
        }
    }

    public void addAnwser(string newAnwser)
    {
        if(!answers.Contains(newAnwser))
        {
            answers.Add(newAnwser);
        }
    }



}
