using UnityEngine;

public class ItemSelectScene2A : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject nextArrow;
    private SceneTrainRecog sceneTrainRecog;
    private GameObject obj;
    private void Awake()
    {
        obj = GameObject.Find("SceneTrainRecog");
        if(obj != null)
        {
            sceneTrainRecog = obj.GetComponent<SceneTrainRecog>();
        }
    }
    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        popup.SetActive(true);
        sceneTrainRecog.CheckCompleted();
        if (nextArrow != null )
        {
            nextArrow.SetActive(true);
        }
    }
}
