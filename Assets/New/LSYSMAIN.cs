using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LSYSMAIN : MonoBehaviour
{
    [SerializeField] bool Itterate = false;
    [SerializeField] bool Evaluate = false;

    [SerializeField]
    public SerializableDict<char, string> rules = new SerializableDict<char, string>();
    int currentRuleLength = 0;


    [SerializeField]
    public SerializableDict<char, UnityEvent> Expressions = new SerializableDict<char, UnityEvent>();

    [SerializeField]
    string CurrentString = "";

    //Branches
    public char OpenBranch;
    public char ClosingBranch;

    //Data Pointer
    LSYS.Data data;
    
    private void Start()
    {
        data = new LSYS.Data(rules, CurrentString,OpenBranch, ClosingBranch);
        UpdateExpressions();
    }

    // Update is called once per frame
    void Update()
    {
        if(rules.ValuesCount() != currentRuleLength)
        {
            UpdateRules();
            currentRuleLength = rules.ValuesCount();
        }


        if (Itterate)
        {
            
            LSYS.Itterate(ref data);
            CurrentString = data.axiom;
            UpdateExpressions();
            Itterate = false;
        }

        if (Evaluate)
        {
            LSYS.Evaluate(ref data, Expressions);
            Evaluate = false;
        }
    }


    //Used to add new unity events for each character present in string
    void UpdateExpressions()
    {
        //Add any values added in inspector due to how serialized dict works
        foreach(var pair in Expressions.values)
        {
            if (!Expressions.Contains(pair.Key))
            {
                Expressions.AddContents(pair);
            }
        }



        foreach (char c in data.Characters)
        {
            //Skip Intergers
            if (int.TryParse(c.ToString(), out int result)) continue;
            
            if (!Expressions.Contains(c))
            {
                Expressions.Add(c, new UnityEvent());
            }
        }


        

        //Add Branching Opperators
        if (!Expressions.Contains('['))
        {
            Expressions.Add('[', new UnityEvent());
        }
        if (!Expressions.Contains(']'))
        {
            Expressions.Add(']', new UnityEvent());
        }
    }


    public void UpdateRules()
    {
        foreach(var rule in rules.values)
        {
            if (!data.Rules.Contains(rule.Key))
            {
                data.Rules.AddContents(rule);
            }
        }
    }


    public void printString(string Value)
    {
        Debug.Log(Value);
    }
}
