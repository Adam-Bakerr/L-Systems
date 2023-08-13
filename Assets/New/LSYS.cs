using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LSYS
{
    [System.Serializable]
    public struct Data
    {
        [HideInInspector]
        public SerializableDict<char,string> Rules;

        public string axiom;

        //Opening and closing characters for evaulation
        public char OpeningBranch;
        public char ClosingBranch;

        public List<char> Characters;

        public Data(SerializableDict<char, string> rules, string axiom, char closingBranch, char openingBranch)
        {
            Characters = new List<char>();
            Rules = rules;
            this.axiom = axiom;

            //Add any Unique Characters From Axiom To Character List
            foreach (char c in axiom)
            {
                if (!Characters.Contains(c))
                {
                    Characters.Add(c);
                }
            }

            ClosingBranch = closingBranch;
            OpeningBranch = openingBranch;
        }
    }

    public static void Itterate(ref Data data)
    {
        string _Output = new string("");
        foreach(char c in data.axiom)
        {
            if (data.Rules.Contains(c))
            {
                _Output += data.Rules.GetValue(c);
            }
            else
            {
                _Output += c;
            }

            if (!data.Characters.Contains(c))
            {
                data.Characters.Add(c);
            }
        }
        data.axiom = _Output;
    }

    public static void Evaluate(ref Data data , SerializableDict<char, UnityEvent> Expressions)
    {
        foreach(char c in data.axiom)
        {
            if (Expressions.Contains(c))
            {
                Expressions.GetValue(c)?.Invoke();
            }
        }
    }
}
