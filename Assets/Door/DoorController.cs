using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public Animator animator;
    public string openTriggerName = "Open";
    public string closeTriggerName = "Close"; // Thêm Trigger để chạy animation đóng cửa

    [Header("Audio Settings")]
    public AudioSource doorAudioSource; // Nguồn phát âm thanh
    public AudioClip openSound;         // Tiếng mở cửa
    public AudioClip closeSound;        // Tiếng đóng cửa

    private bool isOpen = false;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        
        // Tự động tìm AudioSource nếu bạn quên kéo thả trong Inspector
        if (doorAudioSource == null)
        {
            doorAudioSource = GetComponent<AudioSource>();
        }
    }

    // Đã sửa lỗi NotImplementedException
    public string GetInteractPrompt()
    {
        // Hiển thị chữ linh hoạt theo trạng thái cửa
        return isOpen ? "Nhan F de DONG cua" : "Nhan F de MO cua";
    }

    public void OnInteract()
    {
        if (animator != null)
        {
            // Đảo ngược trạng thái cửa (Đang mở -> Đóng, Đang đóng -> Mở)
            isOpen = !isOpen;

            if (isOpen)
            {
                animator.SetTrigger(openTriggerName);
                PlaySound(openSound);
                
                // LƯU Ý: Không nên tắt boxCollider nếu bạn muốn bấm F để đóng lại.
                boxCollider.enabled = false; 
            }
            else
            {
                animator.SetTrigger(closeTriggerName);
                PlaySound(closeSound);
                
                boxCollider.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("Animator chua duoc gan cho Door!");
        }
    }

    // Hàm hỗ trợ phát âm thanh không bị đè lên nhau
    private void PlaySound(AudioClip clip)
    {
        if (doorAudioSource != null && clip != null)
        {
            doorAudioSource.PlayOneShot(clip);
        }
    }
}