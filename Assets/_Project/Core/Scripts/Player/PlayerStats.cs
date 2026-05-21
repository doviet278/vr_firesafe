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


    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        if (currentHealth <= 0f) Die();
    }

    // Tính toán tỷ lệ làm chậm dựa trên cân nặng (Trả về từ 0.3 đến 1.0)
    public float GetWeightSpeedMultiplier()
    {
        if (currentWeight <= 0) return 1f;
        float weightRatio = currentWeight / maxWeightCapacity;
        weightRatio = Mathf.Clamp01(weightRatio);
        return Mathf.Lerp(1f, 0.3f, weightRatio); 
    }

    private void Die()
    {
        Debug.Log("Player da chet!");
    
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

         if (UIScenPratice.Instance != null)
        {
            UIScenPratice.Instance.ShowLosePopup();
        }
    }
}