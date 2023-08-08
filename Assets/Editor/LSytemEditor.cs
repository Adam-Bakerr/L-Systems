using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(LSystem)),CanEditMultipleObjects]
public class LSytemEditor : Editor
{
    EditorWindow editorWindow;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();



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

    private static Vector2 scrollPos = Vector2.zero;
    private string _AxiomInput;



    //Define a style and function to create a horizontal seperator
    // create your style
    GUIStyle horizontalLine;

    private void Awake()
    {
        // create your style
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;

        
    }

    // utility method
    void HorizontalLine(Color color,int width)
    {
        var c = GUI.color;
        GUI.color = color;
        horizontalLine.fixedHeight = width;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }

    void OnGUI()
    {
        
        //List Rules
        Target = Selection.gameObjects[0]?.GetComponent<LSystem>();
        SerializedObject SO = new SerializedObject(Target);
        SerializedProperty Rules = SO.FindProperty("RulesList");

        SO.Update();
        show(Rules);
        SO.ApplyModifiedProperties();

        //Display Current String
        GUILayout.Label(_AxiomInput);

        //Program Controls
        if(GUILayout.Button("Run Itteration"))
        {
            Target.RunItteration();
        }
        //Reset To Axiom
        if (GUILayout.Button("Reset"))
        {
            Target.restoreAxoim();
        }

    }

    //Prevents weird unity issue of holding focus
    void OnLostFocus()
    {
        this.Close();
    }

    //Used to show a list of elements
    public void show(SerializedProperty list)
    {

        HorizontalLine(Color.gray, 10);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(90));


        GUILayout.BeginHorizontal(GUILayout.MaxWidth(150));
        GUILayout.Label("Rules:");
        if(GUILayout.Button("New Rule",GUILayout.Width(75)))
        {
            Target.addRule();
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal("Rules",GUILayout.MaxWidth(Screen.width));
        for (int i = 0; i < list.arraySize; i++)
        {

            SerializedProperty Rule = list.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(Rule, true);
            if (GUILayout.Button("X",GUIStyle.none,GUILayout.Width(10)))
            {
                Rule.DeleteCommand();
            }


            
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        HorizontalLine(Color.gray,10);
        _AxiomInput = EditorGUILayout.TextArea("");
        
        HorizontalLine(Color.gray,10);

    }

    //Save data on closing the gui
    void OnDestory()
    {
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }


}