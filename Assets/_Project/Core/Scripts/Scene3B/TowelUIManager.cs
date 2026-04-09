using UnityEngine;

public class TowelUIManager : MonoBehaviour
{
    public static TowelUIManager Instance;

    public GameObject clothOverlay;

    void Awake()
    {
        Instance = this;
    }

    public void ShowClothOverlay(bool show)
    {
        clothOverlay.SetActive(show);
    }
}
