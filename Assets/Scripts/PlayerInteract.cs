using UnityEngine;
using TMPro;

public class PlayerInteract : LocalSingleton<PlayerInteract>
{
    [SerializeField] private LayerMask _interactableLayers;
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private InteractionBar _interactionBar;

    private IInteractable _lastInteractedObject;
    private ISelectable _lastSelectableObject = null;
    private bool _isInteracting = false;
    private bool _interactionCompleted = false;

    private void Update()
    {
        CheckInteractable();
        
        if (_lastInteractedObject != null && Input.GetKey(GameManager.Instance.GetPcKeyBindData().GetInteractionKey()))
        {
            if (!_isInteracting && !_interactionCompleted)
            {
                StartInteraction();
            }
        }
        else if (_isInteracting)
        {
            StopInteraction();
        }
        else if (!Input.GetKey(GameManager.Instance.GetPcKeyBindData().GetInteractionKey()))
        {
            _interactionCompleted = false;
        }
    }

    private void CheckInteractable()
    {
        var ray = Camera.main!.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

        if (Physics.Raycast(ray, out var hit, _interactionDistance, _interactableLayers))
        {
            var interactedObject = hit.collider.gameObject;
            if (interactedObject != null)
            {
                if (interactedObject.TryGetComponent(out IInteractable currentInteractable))
                {
                    if (currentInteractable != _lastInteractedObject)
                    {
                        _lastInteractedObject = currentInteractable;
                        UIManager.Instance.ShowInteractionText();
                        UIManager.Instance.GetInteractionText().text = currentInteractable.Text_OnAim();

                        if (interactedObject.TryGetComponent(out ISelectable selectable))
                        {
                            if (_lastSelectableObject != null && _lastSelectableObject != selectable)
                            {
                                _lastSelectableObject.Select(false);
                            }

                            selectable.Select(true);
                            _lastSelectableObject = selectable;
                        }
                    }
                }
            }
        }
        else
        {
            if (_lastInteractedObject != null)
            {
                _lastInteractedObject = null;
                UIManager.Instance.HideInteractionText();
            }

            if (_lastSelectableObject != null)
            {
                _lastSelectableObject.Select(false);
                _lastSelectableObject = null;
            }
        }
    }

    private void StartInteraction()
    {
        _isInteracting = true;
        float interactionTime = _lastInteractedObject.GetInteractionTime();

        if (_interactionBar != null)
        {
            if (interactionTime > 0)
            {
                _interactionBar.StartFilling(interactionTime);
                WaterParticle(true);
            }
            else
            {
                CompleteInteraction();
            }
        }
    }

    private void StopInteraction()
    {
        _isInteracting = false;
        if (_interactionBar != null)
        {
            _interactionBar.StopFilling();
            WaterParticle(false);
        }
    }

    public void CompleteInteraction()
    {
        _lastInteractedObject?.Interact(_lastInteractedObject.GetInteractionTime());
        _interactionCompleted = true;
        StopInteraction();
    }

    private void WaterParticle(bool value)
    {
        var toolSlot = InventoryManager.Instance.GetEquippedSlotItem();
        var equipmentTool = toolSlot as EquipmentData;
        if (equipmentTool != null)
        {
            if (equipmentTool.toolType is EquipmentData.ToolType.WateringCan)
            {
                var waterParticle = InventoryManager.Instance.GetEquippedItem().GetComponentInChildren<ParticleSystem>();
                if(value) waterParticle.Play(true);
                else waterParticle.Stop();
            }
        }
    }
}
