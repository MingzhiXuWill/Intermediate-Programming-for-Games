using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController characterController;

    float speed = 5f;

    float mouseSensitivity = 3.5f;

    Transform cameraTrans;
    float cameraPitch = 0;

    // Gravity
    float gravityValue = Physics.gravity.y;
    float jumpHeight = -2f;

    float currentYVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        cameraTrans = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotate horizontally
        Vector2 MouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        transform.Rotate(Vector3.up * MouseDelta.x * mouseSensitivity);

        cameraPitch -= MouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
        cameraTrans.localEulerAngles = Vector3.right * cameraPitch;

        Vector3 move = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (characterController.isGrounded && currentYVelocity < 0)
        {
            // Player landed
            currentYVelocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            // Jump
            currentYVelocity += Mathf.Sqrt(2 * jumpHeight * gravityValue);
        }
        // Calculate Gravity
        currentYVelocity += gravityValue * Time.deltaTime;

        move.y = currentYVelocity;
        characterController.Move(move * speed * Time.deltaTime);
    }
}
