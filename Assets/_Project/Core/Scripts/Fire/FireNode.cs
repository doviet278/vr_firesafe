using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireNode : MonoBehaviour
{
    [Header("Fire Logic")]
    public float burnRate = 5f; // Lượng nhiên liệu đốt mỗi giây
    public float heatSpreadRadius = 3f; // Bán kính tỏa nhiệt
    public float heatPower = 10f; // Sức nóng truyền cho vật khác mỗi giây

    [Header("Fire Sound")]
    [SerializeField] private float soundMaxDistance = 5f;
    [SerializeField] private float soundMinVolume = 0f;
    [SerializeField] private float soundMaxVolume = 0.8f;

    private FlammableMaterial hostMaterial;
    private float fireHealth = 100f;
    private AudioSource fireAudioSource;
    private Transform playerTransform;
    private SoundError soundError;

    private void Awake()
    {
        fireAudioSource = GetComponent<AudioSource>();
        if (fireAudioSource != null)
        {
            fireAudioSource.playOnAwake = false;
            fireAudioSource.loop = true;
            fireAudioSource.Stop();
        }

        soundError = SoundError.Instance;
    }

    private void Start()
    {
        ResolvePlayerTransform();
    }

    public void Initialize(FlammableMaterial host)
    {
        hostMaterial = host;
        fireHealth = 100f; 
    }

    void Update()
    {
        if (hostMaterial == null || !hostMaterial.isBurning)
        {
            StopFireSound();
            Destroy(gameObject); 
            return;
        }

        ResolvePlayerTransform();
        UpdateFireSound();

        //hostMaterial.ConsumeFuel(burnRate * Time.deltaTime);

        //SpreadHeat();
    }

    private void ResolvePlayerTransform()
    {
        if (playerTransform != null)
            return;

        if (Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }
    }

    private void UpdateFireSound()
    {
        if (fireAudioSource == null || playerTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance > soundMaxDistance)
        {
            if (fireAudioSource.isPlaying)
            {
                fireAudioSource.Stop();
            }

            return;
        }

        float t = 1f - Mathf.InverseLerp(0f, soundMaxDistance, distance);
        fireAudioSource.volume = Mathf.Lerp(soundMinVolume, soundMaxVolume, Mathf.Clamp01(t));

        if (!fireAudioSource.isPlaying && fireAudioSource.clip != null)
        {
            fireAudioSource.Play();
        }
    }

    private void StopFireSound()
    {
        if (fireAudioSource != null && fireAudioSource.isPlaying)
        {
            fireAudioSource.Stop();
        }
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
            soundError?.PlayWrong();
            return;
        }

        // Trừ máu lửa
        fireHealth -= (damageAmount * effectiveness);

        if (fireHealth <= 0)
        {
            hostMaterial.Extinguish();
        }
    }

    private float GetExtinguisherEffectiveness(FireClass fClass, ExtinguisherType eType)
    {
        switch (fClass)
        {
            case FireClass.A_Solid:
                // Gỗ, giấy, vải
                if (eType == ExtinguisherType.Water ||
                    eType == ExtinguisherType.Foam ||
                    eType == ExtinguisherType.Powder)
                    return 1f;
                if (eType == ExtinguisherType.CO2)
                    return 0.3f;
                return 0f;

            case FireClass.B_Liquid:
                // Xăng, dầu
                if (eType == ExtinguisherType.CO2 ||
                    eType == ExtinguisherType.Foam ||
                    eType == ExtinguisherType.Powder)
                    return 1f;
                return 0f;

            case FireClass.C_Gas:
                // Gas
                if (eType == ExtinguisherType.CO2 ||
                    eType == ExtinguisherType.Powder)
                    return 1f;
                return 0f;

            case FireClass.E_Electrical:
                // Điện
                if (eType == ExtinguisherType.CO2 ||
                    eType == ExtinguisherType.Powder||
                    eType == ExtinguisherType.Chemiscal)
                    return 1f;
                return 0f;

            case FireClass.K_CookingOil:
                // Dầu ăn (bếp)
                if (eType == ExtinguisherType.Chemiscal)
                    return 1f;
                return 0f;
        }

        return 0f; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heatSpreadRadius);
    }
}