using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] private Objectives objectivesScriptable;
    [SerializeField] private ObjectivesEnemyGroup objectivesEnemyGroup;

    private static ObjectivesManager instance;

    public static ObjectivesManager Instance => instance;

    public static event Action<IObjective> OnUnlockObjective;
    public static event Action<IObjective> OnObjectiveProgress;
    public static event Action<IObjective> OnObjectiveCompletion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            return;
        }

        Debug.LogError("Duplicate objectives manager.", instance);
        Debug.LogError("Duplicate objectives manager.", this);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeadByPlayer += OnEnemyKilledByPlayer;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeadByPlayer -= OnEnemyKilledByPlayer;
    }

    private void Update()
    {
        HandleDebugTriggers();
    }

    private void OnEnemyKilledByPlayer()
    {
        CheckEnemiesKilledOnSceneObjectiveCompletion();
    }

    private void HandleDebugTriggers()
    {
        IObjective debugObjectiveToTrigger = objectivesScriptable.defeatPreplacedEnemyGroupObjectives[0];

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (debugObjectiveToTrigger.IsUnlocked)
                return;

            objectivesEnemyGroup.gameObject.SetActive(true);

            // unlock preplaced enemies killed objective
            debugObjectiveToTrigger.UnlockObjective();
            OnUnlockObjective?.Invoke(debugObjectiveToTrigger);
            Debug.Log("Unlock preplaced enemies objective");
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (objectivesScriptable.reachTriggerLocationObjectives[0].IsUnlocked)
                return;

            // unlock reach trigger objective
            objectivesScriptable.reachTriggerLocationObjectives[0].UnlockObjective();
            OnUnlockObjective?.Invoke(objectivesScriptable.reachTriggerLocationObjectives[0]);
            Debug.Log("Unlock reach Trigger 111 objective");
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            if (objectivesScriptable.reachTriggerLocationObjectives[1].IsUnlocked)
                return;

            // unlock reach trigger objective
            objectivesScriptable.reachTriggerLocationObjectives[1].UnlockObjective();
            OnUnlockObjective?.Invoke(objectivesScriptable.reachTriggerLocationObjectives[1]);
            Debug.Log("Unlock reach Trigger 222 objective");
        }

        //if (Keyboard.current.xKey.wasPressedThisFrame)
        //{
        //    if (debugObjectiveToTrigger.IsUnlocked)
        //        return;

        //    // unlock enemies killed objective
        //    debugObjectiveToTrigger.UnlockObjective();
        //    OnUnlockObjective?.Invoke(debugObjectiveToTrigger);
        //    Debug.Log("Unlock multiple scenes objective");
        //}

        //if (Keyboard.current.xKey.wasPressedThisFrame)
        //{
        //    if (objectivesScriptable.objectsOnMultipleSceneObjectives[0].IsUnlocked)
        //        return;

        //    // unlock only "multiple scenes" objective
        //    objectivesScriptable.objectsOnMultipleSceneObjectives[0].UnlockObjective();
        //    OnUnlockObjective?.Invoke(objectivesScriptable.objectsOnMultipleSceneObjectives[0]);
        //    Debug.Log("Unlock multiple scenes objective");
        //}

        //if (Keyboard.current.cKey.wasPressedThisFrame)
        //{
        //    if (objectivesScriptable.objectsOnSceneObjectives[0].IsUnlocked)
        //        return;

        //    // unlock first "single scenes" objective
        //    objectivesScriptable.objectsOnSceneObjectives[0].UnlockObjective();
        //    OnUnlockObjective?.Invoke(objectivesScriptable.objectsOnSceneObjectives[0]);
        //    Debug.Log("Unlock first single scene objective");
        //}
    }

    private void CheckEnemiesKilledOnSceneObjectiveCompletion()
    {
        objectivesScriptable.CheckEnemiesKilledOnSceneObjectiveCompletion();
    }

    private void CheckPreplacedGroupEnemiesKilledObjectiveCompletion(int preplacedEnemyGroupObjectiveId)
    {
        objectivesScriptable.CheckPreplacedGroupEnemiesKilledObjectiveCompletion(preplacedEnemyGroupObjectiveId);
    }

    public void OnPreplacedGroupEnemyKilled(int preplacedEnemyGroupObjectiveId)
    {
        CheckPreplacedGroupEnemiesKilledObjectiveCompletion(preplacedEnemyGroupObjectiveId);
    }

    public void CheckCollectableObjectiveCompletion(CollectableType collectableType, int amount)
    {
        objectivesScriptable.CheckCollectableObjectiveCompletion(collectableType, amount);
    }

    public void OnObjectiveProgressChanged(IObjective progressableObjective)
    {
        OnObjectiveProgress?.Invoke(progressableObjective);
    }

    public void OnObjectiveCompleted(IObjective completedObjective)
    {
        OnObjectiveCompletion?.Invoke(completedObjective);
    }
}