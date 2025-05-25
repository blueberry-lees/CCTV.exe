using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualManager : MonoBehaviour
{
    [Header("Portraits")]
    public Image femalePortraitImage;
    public Image malePortraitImage;

    [Header("Background")]
    public Image backgroundImage;


    private void Start()
    {
        femalePortraitImage.gameObject.SetActive(false);
        malePortraitImage.gameObject.SetActive(false);
    }
    public void ChangeCharacterExpression(string character, string expression)
    {
        //if (character == "Narrator")
        //{
        //    femalePortraitImage.gameObject.SetActive(false);
        //    malePortraitImage.gameObject.SetActive(false);
        //    return;
        //}

        string path = $"Portraits/{character}/{expression}";
        Sprite portrait = Resources.Load<Sprite>(path);
        if (!portrait)
        {
            Debug.LogWarning($"Portrait not found: {path}");
            return;
        }

        femalePortraitImage.gameObject.SetActive(false);
        malePortraitImage.gameObject.SetActive(false);

        switch (character)
        {
            case "You":
                femalePortraitImage.sprite = portrait;
                femalePortraitImage.gameObject.SetActive(true);
                break;

            case "Killer":
                malePortraitImage.sprite = portrait;
                malePortraitImage.gameObject.SetActive(true);
                break;

            default:
                Debug.Log($"Unknown character: {character}");
                break;
        }
        

    }

    public void ChangeEnvironmentBackground(string backgroundName)
    {
        Sprite bgSprite = Resources.Load<Sprite>($"Backgrounds/{backgroundName}");
        if (bgSprite != null)
        {
            backgroundImage.sprite = bgSprite;
            backgroundImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Background not found: {backgroundName}");
        }
    }
}
