using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Rules
    {
        public string Rule;
        public string Definition;
    }

    public List<Rules> RulesList = new List<Rules>();
    public string axiomxd = "";
    public string CurrentString = "";
    public void RunItteration()
    {
        if(axiomxd == null)
        {
            return;
        }
        if (CurrentString == "") CurrentString = axiomxd;
    }

    public void restoreAxoim()
    {

    }

    public void addRule()
    {
        RulesList.Add(new Rules());
    }

    private void Update()
    {
        Debug.Log(CurrentString);
    }
}
