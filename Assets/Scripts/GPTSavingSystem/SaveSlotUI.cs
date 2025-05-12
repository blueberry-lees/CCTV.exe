using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex;
    public Button selectButton;
    public TMP_Text slotLabel;
    public TMP_Text timestampLabel;

    private System.Action<int> onSlotSelected;

    public void Setup(int index, SaveData data, System.Action<int> onSelectCallback)
    {
        slotIndex = index;
        onSlotSelected = onSelectCallback;

        if (data != null)
        {
            slotLabel.text = $"Slot {index + 1}: {data.sceneName}";
            timestampLabel.text = data.timestamp;
        }
        else
        {
            slotLabel.text = $"Slot {index + 1}: <empty>";
            timestampLabel.text = "";
        }

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => onSlotSelected?.Invoke(slotIndex));
    }
}
