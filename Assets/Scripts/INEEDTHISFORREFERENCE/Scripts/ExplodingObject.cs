using UnityEngine;

/// <summary>
/// Component attached to an object that has an explosion animation. Should be added to an object with a trigger collider
/// so that it can be damaged and explode.
/// </summary>
public class ExplodingObject : MonoBehaviour, IDamageable, IExploding {

    [Header("Damageable Settings")] 
    [SerializeField, Tooltip("Hit points of the object.")] private float maxHitPoints = 0f;
    [SerializeField] private float currentHitPoints;
    [SerializeField] private bool isDamagedByDash = false;
    
    private bool isDestroyed = false;
    public DamageableType DamageableType => DamageableType.Explodable;
    public bool IsDamageableAlive => !isDestroyed;
    public bool IsDashDamageable => isDamagedByDash;

    [Header("Exploding Settings")]
    [SerializeField, Tooltip("Animator to trigger.")] private Animator animator;
    [SerializeField, Tooltip("Trigger name in the animator.")] private string animatorTriggerName = "Active";
    
    public bool HasExploded { get; private set; } = false;

    public Animator Animator => animator;

    public void Start() 
    {
        currentHitPoints = maxHitPoints;
    }

    public void TakeDamage(float damage, bool damagedByPlayer, bool isCriticalHit = false) 
    {
        if (isDestroyed) return;
        
        currentHitPoints -= damage;
        OnDamageTaken();
    }

    private void OnDamageTaken() 
    {
        if (currentHitPoints > 0f || HasExploded) return;
        
        isDestroyed = true;
        Explode();
    }

    public void Explode() 
    {
        if (animator is null) return;
        
        HasExploded = true;
        animator.SetTrigger(animatorTriggerName);
    }
}
