using UnityEngine;

// TUÂN THỦ BLUEPRINT: Kế thừa ItemBase
public class TestExtinguisher : ItemBase
{
    [Header("Extinguisher FX")]
    public ParticleSystem foamParticles; // Hiệu ứng xịt bọt

    void Awake()
    {
        // Định nghĩa thông số Core cho vật phẩm này
        itemName = "Binh CO2 (TEST)";
        itemWeight = 10f; // Nặng 10kg
    }

    // TUÂN THỦ BLUEPRINT: Ghi đè hàm su dung
    public override void UseItem()
    {
        // Gọi logic cơ bản (chỉ để Debug Log)
        base.UseItem();

        // Viết logic riêng của Binh chua chay
        if (foamParticles != null)
        {
            // Xịt bọt trong 1 giây rồi tắt
            foamParticles.Play();
            CancelInvoke("StopFoam");
            Invoke("StopFoam", 1f);
        }

        Debug.Log("--- DANG XIT BOT CHUA CHAY (TEST) ---");
    }

    private void StopFoam()
    {
        if (foamParticles != null) foamParticles.Stop();
    }
}