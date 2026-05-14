using UnityEngine;


public class SoundError : MonoBehaviour
{
    public static SoundError Instance;
    [Header("Audio Sources")]
    public AudioSource wrongAudio;

    [Header("Settings")]
    public float wrongCooldown = 0.5f;

    private float lastWrongTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayWrong()
    {
        if (wrongAudio == null) return;

        if (Time.time - lastWrongTime > wrongCooldown)
        {
            if (wrongAudio.isPlaying) {
                return;
            }
            wrongAudio.Play();
            lastWrongTime = Time.time;
        }
    }
}
