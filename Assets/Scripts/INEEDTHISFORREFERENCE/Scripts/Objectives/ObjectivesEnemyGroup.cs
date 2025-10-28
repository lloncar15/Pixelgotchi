using UnityEngine;
using System.Collections.Generic;

public class ObjectivesEnemyGroup : MonoBehaviour
{
    [SerializeField] private int PreplacedEnemyGroupObjectiveId = -1;
    private HashSet<EnemyHealth> enemiesSet;

    private void Awake()
    {
        enemiesSet = new (transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            enemiesSet.Add(transform.GetChild(i).GetComponent<EnemyHealth>());
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDead += OnEnemyDead;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDead += OnEnemyDead;
    }

    private void OnEnemyDead(EnemyHealth deadEnemy)
    {
        if (!enemiesSet.Contains(deadEnemy))
            return;

        enemiesSet.Remove(deadEnemy);
        ObjectivesManager.Instance.OnPreplacedGroupEnemyKilled(PreplacedEnemyGroupObjectiveId);
    }
}