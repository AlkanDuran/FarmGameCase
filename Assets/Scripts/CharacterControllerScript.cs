using System;
using DG.Tweening;
using UnityEngine;

public enum CharacterMoveState
{
    Idle,
    Run,
    Sprint,
    Jump
}

public class CharacterControllerScript : LocalSingleton<CharacterControllerScript>
{

    PlayerInteract playerInteraction; 



    [Header("Character Movement Settings")] 
    [SerializeField] private CharacterMoveState _characterMoveState = CharacterMoveState.Idle;
    [SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private float _animationTransitionSmoothness = 0.1f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 1.5f;
    [SerializeField] private float _lookSensitivityX = 1f;
    [SerializeField] private float _lookSensitivityY = 1f;
    [SerializeField] private float _minYLookAngle = -90f; 
    [SerializeField] private float _maxYlookAngle = 90f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private bool _canMove = true;
    
    [Space][Header("References")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    
    private Vector3 _velocity;
    private float _verticalRotation = 0f;
    private float _characterCurrentSpeed = 0f;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _characterCurrentSpeed = _characterSpeed;
        _characterMoveState = CharacterMoveState.Idle;
    }

    private void Update()
    {
        if (InventoryManager.Instance.IsInventoryOpen || YesNoPrompt.Instance.IsYesNoPromptOpen || SettingsPanel.Instance.isSettingsOpen || (!_canMove && TutorialManager.Instance.IsTutorialActive))
        {
            _characterMoveState = CharacterMoveState.Idle;
            Idle();
            Fall(Vector3.down);
            return;
        }
       
        HandleMovement();
        HandleMouseLook();

        if (Input.GetKey(KeyCode.T))
        {
            TimeManager.Instance.Tick();
        }
    }

    private void HandleMovement()
    {
        if (IsGrounded() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        var keyBindData = GameManager.Instance.GetPcKeyBindData();
        
        var moveDirection = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
        var isPressingWasd = Input.GetKey(keyBindData.GetMoveForwardKey()) 
                             || Input.GetKey(keyBindData.GetMoveBackwardKey()) 
                             || Input.GetKey(keyBindData.GetMoveLeftKey()) 
                             || Input.GetKey(keyBindData.GetMoveRightKey());

        if (!isPressingWasd)
        {
            moveDirection = new Vector3(0f, moveDirection.y, 0f);
        }

        if (moveDirection.magnitude > 0)
        {
            if (Input.GetKey(keyBindData.GetSprintKey()) && !Input.GetKey(keyBindData.GetMoveBackwardKey()))
            {
                _characterMoveState = CharacterMoveState.Sprint;
            }
            else
            {
                _characterMoveState = CharacterMoveState.Run;
            }
        }
        else
        {
            _characterMoveState = CharacterMoveState.Idle;
        }

        if (Input.GetKeyDown(keyBindData.GetJumpKey()) && IsGrounded())
        {
            _characterMoveState = CharacterMoveState.Jump;
        }

        switch (_characterMoveState)
        {
            case CharacterMoveState.Idle: Idle(); break;
            case CharacterMoveState.Run: Run(moveDirection); break;
            case CharacterMoveState.Sprint: Sprint(moveDirection); break;
            case CharacterMoveState.Jump: Jumping(); break;
            default: Idle(); break;
        }

        Fall(moveDirection);
    }

    private void Fall(Vector3 moveDir)
    {
        if (_velocity.y < 0)
        {
            _velocity.y += _gravity * _fallMultiplier * Time.deltaTime;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }

        _characterController.Move((_velocity + moveDir * _characterCurrentSpeed) * Time.deltaTime);
    }
    private void HandleMouseLook()
    {
        if (_playerCamera != null)
        {
            var mouseX = Input.GetAxis("Mouse X") * _lookSensitivityX;
            var mouseY = Input.GetAxis("Mouse Y") * _lookSensitivityY;

            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, _minYLookAngle, _maxYlookAngle);

            _playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out _, _groundCheckDistance);
    }
    private void Idle()
    {
        _animator.SetFloat(Speed, 0, _animationTransitionSmoothness, Time.deltaTime);
    }

    private void Run(Vector3 moveDirection)
    {
        _characterCurrentSpeed = _characterSpeed;
        _animator.SetFloat(Speed, 0.5f, _animationTransitionSmoothness, Time.deltaTime);
        _characterController.Move(moveDirection * (_characterCurrentSpeed * Time.deltaTime));
    }

    private void Sprint(Vector3 moveDirection)
    {
        _characterCurrentSpeed = _characterSpeed * _sprintMultiplier;
        _animator.SetFloat(Speed, 1f, _animationTransitionSmoothness, Time.deltaTime);
        _characterController.Move(moveDirection * (_characterCurrentSpeed * Time.deltaTime));
    }

    private void Jumping()
    {
        _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        _animator.SetTrigger(Jump);
    }

    public void SetCanMove(bool value) => _canMove = value;
    public bool GetCanMove() => _canMove;
}
