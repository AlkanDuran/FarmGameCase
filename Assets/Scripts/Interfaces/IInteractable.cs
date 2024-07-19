public interface IInteractable
{
    public void Interact(float interactionTime = 0);
    public string Text_OnAim();
    public float GetInteractionTime();
}
