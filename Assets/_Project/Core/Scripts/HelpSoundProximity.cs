using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource), typeof(BoxCollider))]
public class HelpSoundProximity : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Transform soundSource;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float minVolume = 0f;
    [SerializeField] private float maxVolume = 0.5f;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<BoxCollider>().isTrigger = true;
        if (soundSource == null)
            soundSource = transform;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (audioSource.isPlaying && playerTransform != null && isPlayerNearby)
        {
            // horizontal distance (ignore Y)
            Vector3 a = playerTransform.position;
            Vector3 b = soundSource.position;
            float distance = Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
            float t = 1f - Mathf.InverseLerp(minDistance, maxDistance, distance);
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, Mathf.Clamp01(t));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if(other.CompareTag("Player"))
            isPlayerNearby = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerNearby = false;
        audioSource.Stop();
    }
}
