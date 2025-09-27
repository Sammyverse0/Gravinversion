using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets; // FPC namespace

[RequireComponent(typeof(CharacterController))]
public class GravityFlip_New : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float gravityForce = 20f; // how strong gravity pulls
    private bool isFlipped = false;
    private Quaternion targetRotation;
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        targetRotation = transform.rotation;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Flip when pressing F
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            FlipGravity();
        }

        // Apply custom gravity
        if (isFlipped)
        {
            // Pull upward (towards ceiling)
            if (!controller.isGrounded)
                velocity.y += gravityForce * Time.deltaTime;
            else
                velocity.y = 0;
        }
        else
        {
            // Pull downward (towards floor)
            if (!controller.isGrounded)
                velocity.y -= gravityForce * Time.deltaTime;
            else
                velocity.y = 0;
        }

        controller.Move(velocity * Time.deltaTime);

        // Smooth rotate player body
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void FlipGravity()
    {
        isFlipped = !isFlipped;

        // Rotate player
        if (isFlipped)
            targetRotation = Quaternion.Euler(180f, transform.eulerAngles.y, 0f);
        else
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
