using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Speeds")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;
    public float proneSpeed = 1f;
    public float moveSmoothTime = 0.1f;

    [Header("Stance Heights (Tư thế)")]
    public float standingHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float proneHeight = 0.4f;
    public float stanceTransitionSpeed = 10f; // Tốc độ đứng lên/ngồi xuống

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;
    private Vector3 dashDirection;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -15f;

    [Header("Camera")]
    public Transform playerCamera;
    public float mouseSensitivity = 15f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    // Các biến quản lý tư thế hiện tại
    private float targetHeight;
    private float targetCamHeight;

    public bool IsCrouching => Mathf.Abs(targetHeight - crouchHeight) < 0.01f;

    private PlayerStats playerStats;
    public bool allowLook = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        playerStats = GetComponent<PlayerStats>();

        // Khởi tạo tư thế đứng mặc định
        targetHeight = standingHeight;
        targetCamHeight = 1.6f; // Mắt thấp hơn đỉnh đầu một chút
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        if (allowLook)
        {
            HandleLook();
        }

        if (!allowLook) return;

        HandleStance(); // Xử lý thay đổi tư thế
        
        if (dashCooldownCounter > 0) dashCooldownCounter -= Time.deltaTime;

        if (dashCounter > 0)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashCounter -= Time.deltaTime;
        }
        else
        {
            HandleMovement();
            HandleDashInput();
            HandleJump();
        }
    }

    private void HandleLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleStance()
    {
        // Nhấn phím C hoặc Ctrl trái để Ngồi (Toggle)
        if (Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.leftCtrlKey.wasPressedThisFrame)
        {
            if (targetHeight == crouchHeight) SetStance(standingHeight, 1.6f); // Đang ngồi -> Đứng lên
            else SetStance(crouchHeight, 0.8f); // Chuyển sang ngồi
        }

        // Nhấn phím Z để Nằm/Bò (Toggle)
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (targetHeight == proneHeight) SetStance(standingHeight, 1.6f); // Đang nằm -> Đứng lên
            else SetStance(proneHeight, 0.2f); // Chuyển sang nằm sát đất
        }

        // Hiệu ứng mượt mà (Lerp) khi thay đổi chiều cao khung va chạm
        if (Mathf.Abs(controller.height - targetHeight) > 0.01f)
        {
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * stanceTransitionSpeed);
            // Cập nhật tâm của khung va chạm để chân luôn chạm đất
            controller.center = new Vector3(0, controller.height / 2f, 0); 
        }

        // Hiệu ứng mượt mà khi hạ thấp tầm nhìn Camera
        if (Mathf.Abs(playerCamera.localPosition.y - targetCamHeight) > 0.01f)
        {
            float newCamY = Mathf.Lerp(playerCamera.localPosition.y, targetCamHeight, Time.deltaTime * stanceTransitionSpeed);
            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, newCamY, playerCamera.localPosition.z);
        }
    }

    private void SetStance(float height, float camHeight)
    {
        targetHeight = height;
        targetCamHeight = camHeight;
    }

    private void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = 0f; float z = 0f;
        if (Keyboard.current.wKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        

        Vector2 targetDir = new Vector2(x, z).normalized;
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        // Lấy chỉ số làm chậm từ PlayerStats
        PlayerStats stats = GetComponent<PlayerStats>();
        float weightMultiplier = (stats != null) ? stats.GetWeightSpeedMultiplier() : 1f;


        // Xác định tốc độ hiện tại dựa trên tư thế
        float currentSpeed = walkSpeed;
        if (targetHeight == proneHeight) currentSpeed = proneSpeed;
        else if (targetHeight == crouchHeight) currentSpeed = crouchSpeed;
        else if (Keyboard.current.leftShiftKey.isPressed) currentSpeed = sprintSpeed; // Chỉ cho phép chạy khi đang đứng

        if (playerStats != null)
        {
            currentSpeed = currentSpeed * playerStats.GetWeightSpeedMultiplier();
        }

        Vector3 move = transform.right * currentDir.x + transform.forward * currentDir.y;
        controller.Move(move * currentSpeed * Time.deltaTime * weightMultiplier);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleDashInput()
    {
        // Chỉ cho phép lướt khi đang đứng hoặc ngồi (không cho lướt khi đang nằm bò)
        if (Keyboard.current.leftAltKey.wasPressedThisFrame && dashCooldownCounter <= 0 && controller.isGrounded && targetHeight != proneHeight)
        {
            dashCounter = dashDuration;
            dashCooldownCounter = dashCooldown;
            dashDirection = currentDir.sqrMagnitude > 0 ? (transform.right * currentDir.x + transform.forward * currentDir.y).normalized : transform.forward;
        }
    }

    private void HandleJump()
    {
        // Hủy tư thế ngồi/nằm nếu bấm Space, hoặc nhảy nếu đang đứng
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            if (targetHeight != standingHeight)
            {
                SetStance(standingHeight, 1.6f); // Bấm nhảy lúc đang ngồi/nằm sẽ tự đứng dậy
            }
            else
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }
}