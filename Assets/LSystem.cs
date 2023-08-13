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

    public List<char> allChars = new List<char>();

    //Evaluation structures

    [System.Serializable]
    public struct charEventPair
    {
        public UnityEvent Event;
        public char Char;
        public charEventPair(char @char) : this()
        {
            Char = @char;
        }
    }

    [SerializeField] public List<charEventPair> evalEvents = new List<charEventPair>();

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
            //Any any new unique char to list of characters
            if (!allChars.Contains(char.Parse(output[i]))) {
                allChars.Add(char.Parse(output[i]));
                evalEvents.Add(new charEventPair(char.Parse(output[i])));
                    }

            //Attempt to replace any rule with a definition
            RuleDict.TryGetValue(output[i], out output[i]);

            //Appened it to the output string
            CurrentString += output[i];
        }

        
    }

    public void restoreAxoim()
    {
        CurrentString = "";
        allChars.Clear();
        evalEvents.Clear();
        Data.axiom = "";
    }

    public void addRule()
    {
        Data.RulesList.Add(new Rules());
    }

    public void evaluate()
    {
        Dictionary<char, UnityEvent> chareventDict = new Dictionary<char, UnityEvent>();

        foreach(charEventPair _eventPair in evalEvents)
        {
            chareventDict.Add(_eventPair.Char,_eventPair.Event);
        }

        foreach(char c in CurrentString)
        {
            chareventDict[c]?.Invoke();
            

            
        }
    }


    public void print(string test)
    {
        Debug.Log(test);
    }
}


