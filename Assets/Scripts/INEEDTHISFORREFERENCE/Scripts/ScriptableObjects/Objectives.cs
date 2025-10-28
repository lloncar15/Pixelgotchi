using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ObjectiveType
{
    Undefined, CollectXObjectsOnScene, CollectXObjectsOnMultipleScenes, ReachLocation, DefeatEnemies
}

public enum CollectableType
{
    Undefined, Coins, Scraps, Ammo, Health, Energy, Brain, Vials
}

public enum ObjectiveRewardType
{
    Undefined, Experience, Coins, Scraps, OpenSceneSegment
}

// idk yet, something to encompass all objective types, for now
public interface IObjective
{
    bool IsUnlocked { get; }
    int TrackingIndex { get; set; }
    bool IsTracked { get; set; }
    bool IsCompleted { get; }

    public string GetName();
    public int Progress();
    public int ProgressGoal();
    public void UnlockObjective();
}

[Serializable]
public class ObjectiveClass : IObjective
{
    public ObjectiveType Type;
    public float Number;
    public GameObject Reference;

    public int TrackingIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsTracked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsCompleted => throw new NotImplementedException();

    public bool IsUnlocked => throw new NotImplementedException();

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public int Progress()
    {
        throw new NotImplementedException();
    }

    public int ProgressGoal()
    {
        throw new NotImplementedException();
    }

    public void UnlockObjective()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class ObjectiveReward
{
    // common data-type za objective reward
    public ObjectiveRewardType RewardType;
    public int Quantity;
}

[Serializable]
public class XObjectsOnSceneObjective : IObjective
{
    // na TOJ sceni..
    // imamo TE objekte
    // A) predefinirani / pre-placed objekti
    // B) runtime-generirani objekti

    // npr. pokupi 10 Scrapsa na sceni.. otvore se Vrata A (to otvara sljedeci segment)
    // npr. pokupi 30 Scrapsa na sceni (dodatnih 20 nakon prvih 10).. otvore se Vrata B

    public string Name;
    public bool IsUnlocked;
    public bool IsTracked;
    public int TrackingIndex = -1;
    public bool IsCompleted;    
    public int SceneBuildIndex;
    public CollectableType CollectableType;
    public int CollectedSinceUnlock;
    public int QuantityForCompletion;
    public ObjectiveReward Reward;

    bool IObjective.IsUnlocked => IsUnlocked;
    int IObjective.TrackingIndex { get => TrackingIndex; set => TrackingIndex = value; }
    bool IObjective.IsTracked { get => IsTracked; set => IsTracked = value; }
    bool IObjective.IsCompleted => IsCompleted;

    public string GetName() => Name;
    public int Progress() => CollectedSinceUnlock;
    public int ProgressGoal() => QuantityForCompletion;

    public void UnlockObjective()
    {
        IsUnlocked = true;
        IsTracked = true;
        CollectedSinceUnlock = 0;
    }

    public bool TryCompleteObjective(int addedAmount)
    {
        CollectedSinceUnlock += addedAmount;
        ObjectivesManager.Instance.OnObjectiveProgressChanged(this);

        if (CollectedSinceUnlock >= QuantityForCompletion)
        {
            IsCompleted = true;
            ObjectivesManager.Instance.OnObjectiveCompleted(this);

            return true;
        }

        return false;
    }

    public void Reset()
    {
        IsUnlocked = false;
        IsTracked = false;
        TrackingIndex = -1;
        IsCompleted = false;
        CollectedSinceUnlock = 0;
    }
}

[Serializable]
public class XObjectsOnMultipleScenesObjective : IObjective
{
    // mislim da "preko vise scena" znaci da moze sve biti i na jednoj (valjda..)

    public string Name;
    public CollectableType CollectableType;
    public bool IsUnlocked;
    public bool IsTracked;
    public int TrackingIndex = -1;
    public bool IsCompleted;
    public int CollectedSinceUnlock;
    public int QuantityForCompletion;
    public ObjectiveReward Reward;

    bool IObjective.IsUnlocked => IsUnlocked;
    int IObjective.TrackingIndex { get => TrackingIndex; set => TrackingIndex = value; }
    bool IObjective.IsTracked { get => IsTracked; set => IsTracked = value; }
    bool IObjective.IsCompleted => IsCompleted;

    public string GetName() => Name;
    public int Progress() => CollectedSinceUnlock;
    public int ProgressGoal() => QuantityForCompletion;

    public void UnlockObjective()
    {
        IsUnlocked = true;
        IsTracked = true;
        CollectedSinceUnlock = 0;
    }

    public bool TryCompleteObjective(int addedAmount)
    {
        CollectedSinceUnlock += addedAmount;
        ObjectivesManager.Instance.OnObjectiveProgressChanged(this);

        if (CollectedSinceUnlock >= QuantityForCompletion)
        {
            IsCompleted = true;
            ObjectivesManager.Instance.OnObjectiveCompleted(this);

            return true;
        }

        return false;
    }

    public void Reset()
    {
        IsUnlocked = false;
        IsTracked = false;
        TrackingIndex = -1;
        IsCompleted = false;
        CollectedSinceUnlock = 0;
    }
}

[Serializable]
public class ReachTriggerObjective : IObjective
{
    // nekako povezati ove funkcionalnosti sa ColliderReactorEvents.cs ili InteractableObject.cs ?

    public string Name;
    public bool IsUnlocked;
    public bool IsTracked;
    public int TrackingIndex = -1;
    public bool IsCompleted;
    public int TODOTriggerLocationId = -1;
    public ObjectiveReward Reward;

    bool IObjective.IsUnlocked => IsUnlocked;
    int IObjective.TrackingIndex { get => TrackingIndex; set => TrackingIndex = value; }
    bool IObjective.IsTracked { get => IsTracked; set => IsTracked = value; }
    bool IObjective.IsCompleted => IsCompleted;

    public string GetName() => Name;
    public int Progress() => -1;
    public int ProgressGoal() => -1;

    public void UnlockObjective()
    {
        IsUnlocked = true;
        //IsTracked = true;
    }

    public bool TryCompleteTriggerObjective()
    {
        if (!IsUnlocked || IsCompleted)
            return false;

        Debug.Log("Complete trigger objective with id " + TODOTriggerLocationId);
        ObjectivesManager.Instance.OnObjectiveCompleted(this);
        IsCompleted = true;

        return true;
    }

    public void Reset()
    {
        IsUnlocked = false;
        IsTracked = false;
        TrackingIndex = -1;
        IsCompleted = false;
    }
}

[Serializable]
public class DefeatEnemiesOnSceneObjective : IObjective
{
    public string Name;
    public bool IsUnlocked;
    public bool IsTracked;
    public int TrackingIndex = -1;
    public bool IsCompleted;
    public int SceneBuildIndex = -1;
    public int CollectedSinceUnlock;
    public int QuantityForCompletion;
    public ObjectiveReward Reward;

    bool IObjective.IsUnlocked => IsUnlocked;
    int IObjective.TrackingIndex { get => TrackingIndex; set => TrackingIndex = value; }
    bool IObjective.IsTracked { get => IsTracked; set => IsTracked = value; }
    bool IObjective.IsCompleted => IsCompleted;

    public string GetName() => Name;
    public int Progress() => CollectedSinceUnlock;
    public int ProgressGoal() => QuantityForCompletion;

    public void UnlockObjective()
    {
        IsUnlocked = true;
        IsTracked = true;
        CollectedSinceUnlock = 0;
    }

    public bool TryCompleteObjective()
    {
        CollectedSinceUnlock += 1;
        ObjectivesManager.Instance.OnObjectiveProgressChanged(this);

        if (CollectedSinceUnlock >= QuantityForCompletion)
        {
            IsCompleted = true;
            ObjectivesManager.Instance.OnObjectiveCompleted(this);

            return true;
        }

        return false;
    }

    public void Reset()
    {
        IsUnlocked = false;
        IsTracked = false;
        TrackingIndex = -1;
        IsCompleted = false;
        CollectedSinceUnlock = 0;
    }
}

[Serializable]
public class DefeatEnemiesPreplacedGroupObjective : IObjective
{
    public string Name;
    public bool IsUnlocked;
    public bool IsTracked;
    public int TrackingIndex = -1;
    public bool IsCompleted;
    public int CollectedSinceUnlock;
    public int QuantityForCompletion;
    public int EnemyGroupObjectiveId = -1;
    public ObjectiveReward Reward;

    bool IObjective.IsUnlocked => IsUnlocked;
    int IObjective.TrackingIndex { get => TrackingIndex; set => TrackingIndex = value; }
    bool IObjective.IsTracked { get => IsTracked; set => IsTracked = value; }
    bool IObjective.IsCompleted => IsCompleted;

    public string GetName() => Name;
    public int Progress() => CollectedSinceUnlock;
    public int ProgressGoal() => QuantityForCompletion;

    public void UnlockObjective()
    {
        IsUnlocked = true;
        IsTracked = true;
        CollectedSinceUnlock = 0;
    }

    public bool TryCompleteObjective()
    {
        CollectedSinceUnlock += 1;
        ObjectivesManager.Instance.OnObjectiveProgressChanged(this);

        if (CollectedSinceUnlock >= QuantityForCompletion)
        {
            IsCompleted = true;
            ObjectivesManager.Instance.OnObjectiveCompleted(this);

            return true;
        }

        return false;
    }

    public void Reset()
    {
        IsUnlocked = false;
        IsTracked = false;
        TrackingIndex = -1;
        IsCompleted = false;
        CollectedSinceUnlock = 0;
    }
}

[CreateAssetMenu(fileName = "Objectives", menuName = "Objectives")]
public class Objectives : ScriptableObject
{
    public ObjectiveClass[] objectives;

    public XObjectsOnSceneObjective[] objectsOnSceneObjectives;
    public XObjectsOnMultipleScenesObjective[] objectsOnMultipleScenesObjectives;
    public ReachTriggerObjective[] reachTriggerLocationObjectives;
    public DefeatEnemiesOnSceneObjective[] defeatEnemiesOnSceneObjectives;
    public DefeatEnemiesPreplacedGroupObjective[] defeatPreplacedEnemyGroupObjectives;

    public void CheckCollectableObjectiveCompletion(CollectableType collectableType, int amount)
    {
        // kad je neki Collectable collectan...

        // potencijalno su kompletirani
            // A) "objects on scene" objective
            // B) "objects on multiple scenes" objective

        // pa chekiraj ta 2

        // zasad gleda "od pocetka scene" kolki je amount u Resource Manageru, tweak ubuduce
        CheckObjectsOnSceneObjectivesForCompletion(collectableType, amount);
        CheckObjectsOnMultipleScenesObjectivesForCompletion(collectableType, amount);
    }

    private void CheckObjectsOnSceneObjectivesForCompletion(CollectableType collectableType, int addedAmount)
    {
        for (int i = 0; i < objectsOnSceneObjectives.Length; i++)
        {
            XObjectsOnSceneObjective currentSameSceneObjective = objectsOnSceneObjectives[i];

            if (!currentSameSceneObjective.IsUnlocked)
                continue;

            if (currentSameSceneObjective.IsCompleted)
                continue;

            if (!currentSameSceneObjective.CollectableType.Equals(collectableType))
                continue;

            if (currentSameSceneObjective.SceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
                continue;

            // new
            if (currentSameSceneObjective.TryCompleteObjective(addedAmount))
                GiveReward(currentSameSceneObjective.Reward);

            // old
            //if (ResourcesManager.Instance.CollectedOnSceneDict[collectableType] >= objectsOnSceneObjectives[i].QuantityForCompletion)
            //{
            //    // onda completaj Objective
            //    objectsOnSceneObjectives[i].IsCompleted = true;

            //    GiveReward(objectsOnSceneObjectives[i].Reward);
            //}
        }
    }

    private void CheckObjectsOnMultipleScenesObjectivesForCompletion(CollectableType collectableType, int addedAmount)
    {
        // pogledat jel imamo taj tip Objective-a s tim Collectable tipom
        // pogledat jel "unlocked"
        // ako da, dodat mu amount i chekirat jel predjena granica za "completion"

        for (int i = 0; i < objectsOnMultipleScenesObjectives.Length; i++)
        {
            if (objectsOnMultipleScenesObjectives[i].IsCompleted)
                continue;

            if (!objectsOnMultipleScenesObjectives[i].IsUnlocked)
                continue;

            if (!objectsOnMultipleScenesObjectives[i].CollectableType.Equals(collectableType))
                continue;

            //objectsOnMultipleSceneObjectives[i].AddCollectableAmount(addedAmount);

            if (objectsOnMultipleScenesObjectives[i].TryCompleteObjective(addedAmount))
                GiveReward(objectsOnMultipleScenesObjectives[i].Reward);
        }
    }

    private void GiveReward(ObjectiveReward reward)
    {
        switch (reward.RewardType)
        {
            case ObjectiveRewardType.Undefined:
                break;

            case ObjectiveRewardType.Experience:
                Debug.Log("Give " + reward.Quantity + " experience to the Player");
                break;

            case ObjectiveRewardType.Coins:
                Debug.Log("Give " + reward.Quantity + " coins to the Player");
                break;

            case ObjectiveRewardType.Scraps:
                Debug.Log("Give " + reward.Quantity + " scraps to the Player");
                break;

            case ObjectiveRewardType.OpenSceneSegment:
                Debug.Log("Open new scene segment");
                break;

            default:
                // Optional fallback for unexpected values
                break;
        }
    }

    public void TryCompleteTriggerObjective(int TODOTriggerId)
    {
        Debug.Log("Objectives: try complete trigger objective (" + TODOTriggerId + ")");

        for (int i = 0; i < reachTriggerLocationObjectives.Length; i++)
        {
            if (reachTriggerLocationObjectives[i].TODOTriggerLocationId != TODOTriggerId)
                continue;

            if (reachTriggerLocationObjectives[i].TryCompleteTriggerObjective())
                GiveReward(reachTriggerLocationObjectives[i].Reward);

            break;
        }
    }

    public void CheckEnemiesKilledOnSceneObjectiveCompletion()
    {
        for (int i = 0; i < defeatEnemiesOnSceneObjectives.Length; i++)
        {
            DefeatEnemiesOnSceneObjective currentEnemiesObjective = defeatEnemiesOnSceneObjectives[i];

            if (!currentEnemiesObjective.IsUnlocked)
                continue;

            if (currentEnemiesObjective.IsCompleted)
                continue;

            if (currentEnemiesObjective.SceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
                continue;

            if (currentEnemiesObjective.TryCompleteObjective())
                GiveReward(currentEnemiesObjective.Reward);
        }
    }

    public void CheckPreplacedGroupEnemiesKilledObjectiveCompletion(int preplacedEnemyGroupObjectiveId)
    {
        for (int i = 0; i < defeatPreplacedEnemyGroupObjectives.Length; i++)
        {
            DefeatEnemiesPreplacedGroupObjective currentPreplacedEnemyGroupObjective = defeatPreplacedEnemyGroupObjectives[i];

            if (currentPreplacedEnemyGroupObjective.EnemyGroupObjectiveId != preplacedEnemyGroupObjectiveId)
                continue;

            if (!currentPreplacedEnemyGroupObjective.IsUnlocked)
                continue;

            if (currentPreplacedEnemyGroupObjective.IsCompleted)
                continue;

            if (currentPreplacedEnemyGroupObjective.TryCompleteObjective())
                GiveReward(currentPreplacedEnemyGroupObjective.Reward);
        }
    }
}