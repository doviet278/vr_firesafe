using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health & Survival")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float smokeToxicity = 0f; // Từ 0 đến 100, càng cao màn hình càng đen

    [Header("Weight System")]
    public float maxWeightCapacity = 30f; // Cân nặng tối đa (kg) mang được
    public float currentWeight = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // AI/Dev khác gọi hàm này để gây sát thương
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    // Tính toán tỷ lệ làm chậm dựa trên cân nặng (Trả về từ 0.3 đến 1.0)
    public float GetWeightSpeedMultiplier()
    {
        if (currentWeight <= 0) return 1f;
        float weightRatio = currentWeight / maxWeightCapacity;
        weightRatio = Mathf.Clamp01(weightRatio);
        
        // Ví dụ: Đầy tải (ratio = 1) thì tốc độ còn 30% (0.3f)
        return Mathf.Lerp(1f, 0.3f, weightRatio); 
    }

    private void Die()
    {
        Debug.Log("Player da chet!");
        // Gọi GameManager để hiện màn hình Game Over
    }
}