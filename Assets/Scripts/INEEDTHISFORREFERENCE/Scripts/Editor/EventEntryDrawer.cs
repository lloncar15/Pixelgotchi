using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColliderReactorEvents.EventEntry))]
public class EventEntryDrawer : PropertyDrawer
{
    private const float PADDING = 6;
    private const float LINE_HEIGHT = 2;

    // Keep track of foldout state per property
    private static readonly Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    private bool GetFoldout(SerializedProperty property)
    {
        if (!foldouts.TryGetValue(property.propertyPath, out bool state))
        {
            state = true;
            foldouts[property.propertyPath] = state;
        }
        return state;
    }

    private void SetFoldout(SerializedProperty property, bool value)
    {
        foldouts[property.propertyPath] = value;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool foldout = GetFoldout(property);

        if (!foldout)
            return EditorGUIUtility.singleLineHeight + PADDING;

        SerializedProperty modeProp = property.FindPropertyRelative("Mode");
        SerializedProperty colliderProp = property.FindPropertyRelative("ColliderEvent");
        SerializedProperty noArgsProp = property.FindPropertyRelative("NoArgsEvent");

        float height = EditorGUIUtility.singleLineHeight;

        switch ((ColliderReactorEvents.EventEntry.InvokeMode)modeProp.enumValueIndex)
        {
            case ColliderReactorEvents.EventEntry.InvokeMode.NoArgs:
                height += EditorGUI.GetPropertyHeight(noArgsProp, new GUIContent("No Args Event"));
                break;
            case ColliderReactorEvents.EventEntry.InvokeMode.WithCollider:
                height += EditorGUI.GetPropertyHeight(colliderProp, new GUIContent("Collider Event"));
                break;
            case ColliderReactorEvents.EventEntry.InvokeMode.Both:
                height += EditorGUI.GetPropertyHeight(colliderProp, new GUIContent("Collider Event"));
                height += EditorGUI.GetPropertyHeight(noArgsProp, new GUIContent("No Args Event")) + LINE_HEIGHT;
                break;
        }

        return height + PADDING;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty modeProp = property.FindPropertyRelative("Mode");
        SerializedProperty noArgsProp = property.FindPropertyRelative("NoArgsEvent");
        SerializedProperty colliderProp = property.FindPropertyRelative("ColliderEvent");

        bool foldout = GetFoldout(property);

        Rect line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        foldout = EditorGUI.Foldout(line, foldout, label, true);
        SetFoldout(property, foldout);

        line.y += EditorGUIUtility.singleLineHeight + LINE_HEIGHT;

        if (!foldout)
            return;
        
        EditorGUI.PropertyField(line, modeProp);
        line.y += EditorGUIUtility.singleLineHeight + LINE_HEIGHT;
        
        switch ((ColliderReactorEvents.EventEntry.InvokeMode)modeProp.enumValueIndex)
        {
            case ColliderReactorEvents.EventEntry.InvokeMode.NoArgs:
                EditorGUI.PropertyField(line, noArgsProp, new GUIContent("No Args Event"));
                break;
            case ColliderReactorEvents.EventEntry.InvokeMode.WithCollider:
                EditorGUI.PropertyField(line, colliderProp, new GUIContent("Collider Event"));
                break;
            case ColliderReactorEvents.EventEntry.InvokeMode.Both:
                EditorGUI.PropertyField(line, colliderProp, new GUIContent("Collider Event"));
                line.y += EditorGUI.GetPropertyHeight(colliderProp, new GUIContent("Collider Event")) + LINE_HEIGHT;
                EditorGUI.PropertyField(line, noArgsProp, new GUIContent("No Args Event"));
                break;
        }
    }
}
