#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Objectives))]
public class ObjectivesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
            return;

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Reset all objectives"))
            ResetAllObjectives();
    }

    private void ResetAllObjectives()
    {
        Objectives objectivesScriptable = (Objectives)target;

        for (int i = 0; i < objectivesScriptable.objectsOnSceneObjectives.Length; i++)
        {
            objectivesScriptable.objectsOnSceneObjectives[i].Reset();
        }

        for (int i = 0; i < objectivesScriptable.objectsOnMultipleScenesObjectives.Length; i++)
        {
            objectivesScriptable.objectsOnMultipleScenesObjectives[i].Reset();
        }

        for (int i = 0; i < objectivesScriptable.reachTriggerLocationObjectives.Length; i++)
        {
            objectivesScriptable.reachTriggerLocationObjectives[i].Reset();
        }

        for (int i = 0; i < objectivesScriptable.defeatEnemiesOnSceneObjectives.Length; i++)
        {
            objectivesScriptable.defeatEnemiesOnSceneObjectives[i].Reset();
        }

        for (int i = 0; i < objectivesScriptable.defeatPreplacedEnemyGroupObjectives.Length; i++)
        {
            objectivesScriptable.defeatPreplacedEnemyGroupObjectives[i].Reset();
        }
    }
}
#endif