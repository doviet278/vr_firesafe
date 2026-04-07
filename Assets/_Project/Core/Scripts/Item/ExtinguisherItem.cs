using UnityEngine;
using UnityEngine.InputSystem; // Thêm thư viện này để check nhả chuột

public class ExtinguisherItem : ItemBase
{
    [Header("Extinguisher Settings")]
    public ExtinguisherType extType = ExtinguisherType.CO2; // Loại bình (CO2, Bột, Nước)
    public float sprayDistance = 5f; // Tầm xịt
    public float damagePerSecond = 50f; // Sức dập lửa (Máu lửa mặc định là 100)

    [Header("Effects")]
    public ParticleSystem foamParticles; // Hiệu ứng bọt tuyết
    private Camera cam;
    void Awake()
    {
        cam = Camera.main;
        // Gán thông tin mặc định cho bình
        itemName = "Binh Chua Chay " + extType.ToString();
        itemWeight = 10f; 
    }

    public override void OnEquip()
    {
        base.OnEquip();
        if (foamParticles != null) foamParticles.Stop();
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        if (foamParticles != null) foamParticles.Stop();
    }

    // Hàm này giờ đây sẽ được gọi LIÊN TỤC mỗi khung hình khi bạn GIỮ chuột phải
    public override void UseItem()
    {
        // 1. Chạy hiệu ứng hạt bọt tuyết
        if (foamParticles != null && !foamParticles.isPlaying)
        {
            foamParticles.Play();
        }

        // 2. Bắn tia Raycast để dập lửa
        Transform camTransform = cam.transform;
        Ray ray = new Ray(camTransform.position, camTransform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, sprayDistance);

        foreach (RaycastHit hit in hits)
        {
            Debug.LogError(hit.collider);
            // Kiểm tra xem tia ngắm có chạm trúng vật thể nào chứa code FireNode không
            FireNode fire = hit.collider.GetComponent<FireNode>();
            if (fire != null)
            {
                fire.TakeDamage(extType, damagePerSecond * Time.deltaTime);
               
                break; 
            }
        }
    }

    void Update()
    {
        // Xử lý tắt bọt tuyết khi nhả chuột phải
        if (isEquipped && Mouse.current != null)
        {
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                if (foamParticles != null) foamParticles.Stop();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Transform camTransform = cam.transform;

        // Set màu tia (đỏ cho dễ thấy)
        Gizmos.color = Color.red;

        // Vẽ tia raycast
        Gizmos.DrawRay(camTransform.position, camTransform.forward * sprayDistance);
    }
}