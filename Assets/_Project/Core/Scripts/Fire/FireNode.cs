using UnityEngine;

public class FireNode : MonoBehaviour
{
    [Header("Fire Logic")]
    public float burnRate = 5f; // Lượng nhiên liệu đốt mỗi giây
    public float heatSpreadRadius = 3f; // Bán kính tỏa nhiệt
    public float heatPower = 10f; // Sức nóng truyền cho vật khác mỗi giây

    private FlammableMaterial hostMaterial;
    private float fireHealth = 100f; // Máu của ngọn lửa (Bị trừ khi xịt bình)

    public void Initialize(FlammableMaterial host)
    {
        hostMaterial = host;
        fireHealth = 100f; // Khởi tạo máu lửa
    }

    void Update()
    {
        if (hostMaterial == null || !hostMaterial.isBurning)
        {
            Destroy(gameObject); // Tự hủy nếu vật chủ hết cháy
            return;
        }

        //hostMaterial.ConsumeFuel(burnRate * Time.deltaTime);

        // 2. Tỏa nhiệt lây lan sang các vật xung quanh (Cơ chế cháy lan)
        SpreadHeat();
    }

    private void SpreadHeat()
    {
        // Quét một hình cầu xung quanh ngọn lửa
        Collider[] colliders = Physics.OverlapSphere(transform.position, heatSpreadRadius);
        foreach (Collider col in colliders)
        {
            FlammableMaterial nearbyMaterial = col.GetComponent<FlammableMaterial>();
            
            // Nếu tìm thấy vật dễ cháy khác, và không phải là chính vật chủ của mình
            if (nearbyMaterial != null && nearbyMaterial != hostMaterial)
            {
                // Truyền nhiệt lượng dựa trên thời gian
                nearbyMaterial.AddHeat(heatPower * Time.deltaTime);
            }
        }
    }

    // Hàm nhận bọt tuyết từ bình chữa cháy
    public void TakeDamage(ExtinguisherType extType, float damageAmount)
    {
        float effectiveness = GetExtinguisherEffectiveness(hostMaterial.fireClass, extType);

        if (effectiveness <= 0f)
        {

            return;
        }

        // Trừ máu lửa
        fireHealth -= (damageAmount * effectiveness);

        if (fireHealth <= 0)
        {
            hostMaterial.Extinguish();
        }
    }

    // Ma trận đánh giá hiệu quả PCCC
    private float GetExtinguisherEffectiveness(FireClass fClass, ExtinguisherType eType)
    {
        // Ví dụ: Lửa chất Rắn (A)
        if (fClass == FireClass.A_Solid)
        {
            if (eType == ExtinguisherType.Water || eType == ExtinguisherType.Foam || eType == ExtinguisherType.Powder) 
                return 1f; // Hiệu quả 100%
            if (eType == ExtinguisherType.CO2) 
                return 0.3f; // CO2 kém hiệu quả với lửa rắn (dễ bùng lại)
        }
        
        // Ví dụ: Lửa Điện (E)
        if (fClass == FireClass.E_Electrical)
        {
            if (eType == ExtinguisherType.Water || eType == ExtinguisherType.Foam) 
                return 0f; // NGHIÊM CẤM (Có thể trả về -1 để báo hiệu giật điện)
            if (eType == ExtinguisherType.CO2 || eType == ExtinguisherType.Powder) 
                return 1f; // Chuẩn
        }

        return 1f; // Mặc định
    }

    // Vẽ vòng tròn tầm nhiệt trong Editor để dễ quan sát thiết kế màn chơi
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heatSpreadRadius);
    }
}