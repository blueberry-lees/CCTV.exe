using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]

    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;

    [SerializeField] private GameObject hasDataContent;

    [SerializeField] private TextMeshProUGUI  percentageCompleteText;

    [SerializeField] private TextMeshProUGUI progressText;

    public void SetData(GameData data)
    {
        //there's no data for this profileId
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        //there's data for this profile
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            //TODO: remeber to fetch data in GetTimePlayed() in GameData.cs
            percentageCompleteText.text = data.GetFormattedPlayTime() + "Time Played";
            progressText.text = "Story Progress:" + data.storyProgress;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

}
