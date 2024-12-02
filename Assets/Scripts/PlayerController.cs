using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 5f;  // Speed at which the character rotates
    private Animator animator;  // Reference to the Animator
    private float moveInput;    // Store input from player

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Get horizontal input (A/D or left arrow/right arrow)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Apply movement only on the X-axis
        transform.Translate(Vector3.right * moveInput * moveSpeed * Time.deltaTime);

        // Calculate desired position along the X-axis, but clamped to the specified range
        Vector3 newPosition = transform.position;

        // Lock Y and Z position
        newPosition.z = 0;  // Lock Z position to 0
        newPosition.y = transform.position.y; // Keep current Y value (e.g., for jumping)

        // Move along X with input, but clamp to -8 and +8
        newPosition.x = Mathf.Clamp(newPosition.x + moveInput * moveSpeed * Time.deltaTime, -3.3f, 3.3f);

        // Apply the modified position
        transform.position = newPosition;

        // Update animation states
        HandleAnimations();
        RotateCharacter();
    }


    void RotateCharacter()
    {
        if (moveInput > 0)
        {
            // Rotate character to face 45 degrees to the right (clockwise)
            Quaternion targetRotation = Quaternion.Euler(0, 45, 0);  // 45 degrees to the right
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput < 0)
        {
            // Rotate character to face 45 degrees to the left (counter-clockwise)
            Quaternion targetRotation = Quaternion.Euler(0, -45, 0);  // 45 degrees to the left
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // If no input, rotate character back to facing forward (0 degrees)
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);  // Facing straight ahead
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
    }

    void HandleAnimations()
    {
        // Set runDirection parameter based on movement direction
        if (moveInput > 0)
        {
            animator.SetInteger("runDirection", 1); // right
        }
        else if (moveInput < 0)
        {
            animator.SetInteger("runDirection", -1); // -1 for moving left
        } else if (moveInput == 0)
        {
            animator.SetInteger("runDirection", 0);
        }
    }
}