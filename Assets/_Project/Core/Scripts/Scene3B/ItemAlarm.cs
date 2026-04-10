using UnityEngine;

public class ItemAlarm : MonoBehaviour,IInteractable
{
    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        Scene3BManager.Instance.SetUsedAlarm();
    }

}
