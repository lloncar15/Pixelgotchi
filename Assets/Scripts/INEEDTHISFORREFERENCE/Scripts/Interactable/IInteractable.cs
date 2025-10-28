public interface IInteractable 
{
    void Interact();
    bool CanInteract();
    void EnableInteraction();
    void DisableInteraction();
    void DeactivateInteractable();
    void ActivateInteractable();
}