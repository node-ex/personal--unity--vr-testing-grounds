using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PersonalInputSystemExperiment_InputActionReference : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController _characterController;
    private bool _jumpPressed;
    private Vector3 _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        jumpAction.action.performed += OnJumpAction;
        jumpAction.action.canceled += OnJumpAction;
        jumpAction.action.Enable();
    }

    private void OnDestroy()
    {
        jumpAction.action.performed -= OnJumpAction;
        jumpAction.action.canceled -= OnJumpAction;
        jumpAction.action.Disable();
    }

    private void Update()
    {
        HandleJumpAndGravity();
    }

    private void HandleJumpAndGravity()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity.y = -0.5f; // Small downward force to stay grounded
            if (_jumpPressed)
            {
                _verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        _verticalVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _jumpPressed = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _jumpPressed = false;
        }
    }
}
