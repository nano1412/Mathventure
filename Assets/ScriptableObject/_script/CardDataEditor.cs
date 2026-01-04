using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CardInDeckData))]
public class CardDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FaceValue"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EffectValue"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Effect"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
