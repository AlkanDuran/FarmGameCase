using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private float _interactionTime = 0;
   
    public void Interact(float interactionTime = 0)
    {
        UIManager.Instance.TriggerYesNoPrompt("Want to Sleep?", GameManager.Instance.Sleep);
    }

    public string Text_OnAim() => "Press E to Interact";
    public float GetInteractionTime() => _interactionTime;
}
