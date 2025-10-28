using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 8f;
    public float force = 900f;
    public float upwardsModifier = 0.5f; // adds lift
    public ForceMode forceMode = ForceMode.Impulse;

    [Header("Layers")]
    public LayerMask affectLayers = ~0;     // which layers get pushed
    public LayerMask occlusionLayers = ~0;  // which layers block the explosion
    public bool respectLineOfSight = true;

    [Header("Lifecycle")]
    public bool autoDisable = false;  // useful if pooling
    public float disableAfter = 2f;

    void OnEnable()
    {
        Explode(transform.position);

        if (autoDisable)
            Invoke(nameof(DisableSelf), disableAfter);
    }

    private void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(
            position,
            radius,
            affectLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (var hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (!rb || rb.isKinematic) continue;

            // Line of sight check (optional)
            if (respectLineOfSight)
            {
                Vector3 closest = hit.ClosestPoint(position);
                if (Physics.Linecast(position, closest, occlusionLayers, QueryTriggerInteraction.Ignore))
                    continue; // blocked
            }

            rb.AddExplosionForce(force, position, radius, upwardsModifier, forceMode);
            rb.WakeUp();
        }
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    // Draw radius in Scene view for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}