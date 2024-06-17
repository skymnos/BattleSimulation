using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraRotationSpeed;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;

    private Vector2 inputMovement;
    private float inputMovementUpDown;

    private float mouseX;
    private float mouseY;
    private bool canLook;
    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = 0;
        yRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // move camera
        orientation.position += (orientation.forward * inputMovement.y + orientation.right * inputMovement.x + orientation.up * inputMovementUpDown) * cameraMoveSpeed;
        transform.position = orientation.position;

        // rotate camera
        if (canLook)
        {
            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -89, 89);

            transform.rotation = Quaternion.SlerpUnclamped(Camera.main.transform.rotation, Quaternion.Euler(xRotation, yRotation, 0), cameraRotationSpeed * Time.deltaTime);
            orientation.rotation = Quaternion.SlerpUnclamped(orientation.rotation, Quaternion.Euler(0, yRotation, 0), cameraRotationSpeed * Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void MoveUpDown(InputAction.CallbackContext context)
    {
        inputMovementUpDown = context.ReadValue<float>();
    }

    public void UnlockLook(InputAction.CallbackContext context)
    {
        canLook = (context.started || context.performed)? true : false;
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 mouse = context.ReadValue<Vector2>();
        mouseX = mouse.x * sensX;
        mouseY = mouse.y * sensY;
    }
}
