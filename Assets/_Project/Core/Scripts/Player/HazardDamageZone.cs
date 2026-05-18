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

        float finalDamage = damagePerSecond;

        if (hazardType == HazardType.Smoke)
        {
            // If player is using the cloth (towel) over face, reduce smoke damage by 90% (keep 10%)
            if (usingCloth)
            {
                finalDamage *= 0.1f; // 90% reduction
            }

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