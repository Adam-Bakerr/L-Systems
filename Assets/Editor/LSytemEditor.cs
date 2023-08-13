using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager.UI;

[CustomEditor(typeof(LSystem)),CanEditMultipleObjects]
public class LSytemEditor : Editor
{
    EditorWindow editorWindow;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LSystem lSystem = (LSystem)target;

        if (editorWindow == null && GUILayout.Button("Open L-System Editor"))
        {
            editorWindow = EditorWindow.CreateWindow<LSystemWindow>("L-System Editor",
                new[] { System.Type.GetType("UnityEditor.ConsoleWindow, UnityEditor.dll") });

        }



    }
}

[CustomEditor(typeof(LSystem))]
public class LSystemWindow : EditorWindow
{
    public LSystem Target;

    private static Vector2 mainScrollPos = Vector2.zero;
    private static Vector2 scrollPos = Vector2.zero;
    private static Vector2 scrollPosAxInput = Vector2.zero;
    private static Vector2 scrollPosCurrentStringInput = Vector2.zero;
    string fileName = "";

    //Define a style and function to create a horizontal seperator
    // create your style
    GUIStyle horizontalLine;
    Color DefaultGuiColor;
    Color DefaultGuiBackgroundColor;
    //window dimensions

    [SerializeField]
    List<UnityEvent> events;

    private void Awake()
    {

        //Identify Clicked object
        Target = Selection.gameObjects[0]?.GetComponent<LSystem>();

        // create your style
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;

        if (Target.Data == null)
        {
            Target.Data = ScriptableObject.CreateInstance<LSystemData>();
        }


    }

    private void OnValidate()
    {
        Target.RuleDict = new Dictionary<string, string>();
        //Add all rules to a dict that are loaded from a save
        foreach (var Rule in Target.Data.RulesList)
        {
            Target.RuleDict.Add(Rule.Rule, Rule.Definition);
        }
    }

    // utility method
    void HorizontalLine(Color color,int width)
    {
        GUI.color = color;
        horizontalLine.fixedHeight = width;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = DefaultGuiColor;
    }

    void OnGUI()
    {
     
        DefaultGuiColor = GUI.color;
        DefaultGuiBackgroundColor = GUI.backgroundColor;
        SerializedObject SO = new SerializedObject(Target.Data);
        SerializedProperty Rules = SO.FindProperty("RulesList");
        SO.Update();
        GUI.color = Color.black;
        GUILayout.BeginArea(new Rect(30,30,position.width-60,position.height-60),GUI.skin.box);
        GUI.color = DefaultGuiColor;
        mainScrollPos = GUILayout.BeginScrollView(mainScrollPos);
        show(Rules);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        SO.ApplyModifiedProperties();


    }

    //Prevents weird unity issue of holding focus
    void OnLostFocus()
    {
        this.Close();
    }

    //Used to show a list of elements
    public void show(SerializedProperty list)
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(150));
        //GUI.backgroundColor = new Color(50 ,50, 50);
        GUILayout.Box("Rules:");
        GUI.backgroundColor = Color.black;
        if(GUILayout.Button("New Rule", GUI.skin.box, GUILayout.Width(75)))
        {
            
            Target.addRule();
        }

        GUI.backgroundColor = DefaultGuiBackgroundColor;
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal("Rules",GUILayout.MaxWidth(position.width));
        for (int i = 0; i < list.arraySize; i++)
        {

            SerializedProperty Rule = list.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(Rule, true);
            if (GUILayout.Button("X", GUI.skin.box, GUILayout.Width(20)))
            {
                Rule.DeleteCommand();
            }

        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        HorizontalLine(Color.grey, 5);
        scrollPosAxInput = EditorGUILayout.BeginScrollView(scrollPosAxInput, GUILayout.Height(30));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Box("Axiom:");

        //Listen for changed text
        EditorGUI.BeginChangeCheck();
        Target.Data.axiom = EditorGUILayout.TextArea(Target.Data.axiom, GUI.skin.textField, new GUILayoutOption[] { GUILayout.Height(25) });
        if (EditorGUI.EndChangeCheck())
        {
            //Reset current string
            Target.CurrentString = "";

            //Add all unique chars to character set
            for (int i = 0; i < Target.Data.axiom.Length; i++)
            {
                if (!Target.allChars.Contains(Target.Data.axiom[i]))
                {
                    Target.allChars.Add(Target.Data.axiom[i]);
                    Target.evalEvents.Add(new LSystem.charEventPair(Target.Data.axiom[i]));
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        GUILayout.Box("Branching Operators:");
        GUILayout.BeginHorizontal();
        GUILayout.Box("Opening",new GUILayoutOption[]{ GUILayout.Width(60) , GUILayout.Height(20) });
        Target.Data.openBranch = EditorGUILayout.TextArea(Target.Data.openBranch, GUI.skin.textField , new GUILayoutOption[]{ GUILayout.Height(25) , GUILayout.Width(25)});
        GUILayout.Box("Closing");
        Target.Data.closeBranch = EditorGUILayout.TextArea(Target.Data.closeBranch,GUI.skin.textField, new GUILayoutOption[] { GUILayout.Height(25), GUILayout.Width(25) });
        GUILayout.EndHorizontal();
        HorizontalLine(Color.grey,5);

        //Display Current String
        scrollPosCurrentStringInput = GUILayout.BeginScrollView(scrollPosCurrentStringInput,new GUILayoutOption[]{ GUILayout.Height(50) , GUILayout.Width(position.width - 75) });
        GUI.backgroundColor = Color.black *.8f;
        GUILayout.Box(Target.CurrentString, GUILayout.Width(position.width - 105));
        GUI.backgroundColor = DefaultGuiBackgroundColor;
        GUILayout.EndScrollView();

        //Program Controls
        if (GUILayout.Button("Run Itteration", GUI.skin.box))
        {
            Target.RunItteration();
        }

        //Run Custom Evaluation on current string
        if (GUILayout.Button("Evalualte", GUI.skin.box))
        {
            Target.evaluate();
        }

        //Reset To Axiom
        if (GUILayout.Button("Reset", GUI.skin.box))
        {
            Target.restoreAxoim();
        }

        HorizontalLine(Color.grey, 5);
        //Save
        GUILayout.BeginHorizontal();
        if(fileName == "") GUI.enabled = false;
        if (GUILayout.Button("Save", GUI.skin.box, GUILayout.Width(180)))
        {
            Target.Data.Save(fileName);
        }
        GUI.enabled = true;
        fileName = GUILayout.TextArea(fileName, GUILayout.Height(20));
        GUILayout.EndHorizontal();

        //Load
        GUILayout.BeginHorizontal();
        if (fileName == "") GUI.enabled = false;
        if (GUILayout.Button("Load", GUI.skin.box, GUILayout.Width(180)))
        {
            Target.Data.Load(fileName);
        }
        GUI.enabled = true;
        fileName = GUILayout.TextArea(fileName,GUILayout.Height(20));
        GUILayout.EndHorizontal();
    }

    //Save data on closing the gui
    void OnDestory()
    {
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }


}