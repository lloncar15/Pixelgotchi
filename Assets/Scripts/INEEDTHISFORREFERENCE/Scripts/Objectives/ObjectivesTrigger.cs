using UnityEngine;

public class ObjectivesTrigger : MonoBehaviour
{
    [SerializeField] private Objectives objectivesScriptable;
    [SerializeField] private int objectiveTriggerId;

    public void OnReactorTriggerEnter()
    {
        objectivesScriptable.TryCompleteTriggerObjective(objectiveTriggerId);
    }
}