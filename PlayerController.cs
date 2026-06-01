using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 4.0f;
    public float SprintSpeed = 6.0f;
    public float SpeedChangeRate = 10.0f; // Acceleration and deceleration
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f; // How fast the character turns to face movement direction

    [Header("Physics")]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;

    [Header("Grounded Check")]
    public float GroundedOffset = -0.2f;
    public float GroundedRadius = 0.25f;
    public LayerMask GroundLayers; // What layers the character uses as ground

    private CharacterController controller;

    // Movement
    private float currentSpeed;

    // Rotation
    private float targetRotation;
    private float rotationVelocity;

    // Physics
    private float verticalVelocity;

    // Grounded Check
    private bool grounded;

    // Getter
    public bool Grounded => grounded; 

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GroundedCheck(); 
    }

    private void GroundedCheck()
    {
        // Set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        // Check all objects located inside the sphere, if any satifies layer condition return true
        grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    public void ExecuteMove(Vector2 moveInput, float targetSpeed)
    {
        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        float speedOffset = 0.1f;

        if (Mathf.Abs(currentSpeed - targetSpeed) > speedOffset) // If the distance between current and target speed is greater speedOffset
        {
            // Get the actual speed based on current and target Speed
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
        }
        else
        {
            currentSpeed = targetSpeed; // Set actual speed to targetSpeed
        }

        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        if (moveInput != Vector2.zero)
        {
            // Target rotation will be direction of player's move input use camera direction as the main axis
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                RotationSmoothTime); // Get the actual rotation for player

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f); // Rotate player
        }

        // Direction the player is heading
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // Move
        controller.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void ExecuteJump()
    {
        // The square root of H * -2 * G = how much velocity needed to reach desired height
        verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
    }

    public void ApplyGravity()
    {
        if (grounded && verticalVelocity < 0) verticalVelocity = -2f; // Prevent verticalVelocity decreases to infinity
        else verticalVelocity += Gravity * Time.deltaTime;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        // Green if the character is on the ground
        if (grounded) Gizmos.color = transparentGreen;
        // Red if in the air
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }
#endif
}
