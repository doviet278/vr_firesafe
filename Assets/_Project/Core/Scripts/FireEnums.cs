// Không cần using UnityEngine ở đây
public enum FireClass
{
    A_Solid,      // Rắn: Gỗ, giấy, vải
    B_Liquid,     // Lỏng: Xăng, dầu, cồn
    C_Gas,        // Khí: Gas, Metan
    E_Electrical, // Điện: Thiết bị điện có điện
    K_CookingOil  // Dầu ăn (Bếp)
}

public enum ExtinguisherType
{
    Water,        // Nước (Tốt cho A, cấm dùng cho E và B)
    CO2,          // Khí CO2 (Tốt cho B, C, E)
    Powder,       // Bột tổng hợp (Đa năng ABC)
    Foam,          // Bọt tuyết (Tốt cho A, B)
    Chemiscal
}