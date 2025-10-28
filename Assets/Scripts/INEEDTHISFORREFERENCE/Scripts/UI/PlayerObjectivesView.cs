using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerObjectivesView : MonoBehaviour
{
    [SerializeField] private Objectives objectivesScriptable;
    [SerializeField] private TMP_Text[] objectiveTexts;
    private IObjective[] objectives;

    private void Awake()
    {
        objectives = new IObjective[objectiveTexts.Length];
        // namjesti Tracked objectives-e
        
        SetInitialTrackedObjectivesOnSceneLoad();
    }

    private void SetInitialTrackedObjectivesOnSceneLoad()
    {
        // proc kroz sve objectivese

        // vidit koji su 'tracked', i sukladno 'tracked indexu'
        // ih tu postavit

        // trebalo bi napravit nesto "da se provjeri jel relevantno za scenu"
        for (int i = 0; i < objectivesScriptable.objectsOnSceneObjectives.Length; i++)
        {
            IObjective current = objectivesScriptable.objectsOnSceneObjectives[i];

            if (((XObjectsOnSceneObjective)current).SceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
                continue;

            if (!current.IsCompleted && current.IsTracked)
                SetTrackedObjectiveUI(current);
        }

        for (int i = 0; i < objectivesScriptable.objectsOnMultipleScenesObjectives.Length; i++)
        {
            IObjective current = objectivesScriptable.objectsOnMultipleScenesObjectives[i];

            if (!current.IsCompleted && current.IsTracked)
                SetTrackedObjectiveUI(current);
        }
    }

    private void SetTrackedObjectiveUI(IObjective trackedObjective)
    {
        objectives[trackedObjective.TrackingIndex] = trackedObjective;
        objectiveTexts[trackedObjective.TrackingIndex].gameObject.SetActive(true);
        objectiveTexts[trackedObjective.TrackingIndex].text = GetObjectiveProgressText(trackedObjective);
    }

    private void OnEnable()
    {
        ObjectivesManager.OnUnlockObjective += OnUnlockObjective;
        ObjectivesManager.OnObjectiveProgress += OnObjectiveProgress;
        ObjectivesManager.OnObjectiveCompletion += OnObjectiveCompletion;
    }

    private void OnDisable()
    {
        ObjectivesManager.OnUnlockObjective -= OnUnlockObjective;
        ObjectivesManager.OnObjectiveProgress -= OnObjectiveProgress;
        ObjectivesManager.OnObjectiveCompletion -= OnObjectiveCompletion;
    }

    private void OnObjectiveCompletion(IObjective completedObjective)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (objectives[i] == null)
                continue;

            if (!objectives[i].Equals(completedObjective))
                continue;

            objectiveTexts[i].text = GetObjectiveCompletedText(completedObjective);
        }
    }

    private void OnObjectiveProgress(IObjective progressedObjective)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (objectives[i] == null)
                continue;

            if (!objectives[i].Equals(progressedObjective))
                continue;

            objectiveTexts[i].text = GetObjectiveProgressText(progressedObjective);
        }
    }

    private void OnUnlockObjective(IObjective newlyUnlockedObjective)
    {
        int index = GetFirstFreeTextIndex();
        TMP_Text firstFreeText = objectiveTexts[index];
        firstFreeText.text = GetObjectiveProgressText(newlyUnlockedObjective);
        firstFreeText.gameObject.SetActive(true);
        objectives[index] = newlyUnlockedObjective;
        newlyUnlockedObjective.IsTracked = true;
        newlyUnlockedObjective.TrackingIndex = index;
    }

    private string GetObjectiveProgressText(IObjective objective)
    {
        if (objective is ReachTriggerObjective)
            return objective.GetName();

        return objective.GetName() + ", " + objective.Progress() + " / " + objective.ProgressGoal();
    }

    private string GetObjectiveCompletedText(IObjective objective)
    {
        return objective.GetName() + " COMPLETED!";
    }

    private int GetFirstFreeTextIndex()
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (objectives[i] == null)
                return i;
        }

        return -1;
    }
}