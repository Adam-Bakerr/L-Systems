using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class LSystemData : ScriptableObject
{

    private class data
    {
        public string axiom = "";
        public List<LSystem.Rules> RulesList = new List<LSystem.Rules>();
        public string openBranch;
        public string closeBranch;
    }

    private data thisData;
    public List<LSystem.Rules> RulesList = new List<LSystem.Rules>();
    public string axiom = "";
    public string openBranch;
    public string closeBranch;

    //Parse l system to and from json
    public void Save(string filename)
    {
        thisData = new data();
        thisData.RulesList = RulesList;
        thisData.axiom = axiom;
        thisData.closeBranch = closeBranch;
        thisData.openBranch = openBranch;
        System.IO.File.WriteAllText(Application.dataPath + "/" + filename + ".json", EditorJsonUtility.ToJson(thisData));
    }

    public void Load(string filename)
    {
        string data = System.IO.File.ReadAllText(Application.dataPath + "/" + filename + ".json");
        thisData = JsonUtility.FromJson<data>(data);
        this.RulesList = thisData.RulesList;
        this.axiom = thisData.axiom;
        this.openBranch = thisData.openBranch;
        this.closeBranch = thisData.closeBranch;
    }
}
