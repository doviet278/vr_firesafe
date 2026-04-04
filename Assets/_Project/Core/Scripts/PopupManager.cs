using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private Button btnStart;

    private void Start()
    {
        btnStart.onClick.AddListener(ShowPopup);
    }

    private void ShowPopup()
    {
        Debug.LogError("clickPopup");
        item.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btnStart.onClick.RemoveListener(ShowPopup);
    }

}
