using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInteraction playerInteraction;
    private PlayerInput playerInput;

    private float speed = 2.5f;

    private float mouseSensitivity = 10;

    private Camera cam;
    private float camRot = 0;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private float smoothInputSpeed = .05f;

    private float sneakHeight;

    private void Awake()
    {
        cam = Camera.main;
        playerInput = new PlayerInput();

        sneakHeight = transform.position.y;

        playerInput.CharacterControls.Run.performed += a => speed = 5f;
        playerInput.CharacterControls.Run.canceled += a => speed = 2.5f;
        playerInput.CharacterControls.Sneak.performed += a => sneakHeight = .6f;
        playerInput.CharacterControls.Sneak.canceled += a => sneakHeight = .97f;

        playerInput.CharacterControls.Interaction.performed += ctx => playerInteraction.CheckForInteraction();

        controller = GetComponent<CharacterController>();
        playerInteraction = GetComponent<PlayerInteraction>();
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Turn();
        transform.position = new Vector3(transform.position.x, sneakHeight, transform.position.z);
    }

    private void Move()
    {
        float x = playerInput.CharacterControls.Movement.ReadValue<Vector2>().x;
        float z = playerInput.CharacterControls.Movement.ReadValue<Vector2>().y;

        Vector3 direction = transform.right * x + transform.forward * z;

        controller.Move(direction * speed * Time.deltaTime);
    }

    private void Turn()
    {
        Vector2 input = playerInput.CharacterControls.Turning.ReadValue<Vector2>() * mouseSensitivity;
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);

        float mouseX = currentInputVector.x;
        float mouseY = currentInputVector.y;

        camRot -= mouseY;

        camRot = Mathf.Clamp(camRot, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(camRot, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void SetMouseSensitivity(float mouseSens)
    {
        mouseSensitivity = mouseSens;
    }
}