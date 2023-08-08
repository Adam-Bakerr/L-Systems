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

    void OnGUI()
    {
        
        LSystem Target = Selection.gameObjects[0]?.GetComponent<LSystem>();
        SerializedObject SO = new SerializedObject(Target);
        SerializedProperty Rules = SO.FindProperty("RulesList");

        SO.Update();
        show(Rules);
        SO.ApplyModifiedProperties();

    }

    //Prevents weird unity issue of holding focus
    void OnLostFocus()
    {
        this.Close();
    }

    //Used to show a list of elements
    public static void show(SerializedProperty list)
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));

        GUILayout.Label("Rules:");
        if(GUILayout.Button("New Rule"))
        {
            Target.addRule();
        }
        GUILayout.BeginHorizontal("Rules",GUILayout.MaxWidth(Screen.width));
        for (int i = 0; i < list.arraySize; i++)
        {

            SerializedProperty Rule = list.GetArrayElementAtIndex(i);
            if (EditorGUILayout.PropertyField(Rule, true))
            {
                
            }
            if (GUILayout.Button("X",GUIStyle.none,GUILayout.Width(10)))
            {
                Rule.DeleteCommand();
            }


            
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void addElementToSerializedPropertyList<t>(t _itemToAdd)
    {

    }

    //Save data on closing the gui
    void OnDestory()
    {
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }


}