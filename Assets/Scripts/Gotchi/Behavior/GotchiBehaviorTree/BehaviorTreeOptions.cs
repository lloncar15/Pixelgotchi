using UnityEngine;

namespace GimGim.Gotchi {
    [CreateAssetMenu(fileName = "BehaviorTreeOptions", menuName = "GimGim/Gotchi/BehaviorTreeOptions")]
    public class BehaviorTreeOptions : ScriptableObject {
        [Header("General Attack parameters")] 
        [SerializeField] public float shootingAttackChance = 0.5f;
        [SerializeField] public float chargeAttackChance = 0.5f;
        [SerializeField] public float attackCooldown = 5f;
        
        [Header("Shooting attack parameters")]
        [SerializeField] public int shotsAmount = 1;
    }
}