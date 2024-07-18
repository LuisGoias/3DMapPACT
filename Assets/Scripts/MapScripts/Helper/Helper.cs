using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Helper")]
public class Helper : ScriptableObject
{
   public List<string> phrases;


    public void setPhrases(List<string> newPhrases)
    {
        phrases = new List<string>(newPhrases);
    }

    public void joinPhrases(List<string> newPhrases)
    {
        for (int i = 0; i < newPhrases.Count; i++)
        {
            if (!phrases.Contains(newPhrases[i]))
            {
                phrases.Add(newPhrases[i]);
            }
        }
    }
}
