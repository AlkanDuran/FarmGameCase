using UnityEngine;

[CreateAssetMenu(fileName = "Keybind Data", menuName = "Keybind Data/New Keybind Data", order = 1)]
public class KeybindData : ScriptableObject
{
    [Header("MOVEMENT KEY BINDS")]
    [SerializeField] private KeyCode _moveForward = KeyCode.W;
    [SerializeField] private KeyCode _moveBackward = KeyCode.S;
    [SerializeField] private KeyCode _moveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;
    [SerializeField] private KeyCode _jump = KeyCode.Space;
    [SerializeField] private KeyCode _sprint = KeyCode.LeftShift;

    [Header("ACTION KEY BINDS")]
    [SerializeField] private KeyCode _interaction = KeyCode.E;
    [SerializeField] private KeyCode _harvest = KeyCode.F;
    [SerializeField] private KeyCode _inventory = KeyCode.I;
    [SerializeField] private KeyCode _settings = KeyCode.Escape;

    public KeyCode GetMoveForwardKey() => _moveForward;
    public KeyCode GetMoveBackwardKey() => _moveBackward;
    public KeyCode GetMoveLeftKey() => _moveLeft;
    public KeyCode GetMoveRightKey() => _moveRight;
    public KeyCode GetJumpKey() => _jump;
    public KeyCode GetSprintKey() => _sprint;
    public KeyCode GetInteractionKey() => _interaction;
    public KeyCode GetHarvestKey() => _harvest;
    public KeyCode GetInventoryKey() => _inventory;
    public KeyCode GetSettingsPanelKey() => _settings;

    public void SetMoveForwardKey(KeyCode newKey) => _moveForward = newKey;
    public void SetMoveBackwardKey(KeyCode newKey) => _moveBackward = newKey;
    public void SetMoveLeftKey(KeyCode newKey) => _moveLeft = newKey;
    public void SetMoveRightKey(KeyCode newKey) => _moveRight = newKey;
    public void SetJumpKey(KeyCode newKey) => _jump = newKey;
    public void SetSprintKey(KeyCode newKey) => _sprint = newKey;
    public void SetInteractionKey(KeyCode newKey) => _interaction = newKey;
    public void SetHarvestKey(KeyCode newKey) => _harvest = newKey;
    public void SetInventoryKey(KeyCode newKey) => _inventory = newKey;
     public void SetSettingsPanelKey(KeyCode newKey) => _settings = newKey;
}
