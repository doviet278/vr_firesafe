using UnityEngine;

public class HazardDamageZone : MonoBehaviour
{
    public enum HazardType { Smoke, Fire }

    [SerializeField] private HazardType hazardType;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float smokeToxicityPerSecond = 10f;
    [SerializeField, Range(0f, 1f)] private float crouchSmokeDamageMultiplier = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player == null) return;

        bool usingCloth = TowelUIManager.Instance != null && TowelUIManager.Instance.IsClothActive;
        if (hazardType == HazardType.Smoke && usingCloth) return;

        float finalDamage = damagePerSecond;

        if (hazardType == HazardType.Smoke)
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null && movement.IsCrouching)
            {
                finalDamage *= crouchSmokeDamageMultiplier;
            }
        }

        player.TakeDamage(finalDamage * Time.deltaTime);

        if (hazardType == HazardType.Smoke)
        {
            player.smokeToxicity = Mathf.Clamp(player.smokeToxicity + smokeToxicityPerSecond * Time.deltaTime, 0f, 100f);
        }
    }
}