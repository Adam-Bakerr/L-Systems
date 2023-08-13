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
        rules.Add('a',"aba");
        rules.Add('b', "bbb");
        data = new LSYS.Data(rules, CurrentString,OpenBranch, ClosingBranch);
        UpdateExpressions();
    }

    // Update is called once per frame
    void Update()
    {
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
        foreach (char c in data.Characters)
        {
            if (!Expressions.Contains(c))
            {
                Expressions.Add(c, new UnityEvent());
            }
        }
    }


    public void printString(string Value)
    {
        Debug.Log(Value);
    }
}
