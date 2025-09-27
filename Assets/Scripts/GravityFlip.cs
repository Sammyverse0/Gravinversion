using UnityEngine;
using UnityEngine.InputSystem;

// Attach this script to the same object that has the FirstPersonController
[RequireComponent(typeof(CharacterController))]
public class GravityController : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("The strength of the gravitational pull.")]
    public float gravityForce = 20.0f;
    [Tooltip("How long in seconds before the player can flip gravity again.")]
    public float flipCooldown = 2.0f; // NEW: Cooldown duration
    [Tooltip("Layers that the character will consider as ground or ceiling.")]
    public LayerMask groundLayers;

    [Header("Rotation Settings")]
    [Tooltip("How quickly the camera rotates when flipping gravity.")]
    public float rotationSpeed = 8f;
    [Tooltip("Assign the main player camera here.")]
    public Transform playerCamera;

    // Private variables
    private CharacterController _characterController;
    private bool _isFlipped = false;
    private float _verticalVelocity = 0f;
    private Quaternion _cameraTargetRotation;
    private float _cooldownTimer = 0f; // NEW: Timer to track cooldown

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (playerCamera != null)
        {
            _cameraTargetRotation = playerCamera.localRotation;
        }
        else
        {
            Debug.LogError("Player Camera is not assigned in the GravityController script!");
        }
    }

    void Update()
    {
        // NEW: Update the cooldown timer every frame
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        // NEW: Check for input AND if the cooldown is over
        if (Keyboard.current.fKey.wasPressedThisFrame && _cooldownTimer <= 0)
        {
            FlipGravity();
        }

        HandleGravity();
        HandleRotation();
    }

    private void FlipGravity()
    {
        // NEW: Reset the cooldown timer when the ability is used
        _cooldownTimer = flipCooldown;

        _isFlipped = !_isFlipped;
        _verticalVelocity = 0.1f * (_isFlipped ? 1 : -1);

        if (_isFlipped)
        {
            _cameraTargetRotation = Quaternion.Euler(playerCamera.localEulerAngles.x, playerCamera.localEulerAngles.y, 180f);
        }
        else
        {
            _cameraTargetRotation = Quaternion.Euler(playerCamera.localEulerAngles.x, playerCamera.localEulerAngles.y, 0f);
        }
    }

    private void HandleGravity()
    {
        bool isGrounded = CheckGroundedOrCeilinged();

        if (isGrounded && _verticalVelocity < 0.1f && !_isFlipped)
        {
            _verticalVelocity = -2f; 
        }
        else if(isGrounded && _verticalVelocity > -0.1f && _isFlipped)
        {
             _verticalVelocity = 2f;
        }
        
        float gravityDirection = _isFlipped ? 1.0f : -1.0f;
        _verticalVelocity += (gravityForce * gravityDirection) * Time.deltaTime;

        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    private bool CheckGroundedOrCeilinged()
    {
        float radius = _characterController.radius * 0.9f;
        float distance = _characterController.height / 2 - radius + 0.2f;
        Vector3 position = transform.position + _characterController.center;
        Vector3 direction = _isFlipped ? Vector3.up : Vector3.down;
        
        return Physics.SphereCast(position, radius, direction, out RaycastHit hit, distance, groundLayers);
    }

    private void HandleRotation()
    {
        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Lerp(playerCamera.localRotation, _cameraTargetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}