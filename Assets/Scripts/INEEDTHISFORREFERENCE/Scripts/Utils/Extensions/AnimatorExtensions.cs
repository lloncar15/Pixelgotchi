using UnityEngine;

public static class AnimatorExtensions 
{
    /// <summary>
    /// Minimum float value to use for parameters that act as multipliers, to prevent Animator freezing or infinite state duration.
    /// </summary>
    const float minFloatValue = 0.001f;
    
    /// <summary>
    /// Sets a float parameter on the Animator by <paramref name="name"/>, ensuring it is never zero.
    /// This is useful for parameters used as multipliers, as zero can cause Animator issues.
    /// </summary>
    /// <param name="animator">The Animator component whose parameter will be set.</param>
    /// <param name="name">Parameter name to be changed.</param>
    /// <param name="value">The value to set; will be replaced with a small positive value if too close to zero.</param>
    public static void SetNonZeroFloat(this Animator animator, string name, float value)
    {
        if (animator is null) return;
        
        value = Mathf.Max(value, minFloatValue);
        
        animator.SetFloat(name, value);
    }

    /// <summary>
    /// Sets a float parameter on the Animator by <paramref name="hash"/>, ensuring it is never zero.
    /// This is useful for parameters used as multipliers, as zero can cause Animator issues.
    /// </summary>
    /// <param name="animator">The Animator component whose parameter will be set.</param>
    /// <param name="hash">The precomputed hash of the float parameter name (use Animator.StringToHash).</param>
    /// <param name="value">The value to set; will be replaced with a small positive value if too close to zero.</param>
    public static void SetNonZeroFloat(this Animator animator, int hash, float value)
    {
        if (animator is null) return;
        
        value = Mathf.Max(value, minFloatValue);
        
        animator.SetFloat(hash, value);
    }

    /// <summary>
    /// Sets a boolean or trigger parameter on the Animator based on the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="animator">The Animator component whose parameter will be set.</param>
    /// <param name="hash">The precomputed hash of the parameter name (use Animator.StringToHash).</param>
    /// <param name="value">The value to set.</param>
    public static void SetBoolOrTriggerParamater(this Animator animator, int hash, bool value)
    {
        foreach (AnimatorControllerParameter param in animator.parameters) {
            if (param.nameHash != hash) continue;

            switch (param.type) {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(hash, value);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (value)
                        animator.SetTrigger(hash);
                    else
                        animator.ResetTrigger(hash);
                    break;
                case AnimatorControllerParameterType.Float:
                case AnimatorControllerParameterType.Int:
                default:
                    break;
                {

                }
            }
        }
    }
}