using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SceneFirePrevention : MonoBehaviour
{
    public List<EquipmentItem> allEquipments; // danh sách tất cả thiết bị cần tìm
    private int foundCount = 0;

    [Header("UI")]
    public TextMeshProUGUI progressText;
    public GameObject winPanel;

    void Start()
    {
        UpdateUI();

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void OnEquipmentFound(EquipmentItem item)
    {
        if (item.isFound) return; // tránh cộng nhiều lần

        item.MarkFound();
        foundCount++;

        UpdateUI();

        CheckWin();
    }

    void UpdateUI()
    {
        if (progressText != null)
        {
            progressText.text = foundCount + " / " + allEquipments.Count;
        }
    }

    void CheckWin()
    {
        if (foundCount >= allEquipments.Count)
        {
            Debug.Log("YOU COMPLETED THE MISSION!");

            if (winPanel != null)
                winPanel.SetActive(true);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
