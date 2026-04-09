using UnityEngine;

public class ItemSelect3A : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject nextArrow;
    private Scene3AManager scene3A;

    private void Awake()
    {
        GameObject obj = GameObject.Find("Scene3AManager");
        if (obj != null)
        {
            scene3A = obj.GetComponent<Scene3AManager>();
        }
    }
    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        popup.SetActive(true);
        scene3A.CheckCompleted();
        if (nextArrow != null)
        {
            nextArrow.SetActive(true);
        }
    }
}
