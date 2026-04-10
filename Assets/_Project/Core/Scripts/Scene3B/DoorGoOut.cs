using UnityEngine;

public class DoorGoOut : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        if(Scene3BManager.Instance.isSceneCompleted)
        {
            winPopup.SetActive(true);
        }
        else
        {
            losePopup.SetActive(true);
        }
    }

}
