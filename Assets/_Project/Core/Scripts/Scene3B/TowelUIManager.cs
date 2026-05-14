using UnityEngine;

public class TowelUIManager : MonoBehaviour
{
    public static TowelUIManager Instance;

    public GameObject clothOverlay;

    public bool IsClothActive => clothOverlay != null && clothOverlay.activeSelf;

    void Awake()
    {
        Instance = this;
    }

    public void ShowClothOverlay(bool show)
    {
        if (clothOverlay != null)
        {
            clothOverlay.SetActive(show);
        }
    }
}
