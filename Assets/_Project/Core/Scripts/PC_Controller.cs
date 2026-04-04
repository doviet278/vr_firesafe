using UnityEngine;
using UnityEngine.InputSystem; // Thêm thư viện hệ thống mới

[RequireComponent(typeof(CharacterController))]
public class PC_Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Camera Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 15f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Phải luôn kiểm tra xem bàn phím/chuột có đang kết nối không
        if (Keyboard.current == null || Mouse.current == null) return;

        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        // Đọc khoảng cách di chuyển của chuột (delta) theo hệ thống mới
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        // Xây dựng trục X và Z từ các phím WASD
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.wKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        // Chuẩn hóa vector để đi chéo không bị nhanh hơn đi thẳng
        Vector3 inputDirection = new Vector3(x, 0, z).normalized;
        Vector3 move = transform.right * inputDirection.x + transform.forward * inputDirection.z;
        
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}