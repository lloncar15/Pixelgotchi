using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactableOnce = true;
    [SerializeField] private bool autoInteractOnEnableInteractable = false;
    [SerializeField, Tooltip("Events to do after interaction.")] private UnityEvent OnInteracted;
    [SerializeField, Tooltip("Animators that are affected by interaction.")] private Animator[] AffectedAnimators;
    [SerializeField] private Animator TooltipAnimator;
    private InputAction interactionInputAction;
    private bool isInteractable = false;
    private bool hasBeenInteracted = false;

    private readonly int completeHash = Animator.StringToHash("Completed");
    private readonly int activeHash = Animator.StringToHash("Active");

    private void Awake()
    {
        interactionInputAction = InputManager.Instance.PlayerInputActions.Player1.Use;
    }

    private void OnEnable()
    {
        interactionInputAction.performed += OnInteractionActionPerformed;
    }

    private void OnDisable()
    {
        interactionInputAction.performed -= OnInteractionActionPerformed;
    }

    private void OnInteractionActionPerformed(InputAction.CallbackContext context)
    {
        if (!CanInteract())
            return;

        Interact();
    }

    public virtual void Interact()
    {
        SetParameterForAnimators(completeHash, true);

        hasBeenInteracted = true;
        OnInteracted?.Invoke();

        if (interactableOnce)
        {
            DeactivateInteractable();
        }
    }

    public virtual bool CanInteract()
    {
        if (interactableOnce && hasBeenInteracted)
            return false;

        return isInteractable;
    }

    public void EnableInteraction()
    {
        if (interactableOnce && hasBeenInteracted)
            return;

        isInteractable = true;

        if (autoInteractOnEnableInteractable && CanInteract())
            Interact();
    }

    public void DisableInteraction()
    {
        isInteractable = false;
    }

    public void ActivateInteractable()
    {
        SetParameterForAnimators(activeHash, true);
    }

    public void DeactivateInteractable()
    {
        SetParameterForAnimators(activeHash, false);
        DisableInteraction();
    }

    private void SetParameterForAnimators(int parameterHash, bool value)
    {
        foreach (Animator animator in AffectedAnimators)
        {
            animator?.SetBoolOrTriggerParamater(parameterHash, value);
        }

        TooltipAnimator?.SetBoolOrTriggerParamater(parameterHash, value);
    }
}