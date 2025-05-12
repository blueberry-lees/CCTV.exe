using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class SaveSlot : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI slotInfoText;

    public string ProfileId { get; private set; }

    private SaveSlotsMenu menu;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Called by SaveSlotsMenu
    public void Initialize(SaveSlotsMenu menuRef, string profileId, GameData data, bool isLoading)
    {
        menu = menuRef;
        ProfileId = profileId;

        if (data == null)
        {
            slotInfoText.text = "No saved data.";
        }
        else
        {
            string timePlayed = data.GetFormattedPlayTime();
            int chapter = data.storyProgress;
            slotInfoText.text = $"{timePlayed} Time Played\nChapter: {chapter}";
        }

        bool hasData = data != null;
        SetInteractable(hasData || !isLoading);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    // Hook this to NavigableOption.onSelected
    public void OnSelected()
    {
        if (button.interactable && menu != null)
        {
            menu.OnSlotSelected(this);
        }
    }
}

