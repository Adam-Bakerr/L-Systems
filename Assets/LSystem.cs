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

    public static List<Rules> RulesList = new List<Rules>();
    public string axiom;
    public void RunItteration()
    {

    }

    public static void addRule()
    {
        RulesList.Add(new Rules());
    }
}
