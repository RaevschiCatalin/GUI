using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20;
    public float lookSensitivity = 0.2f;

    public Transform playerCamera;

    private PlayerInputActions input;
    private CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float verticalLookRotation = 0f;

    void Awake()
    {
        input = new PlayerInputActions();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Poll the current 2D value every frame
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        // Build a 3D direction, normalize so diagonal isn't faster
        Vector3 rawDir = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 dir = rawDir.sqrMagnitude > 1f
            ? rawDir.normalized
            : rawDir;

        Vector3 worldDir = transform.TransformDirection(dir);
        controller.Move(worldDir * moveSpeed * Time.deltaTime);

        HandleLook();
    }


    void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleLook()
    {
        verticalLookRotation -= lookInput.y * lookSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }
}
