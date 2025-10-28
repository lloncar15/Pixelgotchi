public interface IDamageable
{
    public DamageableType DamageableType { get; }
    public bool IsDamageableAlive { get; }
    public bool IsDashDamageable { get; }
    public void TakeDamage(float damage, bool damagedByPlayer, bool isCriticalHit = false);
}