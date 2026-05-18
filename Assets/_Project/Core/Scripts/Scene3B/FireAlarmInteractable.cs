using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireAlarmInteractable : MonoBehaviour, IInteractable
{
    [Header("Alarm Settings")]
    [SerializeField] private AudioClip alarmClip;
    [SerializeField] private int repeatCount = 3;
    [SerializeField] private float repeatDelay = 0f;

    private AudioSource alarmAudioSource;
    private bool isPlayingAlarm;
    private bool hasPlayedAlarm;

    private void Awake()
    {
        alarmAudioSource = GetComponent<AudioSource>();
        alarmAudioSource.playOnAwake = false;
    }

    public string GetInteractPrompt()
    {
        return hasPlayedAlarm ? "Chuong bao chay da kich hoat" : "Nhan F de bat chuong bao chay";
    }

    public void OnInteract()
    {
        if (hasPlayedAlarm || isPlayingAlarm)
        {
            return;
        }

        if (alarmClip == null || alarmAudioSource == null)
        {
            Debug.LogWarning("FireAlarmInteractable chua duoc gan AudioClip hoac AudioSource.");
            return;
        }

        StartCoroutine(PlayAlarmThreeTimes());
    }

    private IEnumerator PlayAlarmThreeTimes()
    {
        isPlayingAlarm = true;

        for (int i = 0; i < repeatCount; i++)
        {
            alarmAudioSource.PlayOneShot(alarmClip);

            float waitTime = alarmClip.length + repeatDelay;
            if (waitTime > 0f)
            {
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null;
            }
        }

        hasPlayedAlarm = true;
        isPlayingAlarm = false;
    }
}