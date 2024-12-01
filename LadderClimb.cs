using UnityEngine;

public class LadderClimb: MonoBehaviour
{
    public float climbSpeed = 3f; // Speed at which the player climbs
    private bool isClimbing = false; // Is the player on the ladder?
    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }

    void Update()
    {
        if (isClimbing)
        {
            // Capture vertical input for climbing
            float verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow Keys

            // Create climbing motion (vertical movement)
            moveDirection = new Vector3(0, verticalInput * climbSpeed, 0);

            // Apply movement using the CharacterController
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            // Reset vertical movement when not climbing
            moveDirection = Vector3.zero;
        }
    }
}
