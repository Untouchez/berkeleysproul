using UnityEngine;
using UnityEditor;

public class EditModeFunctions : EditorWindow
{
    public NPC npc;
    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<EditModeFunctions>("Edit Mode Functions");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Run Function"))
        {
            FunctionToRun();
        }
    }

    private void FunctionToRun()
    {
        Debug.Log("run");
    }
}