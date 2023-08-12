using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
public class LSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Rules
    {
        public string Rule;
        public string Definition;
    }

    public LSystemData Data;

    public string CurrentString = "";
    public Dictionary<string, string> RuleDict = new Dictionary<string, string>();

    public void RunItteration()
    {
        if (Data == null)
        {
            Data = ScriptableObject.CreateInstance<LSystemData>();
            return;
        }
        //Prevent running on blank axiom
        if (CurrentString == "") CurrentString = Data.axiom;

        //Split string into array of strings
        string[] output = Regex.Split(CurrentString,String.Empty);
        CurrentString = "";
        //Clear out any blankspaces from the array
        output = output.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        StringBuilder sb = new StringBuilder();


        RuleDict = new Dictionary<string, string>();

        //Add all rules to a dict for faster searching
        foreach (var Rule in Data.RulesList)
        {
            RuleDict.Add(Rule.Rule,Rule.Definition);
        }


        for (int i = 0; i < output.Length ; i++)
        {
            RuleDict.TryGetValue(output[i], out output[i]);
            CurrentString += output[i];
        }

        
    }

    public void restoreAxoim()
    {
        CurrentString = "";
    }

    public void addRule()
    {
        Data.RulesList.Add(new Rules());
    }

    private void Update()
    {
        Debug.Log(CurrentString);
    }
}


class Evaluator : MonoBehaviour
{
    public static void Evaulate<t>(string InputString)
    {
    
    }
}